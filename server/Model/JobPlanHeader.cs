using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TWMSServer.Model
{
    [Table("tblJob_Plans_Headers")]
    public class JobPlansHeader
    {
        [Key]
        [Column("JobPlanId")]
        [Display(Name = "Sequentially Server Generated Job Plan Id number")]
        public int JobPlanId { get; set; }

        [Column("JobPlanStatus")]
        [Display(Name = "Status of Job Plan")]
        [MaxLength(25)]
        [Required]
        public bool JobPlanStatus { get; set; } = true; // true by default

        [Column("JobPlanShortDesc")]
        [Display(Name = "Short Description of Job Plan")]
        [MaxLength(25)]
        [Required]
        public required string JobPlanShortDesc { get; set; } = string.Empty;

        [Column("JobPlanLongDesc")]
        [Display(Name = "Long Description of Job Plan")]
        [Required]
        public required string JobPlanLongDesc { get; set; } = string.Empty;

        [Column("AssetClassId")]
        [Display(Name = "Asset Class Id")]
        [Required]
        public int AssetClassId { get; set; }

        [Column("AssetClassShortDesc")]
        [Display(Name = "Short Description of Asset Class")]
        [MaxLength(25)]
        [Required]
        public required string AssetClassShortDesc { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<JobPlansLine> JobPlansLines { get; set; } = [];
    }
}