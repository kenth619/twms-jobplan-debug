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

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}