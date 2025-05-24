using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblSegment_Types")]
    public class SegmentType
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Segment Type Id number")]
        public int SegmentTypeId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Description of the Segment Type")]
        public required string SegmentTypeDesc { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;
    }
}