namespace PartnerWebApp.Models
{
    [Serializable]
    public class SAMLInput
    {
        /// <summary>
        /// PrivateKey. This will be the unique key and will be used to Sign the SAML
        /// </summary>
        public string PrivateKey
        {
            get;
            set;
        }

        /// <summary>
        /// Recipient. This will be the URL or Name of the Recipient party. 
        /// Example: https://isb.wavesecure.com/MFE
        /// </summary>
        public string Recipient
        {
            get;
            set;
        }

        /// <summary>
        /// Issuer. This will be the URL of the SAML Generation party
        /// Example:https://home.mcafee.com/
        /// </summary>
        public string Issuer
        {
            get;
            set;
        }

        /// <summary>
        /// Domain. This will be the SAML Issuer's Domain Name 
        /// Example: mcafee.com
        /// </summary>
        public string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Subject. This will be used to define the purpose of SAML
        /// </summary>
        public string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// ValidPeriod. This will be the duration for which
        /// this SAML should be considered as valid in Minutes.
        /// </summary>
        public int ValidPeriod
        {
            get;
            set;
        }

        /// <summary>
        /// Attributes. A list of attributes to pass in SAML.
        /// SAML Attributes are the Key Value Pair values.
        /// Example: Key as AffId and value as 0
        /// </summary>
        public Dictionary<string, string> Attributes
        {
            get;
            set;
        }
    }
}
