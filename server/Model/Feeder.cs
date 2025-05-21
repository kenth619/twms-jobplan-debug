using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Feeders")]
    public class Feeder
    {
        [Display(Name = "Sequentially Server Generated Feeder ID number")]
        public int Feeder_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Feeder")]
        public required string Feeder_Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}