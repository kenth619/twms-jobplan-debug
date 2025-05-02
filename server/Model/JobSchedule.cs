namespace TWMSServer.Model
{
    public class JobSchedule
    {
        public int JobScheduleId { get; set; }
        
        public string JobName { get; set; } = string.Empty;
        
        public string CronExpression { get; set; } = string.Empty;
        
        public bool IsEnabled { get; set; } = true;
        
        public DateTime? LastRun { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
}