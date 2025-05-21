using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Assets_Statuses")]
    public class AssetStatus
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Status ID number")]
        public int Asset_Status_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Current operational status of the asset")]
        public required string Asset_Status_Desc { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}