using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblJob_Plans_Statuses")]
    public class JobPlanStatus
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Job Plan Status Id number")]
        public int JobPlanStatusId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Job Plan Status")]
        public required string JobPlanStatusName { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}