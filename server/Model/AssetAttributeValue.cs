// using System;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace TWMSServer.Model
// {
//     [Table("tbl_Asset_Attribute_Values")]
//     public class AssetAttributeValue
//     {
//         [Key]
//         [MaxLength(50)]
//         [Display(Name = "Composite Primary Key")]
//         public string AssetId, AttributeId { get; set; }

        // [Display(Name = "Sequentially Server Generated Asset Id number")]
        // public int AssetId { get; set; }

        // [Display(Name = "Sequentially Server Generated Attribute Id number")]
        // public long AttributeId { get; set; }

//         [MaxLength(50)]
//         [Display(Name = "Value of Attribute")]
//         public required string Value { get; set; }

//         [MaxLength(25)]
//         [Display(Name = "Unit of measurement of attribute. E.g. kV for BIL rating")]
//         public string UnitMeasure { get; set; }

//         public DateTime DateCreated { get; set; }
//         public DateTime DateModified { get; set; }
//         public string CreatedBy { get; set; } = string.Empty;
//         public string ModifiedBy { get; set; } = string.Empty;
//     }
// }