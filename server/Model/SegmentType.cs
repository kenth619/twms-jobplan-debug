using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Segment_Types")]
    public class SegmentType
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Segment Type ID number")]
        public int Segment_Type_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Description of the Segment Type")]
        public required string Segment_Type_Desc { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}