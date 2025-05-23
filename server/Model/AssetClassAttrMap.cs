using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblAssets_Classes_Attribute_Maps")]
    public class AssetClassAttributeMap
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Class Attribute Id number")]
        public int ClassAttributeId { get; set; }

        // [Display(Name = "Sequentially Server Generated Asset Class Id number")]
        // public int AssetClassId { get; set; }

        // [Display(Name = "Sequentially Server Generated Attribute Id number")]
        // public int AttributeId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}