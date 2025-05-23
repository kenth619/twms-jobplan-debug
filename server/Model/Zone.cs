using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblZones")]
    public class Zone
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Zone Id number")]
        public int ZoneId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Zone")]
        public required string ZoneName { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}