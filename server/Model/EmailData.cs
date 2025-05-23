using System.ComponentModel.DataAnnotations.Schema;
using MimeKit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWMSServer.Model
{
    public class EmailData
    {
        [Key]
        public int EmailDataId { get; set; }
        public DateTime? SentSuccessfullyAt { get; set; } 
        public string TemplateName { get; set; } = string.Empty;
        public string TemplateDataRaw { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public HashSet<string> ToRecipients { get; set;} = [];
        public HashSet<string> CCRecipients { get; set;} = [];
        public HashSet<string> BCCRecipients { get; set;} = [];

        public void AddTo(string? recipient)
        {
            if (recipient == null)
                return;
            string normalized = recipient.Normalize();
            bool valid = MailboxAddress.TryParse(normalized, out MailboxAddress _add);
            if (valid && !ToRecipients.Contains(normalized))
                ToRecipients.Add(normalized);        
        }        
        
        public void AddCC(string? recipient)
        {
            if (recipient == null)
                return;
            string normalized = recipient.Normalize();
            bool valid = MailboxAddress.TryParse(normalized, out MailboxAddress _add);
            if (valid && !CCRecipients.Contains(normalized))
                CCRecipients.Add(normalized);
        }        
        
        public void AddBCC(string? recipient)
        {
            if (recipient == null)
                return;
            string normalized = recipient.Normalize();
            bool valid = MailboxAddress.TryParse(normalized, out MailboxAddress _add);
            if (valid && !BCCRecipients.Contains(normalized))
                BCCRecipients.Add(normalized);        
        }

        [NotMapped] public IEnumerable<InternetAddress> ParsedToRecipients => ToRecipients.Select(e => MailboxAddress.Parse(e));
        [NotMapped] public IEnumerable<InternetAddress> ParsedCCRecipients => CCRecipients.Select(e => MailboxAddress.Parse(e));
        [NotMapped] public IEnumerable<InternetAddress> ParsedBCCRecipients => BCCRecipients.Select(e => MailboxAddress.Parse(e));

        public ICollection<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
    }
}
