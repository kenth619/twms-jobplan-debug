using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblAssets_Classes")]
    public class AssetClass
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Classification Id number")]
        public int AssetClassId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Short Description of Asset Class E.g. VCB, OCB, Power Transformer")]
        public required string AssetClassShortDesc { get; set; } = string.Empty;

        // Maximum length field
        [Display(Name = "Long Description of Asset Class E.g. Vaccum Circuit Breaker")]
        public required string AssetClassLongDesc { get; set; } = string.Empty;

        [MaxLength(50)]
        [Display(Name = "Whether or not the user can change the attribute value")]
        public required bool Status { get; set; } = true; // true by default

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;
    }
}