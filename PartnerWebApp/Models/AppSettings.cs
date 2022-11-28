namespace PartnerWebApp.Models
{
    public class AppSettings
    {
        public const string AppConfiguration = "AppConfiguration";
        public string PrivateKey { get; set; }
        public string PrivateKeyCertificatePath { get; set; }
        public string UserCCID { get; set; }
        public string UserAffId { get; set; }
        public string UserCulture { get; set; }
        public string SAMLRecipient { get; set; }
        public string SAMLDomain { get; set; }
        public string SAMLIssuer { get; set; }
        public string SAMLPostURL { get;set; }
        public string SAMLFormName { get; set; }
        public int SAMLValidPeriod { get; set; }
        public ICollection<UserDetail> AllowedUsers { get; set; }

    }
}
