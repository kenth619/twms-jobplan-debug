using System.DirectoryServices;

// Disable platform compatibility validation.
// DirectoryServices is a Windows only feature provided by Microsoft.Windows.Compatibility
namespace TWMSServer.Providers.AuthProvider
{
    // Created by Arshad Hosein, 2021-06-30
    // This file implements IAuthProvider using LDAP Authentication.
    // It relies on System.DirectoryServices, which is specific to Windows.
    public class LDAPAuthProvider : IAuthProvider
    {
        private IConfiguration _configuration { get; set; }

        public LDAPAuthProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private const string DisplayNameAttribute = "DisplayName";
        private const string SAMAccountNameAttribute = "SAMAccountName";

        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                string ldap = _configuration["LDAP"] ?? throw new Exception("Not configured for LDAP Auth!");
                string domain = _configuration["DomainName"] ?? throw new Exception("Not configured for LDAP Auth!");;
#pragma warning disable CA1416 // Validate platform compatibility
                using DirectoryEntry entry = new(ldap, $"{domain}\\{username}", password);
                using DirectorySearcher searcher = new(entry);
                searcher.Filter = String.Format("({0}={1})", SAMAccountNameAttribute, username);
                searcher.PropertiesToLoad.Add(DisplayNameAttribute);
                searcher.PropertiesToLoad.Add(SAMAccountNameAttribute);
                var result = searcher.FindOne();
                if (result != null)
                {
                    return true;
                }
#pragma warning restore CA1416 // Validate platform compatibility
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}