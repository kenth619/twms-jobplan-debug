using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Asset_Class_Attr_Map")]
    public class AssetClassAttrMap
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Class Attribute ID number")]
        public int Class_Attr_ID { get; set; }

        // [Display(Name = "Sequentially Server Generated Asset Class ID number")]
        // public int Asset_Class_ID { get; set; }

        // [Display(Name = "Sequentially Server Generated Attribute ID number")]
        // public int Attr_ID { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}