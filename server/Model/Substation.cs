using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Substations")]
    public class Substation
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Substation ID number")]
        public int Substation_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Substation")]
        public required string Substation_Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}