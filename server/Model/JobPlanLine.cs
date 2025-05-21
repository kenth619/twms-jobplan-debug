using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Job_Plans_Lines")]
    public class JobPlansLine
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Job Plan Line (Task) ID number")]
        public int Job_Plan_Line_ID { get; set; }

        [Display(Name = "Sequentially Server Generated Job Plan ID number")]
        public int Job_Plan_ID { get; set; }

        [Display(Name = "Sequentially Generated Task No")]
        public int Job_Plan_Line_No { get; set; }

        [Display(Name = "Description of Line (Task)")]
        public required string Job_Plan_Line_Desc { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}