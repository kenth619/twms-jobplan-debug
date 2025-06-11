using System.ComponentModel.DataAnnotations;

namespace TWMSServer.Model
{
    public class WorkOrder
    {
        [Key]
        public int WorkOrderId { get; set; }
        
        public string RequestedBy { get; set; }
        public string Area { get; set; }
        public string Section { get; set; }
        public int AssociatedWONumber { get; set; }
        public string JobType { get; set; }
        public string JobTypeSubCategory { get; set; }
        public int Priority { get; set; }
        public string SourceDocumentType { get; set; }
        public string SourceDocumentNumber { get; set; }
        public string Account { get; set; }
        public string CostCentre { get; set; }
        public string JobNumber { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
