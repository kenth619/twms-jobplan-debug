using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Job_Plans_Statuses")]
    public class JobPlanStatus
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Job Plan Status ID number")]
        public int Job_Plan_Status_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Job Plan Status")]
        public required string Job_Plan_Status_Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}