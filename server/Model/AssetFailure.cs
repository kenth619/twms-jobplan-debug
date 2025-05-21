using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Assets_Failures")]
    public class AssetsFailure
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Failure ID number")]
        public int Asset_Failure_ID { get; set; }

        // [Display(Name = "Sequentially Server Generated Asset ID number")]
        // public int Asset_ID { get; set; }

        [Display(Name = "Cumulative MTBF value")]
        public long Cumulative_MTBF { get; set; }

        [Display(Name = "Failure Date of Asset")]
        public DateTime Failure_Date { get; set; }

        [MaxLength(50)]
        [Display(Name = "Work Order / Trouble Report associated with failure")]
        public required string Source_Doc { get; set; }

        [Display(Name = "Failure Mode when failure occurred, e.g., Shattered insulator")]
        public required string Failure_Mode { get; set; }

        [Display(Name = "Failure Cause 1 for Asset, e.g., Failed insulator")]
        public required string Failure_Cause1 { get; set; }

        [Display(Name = "Failure Cause 2 for Asset, e.g., Animal contact")]
        public string? Failure_Cause2 { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}