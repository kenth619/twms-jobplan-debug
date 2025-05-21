using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Asset_Attributes")]
    public class AssetAttribute
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Attribute ID number")]
        public int Asset_Attribute_ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "Technical Attribute of the Asset E.g. BILRATNG. This is linked to an asset category")]
        public required string Asset_Attribute_Name { get; set; }

        [MaxLength(25)]
        [Display(Name = "Unit of measurement of attribute. E.g. kV for BIL rating")]
        public required string Unit_Measure { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}