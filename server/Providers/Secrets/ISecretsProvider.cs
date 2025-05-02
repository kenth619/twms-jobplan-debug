
namespace TWMSServer.Providers.Secrets
{
    public interface ISecretsProvider
    {
        public string GetSMTPPassword();
        public string GetTemplateProjectConnectionString();
        public string GetSysIntegrationConnectionString();
    }
}
