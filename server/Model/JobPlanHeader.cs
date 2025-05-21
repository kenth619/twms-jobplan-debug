using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Job_Plans_Headers")]
    public class JobPlansHeader
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Job Plan ID number")]
        public int Job_Plan_ID { get; set; }

        // [Display(Name = "Sequentially Server Generated Classification ID number")]
        // public int Asset_Class_ID { get; set; }

        [Display(Name = "Description of job plan")]
        public required string Job_Plan_Desc { get; set; }

        // [Display(Name = "Sequentially Server Generated Job Plan Status ID number")]
        // public int Job_Plan_Status_ID { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}