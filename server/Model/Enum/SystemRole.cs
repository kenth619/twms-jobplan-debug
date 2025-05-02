using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TWMSServer.Model.Enum
{
    public class SystemRole(string key, string name)
    {
        public static HashSet<SystemRole> All() {
            return [Administrator, Superuser];
        }

        public static readonly SystemRole None = new("none", "None");
        public static readonly SystemRole Administrator = new("system-administrator", "System Administrator");
        public static readonly SystemRole Superuser = new("superuser", "Superuser");

        public static SystemRole FromKey(string _key)
        {
            return _key switch
            {
                "none" => None,
                "system-administrator" => Administrator,
                "superuser" => Superuser,
                _ => throw new Exception($"No GlobalRole with key {_key}"),
            };
        }

        public string Key { get; set; } = key;
        public string Name { get; set; } = name;

        public override bool Equals(object? obj)
        {
            return obj is SystemRole role &&
                   Key == role.Key;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key);
        }
    }
    
    public class SystemRoleConverter : ValueConverter<SystemRole, string>
    {
        public SystemRoleConverter() : base(
            v => v.Key,
            v => SystemRole.FromKey(v))
        { }
    }
}
