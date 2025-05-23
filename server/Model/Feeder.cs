using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblFeeders")]
    public class Feeder
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Feeder Id number")]
        public int FeederId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Feeder")]
        public required string FeederName { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}