using MailKit.Net.Smtp;
using MimeKit;
using TWMSServer.Model;
using TWMSServer.Providers.Secrets;
using HandlebarsDotNet;
using Quartz;
using TWMSServer.Jobs;
using HandlebarsDotNet.Extension.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;

namespace TWMSServer.Providers
{
    public class EmailProvider
    {
        private readonly ILogger<EmailProvider> _logger;
        private readonly IHandlebars _handlebars;
        private readonly TWMSServerContext _context;
        private readonly ISchedulerFactory _schedulerFactory;

        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;
        private readonly string fromEmail;
        private readonly string baseURL;

        private readonly List<string>? AllowedEmails;
        private bool IsConfigured => !(string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpUsername) || string.IsNullOrWhiteSpace(smtpPassword) || string.IsNullOrWhiteSpace(fromEmail)) && (smtpPort == 25 || smtpPort == 465 || smtpPort == 587);

        public EmailProvider(ILogger<EmailProvider> logger, IConfiguration configuration, ISecretsProvider secretsProvider, ISchedulerFactory schedulerFactory, IDbContextFactory<TWMSServerContext> factory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _context = factory.CreateDbContext();
            
            string layoutTemplate = File.ReadAllText(Path.Combine("EmailTemplates", "EmailLayout.hbs"));

            _handlebars = Handlebars.Create();
            _handlebars.Configuration.UseJson();
            _handlebars.RegisterTemplate("layout", layoutTemplate);

            smtpHost = configuration["SmtpSettings:Host"] ?? "";
            smtpPort = int.Parse(configuration["SmtpSettings:Port"] ?? "25");
            smtpUsername = configuration["SmtpSettings:Username"] ?? "";
            smtpPassword = secretsProvider.GetSMTPPassword();
            fromEmail = configuration["SmtpSettings:FromEmail"] ?? "";
            baseURL = configuration["URLs:FrontendURL"] ?? "";

            AllowedEmails = configuration.GetSection("EmailSettings:AllowList").Get<List<string>>()?.Select(x => x.ToLower()).ToList();
        }

        private string RenderTemplate(string templateName, string data)
        {            
            var model = JsonNode.Parse(data);
            model!["companyName"] = "Trinidad and Tobago Electricity Commission";
            model!["currentYear"] = DateTime.UtcNow.Year.ToString();
            model!["logoURL"] = "https://portal.ttec.co.tt/logo-small.png";
            string templateContent = File.ReadAllText(Path.Combine("EmailTemplates", $"{templateName}.hbs"));
            var template = _handlebars.Compile(templateContent);
            return template(model);
        }

        public async Task QueueEmail(int emailDataId)
        {
            var jobId = Guid.NewGuid().ToString();
            var job = JobBuilder.Create<SendEmailJob>()
                .WithIdentity(jobId, "send-email")
                .UsingJobData("EmailDataId", emailDataId)
                .Build();
            var trigger = TriggerBuilder.Create().StartNow().Build();
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.ScheduleJob(job, trigger);
        }
        
        public async Task<bool> SendEmail(int emailDataId)
        {
            if (!IsConfigured)
            {
                _logger.LogWarning("Attempted to send email for email data with id {EmailDataId} but not configured to send mail!", emailDataId);
                return false;
            }

            EmailData? emailData = await _context.Emails
                .Include(e => e.Attachments)
                .FirstOrDefaultAsync(e => e.EmailDataId == emailDataId);

            if (emailData == null)
            {
                _logger.LogError("Error encountered while sending email: No email with id of {EmailDataId}", emailDataId);
                return false;
            }

            try
            {
                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);

                var message = new MimeMessage();
                message.To.AddRange(emailData.ParsedToRecipients);

                if (message.To.Count == 0)
                {
                    _logger.LogWarning("No valid email addresses found for email data with id {EmailDataId}!", emailData.EmailDataId);
                    return false;
                }

                message.From.Add(new MailboxAddress("T&TEC", fromEmail));
                message.Subject = emailData.Subject;
                var bodyHtml = RenderTemplate(emailData.TemplateName, emailData.TemplateDataRaw);

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = bodyHtml
                };

                // Add attachments if any
                if (emailData.Attachments != null && emailData.Attachments.Any())
                {
                    foreach (var attachment in emailData.Attachments)
                    {
                        if (File.Exists(attachment.FilePath))
                        {
                            var contentType = string.IsNullOrEmpty(attachment.ContentType)
                                ? null
                                : new ContentType("application", "octet-stream");
                                
                            bodyBuilder.Attachments.Add(
                                attachment.OriginalFileName,
                                File.ReadAllBytes(attachment.FilePath),
                                contentType);
                        }
                    }
                }

                message.Body = bodyBuilder.ToMessageBody();

                await client.SendAsync(message);
                emailData.SentSuccessfullyAt = DateTime.UtcNow;
                await client.DisconnectAsync(true);
                _logger.LogInformation("Successfully sent email for email data with id {EmailDataId} and template {EmailTemplate}", emailData.EmailDataId, emailData.TemplateName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email for email data with id {EmailDataId}", emailData.EmailDataId);
                return false;
            }
            finally
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<bool>> SendEmails(params EmailData[] emails)
        {
            if (!IsConfigured)
            {
                _logger.LogWarning("Attempted to send emails but not configured to send mail!");
                return emails.Select(e => false).ToList();
            }

            List<bool> result = [];

            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUsername, smtpPassword);

            foreach (var emailData in emails)
            {
                try
                {
                    var message = new MimeMessage();
                    message.To.AddRange(emailData.ParsedToRecipients);

                    if (message.To.Count == 0)
                    {
                        _logger.LogWarning("No valid email addresses found for email data with id {EmailDataId}!", emailData.EmailDataId);
                        result.Add(false);
                        continue;
                    }

                    message.From.Add(new MailboxAddress("T&TEC", fromEmail));
                    message.Subject = emailData.Subject;
                    var bodyHtml = RenderTemplate(emailData.TemplateName, emailData.TemplateDataRaw);

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = bodyHtml
                    };

                    // Add attachments if any
                    if (emailData.Attachments != null && emailData.Attachments.Any())
                    {
                        foreach (var attachment in emailData.Attachments)
                        {
                            if (File.Exists(attachment.FilePath))
                            {
                                var contentType = string.IsNullOrEmpty(attachment.ContentType)
                                    ? null
                                    : new ContentType("application", "octet-stream");
                                    
                                bodyBuilder.Attachments.Add(
                                    attachment.OriginalFileName,
                                    File.ReadAllBytes(attachment.FilePath),
                                    contentType);
                            }
                        }
                    }

                    message.Body = bodyBuilder.ToMessageBody();
                    await client.SendAsync(message);
                    emailData.SentSuccessfullyAt = DateTime.UtcNow;
                    result.Add(true);
                    _logger.LogInformation("Successfully sent email for email data with id {EmailDataId} and template {EmailTemplate}", emailData.EmailDataId, emailData.TemplateName);
                }
                catch (Exception ex)
                {
                    result.Add(false);
                    _logger.LogError(ex, "Failed to send email for email data with id {EmailDataId}", emailData.EmailDataId);
                    throw;
                }
            }
            await client.DisconnectAsync(true);
            await _context.SaveChangesAsync();
            return result;
        }
    }
}
