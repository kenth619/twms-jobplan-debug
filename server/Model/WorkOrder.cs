namespace TWMSServer.Model
{
    public class WorkOrder
    {
        public int WorkOrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public List<Asset> Assets { get; set; }
    }
}
