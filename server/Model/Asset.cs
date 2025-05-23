using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tblAssets")]
    public class Asset
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Id number")]
        public int AssetId { get; set; }

        [MaxLength(25)]
        [Display(Name = "Unique identifier assigned by the manufacturer")]
        public string? SerialNumber { get; set; }

        [MaxLength(25)]
        [Display(Name = "Identifier for the model type of the asset")]
        public string? ModelNumber { get; set; }

        [Display(Name = "Description of Asset")]
        public required string AssetDescription { get; set; }

        [MaxLength(25)]
        [Display(Name = "GIS Id for asset identification")]
        public string? AssetGISId { get; set; }

        // [ForeignKey("Asset_Status_Id")]
        // [Display(Name = "Sequentially Server Generated Asset Status Id number")]
        // public int Asset_Status_Id { get; set; }

        // [ForeignKey("Manufacturer_Id")]
        // [Display(Name = "Sequentially Server Generated Asset Manufacturer Id number")]
        // public int Manufacturer_Id { get; set; }

        // [ForeignKey("Dept_Id")]
        // [Display(Name = "Sequentially Server Generated Owning Dept Id number")]
        // public int Dept_Id { get; set; }

        // [ForeignKey("Substation_Id")]
        // [Display(Name = "Sequentially Server Generated Substation Id number")]
        // public int Substation_Id { get; set; }

        // [ForeignKey("Feeder_Id")]
        // [Display(Name = "Sequentially Server Generated Feeder Id number")]
        // public int Feeder_Id { get; set; }

        // [ForeignKey("Ring_Id")]
        // [Display(Name = "Sequentially Server Generated Ring Id number")]
        // public int Ring_Id { get; set; }

        // [ForeignKey("Zone_Id")]
        // [Display(Name = "Sequentially Server Generated Zone Id number")]
        // public int Zone_Id { get; set; }

        // [ForeignKey("Segment_Type_Id")]
        // [Display(Name = "Sequentially Server Generated Zone Id number")]
        // public int Segment_Type_Id { get; set; }

        // [ForeignKey("Segment_Point_Id")]
        // [Display(Name = "Sequentially Server Generated Segment Point Id number")]
        // public int Segment_Point_Id { get; set; }

        // [ForeignKey("Tx_Circuit_Id")]
        // [Display(Name = "Sequentially Server Generated Tx circuit Id number")]
        // public int Tx_Circuit_Id { get; set; }

        // [ForeignKey("Asset_Class_Id")]
        // [Display(Name = "Sequentially Server Generated Classification Id number")]
        // public int Asset_Class_Id { get; set; }

        [Display(Name = "Time interval for asset maintenance")]
        public int? MaintenanceInterval { get; set; }

        [MaxLength(25)]
        [Display(Name = "Demarcation starting point of for asset (AutoRecloser / ABS / Section Fuse / T/F / Pole No. / GIS No.) e.g., pole A/12")]
        public string? FromPoint { get; set; }

        [MaxLength(25)]
        [Display(Name = "Demarcation ending point for connections, (AutoRecloser / ABS / Section Fuse / T/F / Pole No. / GIS No.) e.g., pole C9050027")]
        public string? ToPoint { get; set; }

        [MaxLength(50)]
        [Display(Name = "Street Name, e.g., Ojoe Road")]
        public string? StreetName { get; set; }

        [Display(Name = "Frequency of Level 1 Ground Patrols")]
        public int? Lvl1PatrolFreq { get; set; }

        [Display(Name = "Frequency of Level 2 Ground Patrols")]
        public int? Lvl2PatrolFreq { get; set; }

        [Display(Name = "Frequency of Level 3 Ground Patrols")]
        public int? Lvl3PatrolFreq { get; set; }

        [Display(Name = "Frequency of aerial inspections")]
        public int? AerialInspectionFreq { get; set; }

        [Display(Name = "Frequency of Thermographic Inspection")]
        public int? ThermographicFreq { get; set; }

        [Display(Name = "Frequency of Ultrasonic Inspection")]
        public int? UltrasonicFreq { get; set; }

        [Display(Name = "Frequency of Vegetation Patrol")]
        public int? VegetationPatrolFreq { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
