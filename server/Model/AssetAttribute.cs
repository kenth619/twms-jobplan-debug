using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblAssets_Attributes")]
    public class AssetAttribute
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Attribute Id number")]
        public int AssetAttrId { get; set; }

        [MaxLength(50)]
        [Display(Name = "Technical Attribute of the Asset E.g. BILRATNG. This is linked to an asset category")]
        public required string AssetAttrName { get; set; }

        [MaxLength(25)]
        [Display(Name = "Unit of measurement of attribute. E.g. kV for BIL rating")]
        public required string Unit { get; set; }

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