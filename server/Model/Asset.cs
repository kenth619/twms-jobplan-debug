namespace TWMSServer.Model
{
    public class Asset
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public string AssetType { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public int WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; } = null!;
        public override string ToString()
        {
            return $"{AssetName} ({SerialNumber})";
        }
    }
}
