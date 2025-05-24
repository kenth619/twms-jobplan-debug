using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblSegment_Points")]
    public class SegmentPoint
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Segment Point Id number")]
        public int SegmentPointId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Start point name for the segment")]
        public required string SegmentPointName { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;
    }
}