using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblJob_Plans_Headers")]
    public class JobPlansHeader
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Job Plan Id number")]
        public int Job_PlanId { get; set; }

        // [Display(Name = "Sequentially Server Generated Classification Id number")]
        // public int Asset_Class_Id { get; set; }

        [Display(Name = "Description of job plan")]
        public required string JobPlanDesc { get; set; }

        // [Display(Name = "Sequentially Server Generated Job Plan Status Id number")]
        // public int Job_Plan_Status_Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}