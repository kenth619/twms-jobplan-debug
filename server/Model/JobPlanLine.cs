using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblJob_Plans_Lines")]
    public class JobPlansLine
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Job Plan Line (Task) Id number")]
        public int Job_PlanLineId { get; set; }

        [Display(Name = "Sequentially Server Generated Job Plan Id number")]
        public int JobPlanId { get; set; }

        [Display(Name = "Sequentially Generated Task No")]
        public int JobPlanLineNo { get; set; }

        [Display(Name = "Description of Line (Task)")]
        public required string JobPlanLineDesc { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}