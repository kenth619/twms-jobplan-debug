using Quartz;
using TWMSServer.Providers;

namespace TWMSServer.Jobs
{
    public class SendEmailJob(ILogger<SendEmailJob> logger, EmailProvider emailProvider) : IJob
    {
        private readonly ILogger<SendEmailJob> _logger = logger;
        private readonly EmailProvider _emailProvider = emailProvider;

        public async Task Execute(IJobExecutionContext context)
        {
            var jobId = context.JobDetail.Key.ToString();
            var dataMap = context.MergedJobDataMap;
            var emailDataId = dataMap.GetInt("EmailDataId");
            bool result = await _emailProvider.SendEmail(emailDataId);
            _logger.LogInformation("Result of job with id {JobId} to send email with id of {EmailDataId} is: {JobResult}", jobId, emailDataId, result);
        }
    }
}
