namespace TemplateProject.Providers.AuthProvider
{
    // This interface describes a method for verifying user credentials.
    // It is used during login and when signing changes to a Form.
    public interface IAuthProvider
    {
        // Given a username and password, verifies those credentials and returns.
        public bool AuthenticateUser(string username, string password);
    }
}
