using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblSubstations")]
    public class Substation
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Substation Id number")]
        public int SubstationId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Substation")]
        public required string SubstationName { get; set; }

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