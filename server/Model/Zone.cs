using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Zones")]
    public class Zone
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Zone ID number")]
        public int Zone_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Zone")]
        public required string Zone_Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}