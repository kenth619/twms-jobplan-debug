using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblAssets_Manufacturers")]
    public class AssetManufacturer
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Status Id number")]
        public int AssetStatusId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Description of the status of the asset")]
        public required string AssetStatusDesc { get; set; }

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