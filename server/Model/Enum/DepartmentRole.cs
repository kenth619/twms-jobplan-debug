using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TemplateProject.Model.Enum
{
    public class DepartmentRole(string key, string name)
    {
        public static HashSet<DepartmentRole> All() 
        {
            return [Manager, Supervisor, Approver, Reviewer];
        } 

        public static readonly DepartmentRole None = new("none", "None");
        public static readonly DepartmentRole Manager = new("manager", "Manager");
        public static readonly DepartmentRole Supervisor = new("supervisor", "Supervisor");
        public static readonly DepartmentRole Approver = new("approver", "Approver");
        public static readonly DepartmentRole Reviewer = new("reviewer", "Reviewer");

        public static DepartmentRole FromKey(string _key)
        {
            return _key switch
            {
                "none" => None,
                "manager" => Manager,
                "supervisor" => Supervisor,
                "approver" => Approver,
                "reviewer" => Reviewer,
                _ => throw new Exception($"No DepartmentRole with key {_key}"),
            };
        }

        public string Key { get; set; } = key;
        public string Name { get; set; } = name;

        public override bool Equals(object? obj)
        {
            return obj is DepartmentRole role &&
                   Key == role.Key;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key);
        }
    }
    
    public class DepartmentRoleConverter : ValueConverter<DepartmentRole, string>
    {
        public DepartmentRoleConverter() : base(
            v => v.Key,
            v => DepartmentRole.FromKey(v))
        { }
    }
}
