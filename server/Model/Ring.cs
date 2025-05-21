using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Rings")]
    public class Ring
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Ring ID number")]
        public int Ring_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Ring")]
        public required string Ring_Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}