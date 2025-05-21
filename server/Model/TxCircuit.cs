using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWMSServer.Model
{
    [Table("tbl_Tx_Circuits")]
    public class TxCircuit
    {
        [Key]
        [Display(Name = "Sequentially Server Generated Tx circuit ID number")]
        public int Tx_Circuit_ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Name of the Tx Circuit")]
        public required string Tx_Circuit_Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
    }
}