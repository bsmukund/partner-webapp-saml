using PartnerWebApp.Models;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using Microsoft.Extensions.Options;

namespace PartnerWebApp.Utilities
{
    public class SamlGenerator
    {
        private AppSettings _configuration;
        public string SamlResponse { get;set; }
        public SamlGenerator(AppSettings configuration)
        {
            SamlResponse = "";
            _configuration = configuration;
        }
        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }
        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }

        public string GenerateSAML(string samlAuthRequest, AuthRequestFormat authRequestFormat)
        {
            var resp = new Response();
            string samlResponseString = "";

            SAMLInput samlInput = new SAMLInput()
            {
                PrivateKey = _configuration.PrivateKey,
                Domain = _configuration.SAMLDomain,
                Issuer = _configuration.SAMLIssuer,
                //Recipient = "https://idpartner.mcafee.com/login/callback?connection=saml-testsp",
                Recipient = _configuration.SAMLRecipient,
                //ValidPeriod = 20,
                ValidPeriod = _configuration.SAMLValidPeriod,
                Attributes = new Dictionary<string, string>()
                {
                    {"ccid", _configuration.UserCCID},
                    {"affid", _configuration.UserAffId},
                    {"culture", _configuration.UserCulture}

                }
            };


            if (!string.IsNullOrWhiteSpace(samlAuthRequest))
            {
                //Parse SAML and use the ID and Issuer
                //if deflate encoding is enabled, then we have to deflate and then decode
                {
                    //TODO: Check deflate encoding based on the setting and then deflate decode if necessary
                }
                var authnReq = Deserialize<AuthnRequest>(Encoding.UTF8.GetString(Convert.FromBase64String(samlAuthRequest)));
                var incomingReqId = authnReq.ID;
                var incomingIssuer = authnReq.Issuer;


                resp.InResponseTo = incomingReqId;
                resp.ID = "_" + Guid.NewGuid().ToString();
                resp.Version = authnReq.Version;
                resp.IssueInstant = DateTime.UtcNow;
                resp.Destination = samlInput.Recipient;

                resp.Assertion = new Assertion();

                resp.Assertion.ID = "_" + Guid.NewGuid().ToString();
                resp.Assertion.Version = authnReq.Version;


                resp.Assertion.Issuer = new AssertionIssuer()
                {
                    Value = samlInput.Issuer.Trim(),
                    Version = authnReq.Version
                };

                resp.Assertion.IssueInstant = DateTime.UtcNow;

                resp.Assertion.Conditions = new AssertionConditions()
                {
                    AudienceRestriction = new AssertionConditionsAudienceRestriction()
                    {
                        Audience = samlInput.Domain
                    }
                };

                resp.Assertion.Subject = new AssertionSubject();
                resp.Assertion.Subject.NameID = new AssertionSubjectNameID()
                {
                    Format = "urn:oasis:names:tc:SAML:2.0:nameid - format:unspecified",
                    Value = samlInput.Subject?.Trim()
                };

                resp.Assertion.Subject.SubjectConfirmation = new AssertionSubjectSubjectConfirmation
                {
                    Method = "urn:oasis:names:tc:SAML:2.0:cm:bearer",
                    SubjectConfirmationData = new AssertionSubjectSubjectConfirmationSubjectConfirmationData()
                    {
                        InResponseTo = incomingReqId,
                        NotOnOrAfter = DateTime.UtcNow.AddMinutes(_configuration.SAMLValidPeriod),
                        Recipient = samlInput.Recipient
                    }
                };
                resp.Assertion.AuthnStatement = new AssertionAuthnStatement
                {
                    AuthnContext = new AssertionAuthnStatementAuthnContext()
                    {
                        AuthnContextClassRef = "urn:oasis:names:tc:SAML:2.0:ac:classes:unspecified",
                    },
                    AuthnInstant = authnReq.IssueInstant,
                };

                resp.Assertion.AttributeStatement = new AssertionAttribute[]{
                    new AssertionAttribute(){Name = "ccid", AttributeValue = samlInput.Attributes["ccid"] },
                    new AssertionAttribute(){Name = "affid", AttributeValue = samlInput.Attributes["affid"]},
                    new AssertionAttribute(){Name = "culture", AttributeValue = samlInput.Attributes["culture"]}
                    };

                resp.Assertion.Conditions = new AssertionConditions()
                {
                    AudienceRestriction = new AssertionConditionsAudienceRestriction()
                    {
                        Audience = samlInput.Domain.Trim()
                    },
                    NotBefore = DateTime.UtcNow,
                    NotOnOrAfter = DateTime.UtcNow.AddMinutes(_configuration.SAMLValidPeriod)
                };
            
                var respString = Serialize<Response>(resp);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(respString);

                samlResponseString = SigningHelper.SignSAMLUsingPrivateKey(xmlDoc, samlInput.PrivateKey, resp.Assertion.ID, _configuration);

            }

            if (authRequestFormat == AuthRequestFormat.Base64)
            {
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(samlResponseString.ToString() ?? "");
                SamlResponse = System.Convert.ToBase64String(toEncodeAsBytes);
            }
            return SamlResponse;
        }
    }
}
