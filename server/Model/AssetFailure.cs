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
        public int AssetFailId { get; set; }

        // [Display(Name = "Sequentially Server Generated Asset Id number")]
        // public int AssetId { get; set; }

        [Display(Name = "Cumulative MTBF value")]
        public long MTBF { get; set; }

        [Display(Name = "Failure Date of Asset")]
        public DateTime FailDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Work Order / Trouble Report associated with failure")]
        public required string SourceDoc { get; set; }

        [Display(Name = "Failure Mode when failure occurred, e.g., Shattered insulator")]
        public required string FailMode { get; set; }

        [Display(Name = "Failure Cause 1 for Asset, e.g., Failed insulator")]
        public required string FailCause1 { get; set; }

        [Display(Name = "Failure Cause 2 for Asset, e.g., Animal contact")]
        public string? FailCause2 { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;
    }
}