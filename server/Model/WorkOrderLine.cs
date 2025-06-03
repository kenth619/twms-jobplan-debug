 namespace TWMSServer.Model
{
    public class WorkOrderLine
    {
        public int WorkOrderLineId { get; set; }
        public int WorkOrderId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        // Navigation property
        public WorkOrder WorkOrder { get; set; }
    }
}
