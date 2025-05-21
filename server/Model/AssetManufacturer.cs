using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    public class AssetManufacturer
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Asset Status ID number")]
        public int Asset_Status_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Description of the status of the asset")]
        public required string Asset_Status_Desc { get; set; }
    }
}