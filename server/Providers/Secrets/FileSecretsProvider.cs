namespace TWMSServer.Providers.Secrets
{
    public class FileSecretsProvider : ISecretsProvider
    {
        private readonly string smtpPassword;

        private readonly string twmsConnectionString;
        private readonly string sysIntegrationConnectionString;
        
        public FileSecretsProvider(IConfiguration configuration)
        {
            string secretsFile = configuration.GetValue<string>("Secrets:FilePath") ?? "./secrets.json";

            if (!File.Exists(secretsFile))
            {
                throw new FileNotFoundException("Secrets file not found.", secretsFile);
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(secretsFile, optional: false, reloadOnChange: true);

            var secrets = builder.Build();

            smtpPassword = secrets["Passwords:SMTP"]!;

            twmsConnectionString = secrets["ConnectionStrings:connMSSQLNoCred"]!;
            sysIntegrationConnectionString = secrets["ConnectionStrings:SysIntegration"]!;
        }

        public string GetSMTPPassword() { return smtpPassword; }
        public string GetTemplateProjectConnectionString() { return twmsConnectionString; }
        public string GetSysIntegrationConnectionString() { return sysIntegrationConnectionString; }
       
    }
}
