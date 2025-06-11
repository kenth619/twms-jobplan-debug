using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace TWMSServer.Model
{
    [Table("tblJob_Plans_Lines")]
    public class JobPlansLine
    {
        [Key]
        [Column("Job_Plan_Line_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobPlanLineId { get; set; }

        [Column("Job_Plan_ID")]
        [Required]
        [ForeignKey("JobPlansHeader")]
        public int JobPlanId { get; set; }

        [Column("Job_Plan_Line_No")]
        [Required]
        public long JobPlanLineNo { get; set; }

        [Column("Job_Plan_Line_Desc")]
        [Required]
        [MaxLength(500)]
        public string JobPlanLineDesc { get; set; } = string.Empty;

        // Navigation property
        [JsonIgnore]
        public virtual JobPlansHeader JobPlansHeader { get; set; } = null!;
    }
}
