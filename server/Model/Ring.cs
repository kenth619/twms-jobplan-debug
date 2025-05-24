using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblRings")]
    public class Ring
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Ring Id number")]
        public int RingId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Ring")]
        public required string RingName { get; set; }

        [Display(Name = "Whether or not the user can change the record value")]
        public required bool Status { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;
    }
}