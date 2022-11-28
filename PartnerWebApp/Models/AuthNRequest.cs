namespace PartnerWebApp.Models
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:SAML:2.0:protocol")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:tc:SAML:2.0:protocol", IsNullable = false)]
    public partial class AuthnRequest
    {

        private string issuerField;

        private string destinationField;

        private string idField;

        private System.DateTime issueInstantField;

        private string protocolBindingField;

        private decimal versionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:tc:SAML:2.0:assertion")]
        public string Issuer
        {
            get
            {
                return this.issuerField;
            }
            set
            {
                this.issuerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Destination
        {
            get
            {
                return this.destinationField;
            }
            set
            {
                this.destinationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime IssueInstant
        {
            get
            {
                return this.issueInstantField;
            }
            set
            {
                this.issueInstantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ProtocolBinding
        {
            get
            {
                return this.protocolBindingField;
            }
            set
            {
                this.protocolBindingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }


}
