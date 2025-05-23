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
        public required string AssetStatusDescription { get; set; }
    }
}