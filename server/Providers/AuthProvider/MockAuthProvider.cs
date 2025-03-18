namespace TemplateProject.Providers.AuthProvider
{
    public class MockAuthProvider : IAuthProvider
    {        
        // Created by Arshad Hosein, 2021-06-30
        // This file implements IAuthProvider but does not validate credentials.
        // This behavior allows for easy testing in  development environments.
        public bool AuthenticateUser(string username, string password)
        {
            return true;
        }
    }
}
