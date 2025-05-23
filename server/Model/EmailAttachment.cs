using System.ComponentModel.DataAnnotations;

namespace TWMSServer.Model
{
    public class EmailAttachment
    {
        [Key]
        public int EmailAttachmentId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string? ContentType { get; set; }

        public int EmailDataId { get; set; }
    }
}