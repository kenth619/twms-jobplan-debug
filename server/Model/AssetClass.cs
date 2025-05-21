using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Assets_Classes")]
    public class AssetClass
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Classification ID number")]
        public int Asset_Class_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Description of asset class E.g. VCB, OCB, Power Transformer")]
        public required string Asset_Class_Desc { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}