using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblAssets_Classes_Attribute_Maps")]
    public class AssetClassAttrMap
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Class Attribute Id number")]
        public int ClassAttrId { get; set; }

        // [Display(Name = "Sequentially Server Generated Asset Class Id number")]
        // public int AssetClassId { get; set; }

        // [Display(Name = "Sequentially Server Generated Attribute Id number")]
        // public int AttributeId { get; set; }

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