using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblAssets_Statuses")]
    public class AssetStatus
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Status Id number")]
        public int AssetStatusId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Current operational status of the asset")]
        public required string AssetStatusDescription { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}