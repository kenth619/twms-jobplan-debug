using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Segment_Points")]
    public class SegmentPoint
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Segment Point ID number")]
        public int Segment_Point_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Start point name for the segment")]
        public required string Segment_Point_Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}