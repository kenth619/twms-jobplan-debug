namespace TWMSServer.Model
{
    public class Department : IComparable
    {
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string? BusinessUnit { get; set; } = String.Empty;

        public List<Section> Sections { get; set; } = new();

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is Department otherDepartment)
            {
                return this.Name.CompareTo(otherDepartment.Name);
            }
            else
            {
                throw new ArgumentException("Object is not a Department");
            }
        }
    }
}
