using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblAssets_Failures")]
    public class AssetFailure
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Failure Id number")]
        public int AssetFailureId { get; set; }

        // [Display(Name = "Sequentially Server Generated Asset Id number")]
        // public int AssetId { get; set; }

        [Display(Name = "Cumulative MTBF value")]
        public long CumulativeMTBF { get; set; }

        [Display(Name = "Failure Date of Asset")]
        public DateTime FailureDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Work Order / Trouble Report associated with failure")]
        public required string SourceDocument { get; set; }

        [Display(Name = "Failure Mode when failure occurred, e.g., Shattered insulator")]
        public required string FailureMode { get; set; }

        [Display(Name = "Failure Cause 1 for Asset, e.g., Failed insulator")]
        public required string FailureCause1 { get; set; }

        [Display(Name = "Failure Cause 2 for Asset, e.g., Animal contact")]
        public string? FailureCause2 { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}