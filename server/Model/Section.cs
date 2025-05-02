namespace TWMSServer.Model
{
    public class Section : IComparable
    {
        public string Name { get; set; } = String.Empty;

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is Section otherSection)
            {
                return this.Name.CompareTo(otherSection.Name);
            }
            else
            {
                throw new ArgumentException("Object is not a Section");
            }
        }
    }
}
