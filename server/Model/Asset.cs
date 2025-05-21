using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    public class Asset
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset ID number")]
        public int Asset_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Unique identifier assigned by the manufacturer")]
        public string? Serial_Number { get; set; }

        [MaxLength(25)]
        [Display(Name = "Identifier for the model type of the asset")]
        public string? Model_Number { get; set; }

        [Display(Name = "Description of Asset")]
        public required string Asset_Desc { get; set; }

        [MaxLength(25)]
        [Display(Name = "GIS ID for asset identification")]
        public string? Asset_GIS_ID { get; set; }

        // [ForeignKey("Asset_Status_ID")]
        // [Display(Name = "Sequentially Server Generated Asset Status ID number")]
        // public int Asset_Status_ID { get; set; }

        // [ForeignKey("Manufacturer_ID")]
        // [Display(Name = "Sequentially Server Generated Asset Manufacturer ID number")]
        // public int Manufacturer_ID { get; set; }

        // [ForeignKey("Dept_ID")]
        // [Display(Name = "Sequentially Server Generated Owning Dept ID number")]
        // public int Dept_ID { get; set; }

        // [ForeignKey("Substation_ID")]
        // [Display(Name = "Sequentially Server Generated Substation ID number")]
        // public int Substation_ID { get; set; }

        // [ForeignKey("Feeder_ID")]
        // [Display(Name = "Sequentially Server Generated Feeder ID number")]
        // public int Feeder_ID { get; set; }

        // [ForeignKey("Ring_ID")]
        // [Display(Name = "Sequentially Server Generated Ring ID number")]
        // public int Ring_ID { get; set; }

        // [ForeignKey("Zone_ID")]
        // [Display(Name = "Sequentially Server Generated Zone ID number")]
        // public int Zone_ID { get; set; }

        // [ForeignKey("Segment_Type_ID")]
        // [Display(Name = "Sequentially Server Generated Zone ID number")]
        // public int Segment_Type_ID { get; set; }

        // [ForeignKey("Segment_Point_ID")]
        // [Display(Name = "Sequentially Server Generated Segment Point ID number")]
        // public int Segment_Point_ID { get; set; }

        // [ForeignKey("Tx_Circuit_ID")]
        // [Display(Name = "Sequentially Server Generated Tx circuit ID number")]
        // public int Tx_Circuit_ID { get; set; }

        // [ForeignKey("Asset_Class_ID")]
        // [Display(Name = "Sequentially Server Generated Classification ID number")]
        // public int Asset_Class_ID { get; set; }

        [Display(Name = "Time interval for asset maintenance")]
        public int? Maintenance_Interval { get; set; }

        [MaxLength(25)]
        [Display(Name = "Demarcation starting point of for asset (AutoRecloser / ABS / Section Fuse / T/F / Pole No. / GIS No.) e.g., pole A/12")]
        public string? From_Point { get; set; }

        [MaxLength(25)]
        [Display(Name = "Demarcation ending point for connections, (AutoRecloser / ABS / Section Fuse / T/F / Pole No. / GIS No.) e.g., pole C9050027")]
        public string? To_Point { get; set; }

        [MaxLength(50)]
        [Display(Name = "Street Name, e.g., Ojoe Road")]
        public string? Street_Name { get; set; }

        [Display(Name = "Frequency of Level 1 Ground Patrols")]
        public int? Lvl1_Patrol_Freq { get; set; }

        [Display(Name = "Frequency of Level 2 Ground Patrols")]
        public int? Lvl2_Patrol_Freq { get; set; }

        [Display(Name = "Frequency of Level 3 Ground Patrols")]
        public int? Lvl3_Patrol_Freq { get; set; }

        [Display(Name = "Frequency of aerial inspections")]
        public int? Aerial_Inspection_Freq { get; set; }

        [Display(Name = "Frequency of Thermographic Inspection")]
        public int? Thermographic_Freq { get; set; }

        [Display(Name = "Frequency of Ultrasonic Inspection")]
        public int? Ultrasonic_Freq { get; set; }

        [Display(Name = "Frequency of Vegetation Patrol")]
        public int? Vegetation_Patrol_Freq { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
