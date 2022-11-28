using PartnerWebApp.Models;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace PartnerWebApp.Utilities
{

    public static class SigningHelper
    {
        public static string SignSAMLUsingPrivateKey(XmlDocument doc, string privateKey, string referenceValue, AppSettings configuration)
        {
            string responseStr = string.Empty;

            try
            {
                SamlSignedXml sig = new SamlSignedXml(doc);

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(GetCspParameters());

                rsa.FromXmlString(privateKey);
                sig.SigningKey = rsa;

                // Create a reference to be signed. 
                Reference reference = new Reference();

                reference.Uri = String.Empty;
                reference.Uri = "#" + referenceValue;

                // Add an enveloped transformation to the reference. 
                XmlDsigEnvelopedSignatureTransform env = new
                    XmlDsigEnvelopedSignatureTransform();
                XmlDsigC14NTransform env2 = new XmlDsigC14NTransform();

                reference.AddTransform(env);
                reference.AddTransform(env2);

                // Add the reference to the SignedXml object. 
                sig.AddReference(reference);


                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new RSAKeyValue((RSA)rsa));

                sig.KeyInfo = keyInfo;

                string path = configuration.PrivateKeyCertificatePath; //@"C:\Projects\UnifiedAuth\SSLRSACertGen\cert.pfx";
                X509Certificate2 privateCert = new X509Certificate2(path, string.Empty, X509KeyStorageFlags.Exportable);
                RSA? privateKey1 = privateCert.GetRSAPrivateKey();
                RSACryptoServiceProvider privateKey2 = new RSACryptoServiceProvider();
                privateKey2.ImportParameters(privateKey1.ExportParameters(true));


                sig.SigningKey = privateKey2;

                
                sig.ComputeSignature();

                // Get the XML representation of the signature and save it to an XmlElement object. 
                XmlElement signature = sig.GetXml();


                doc.DocumentElement.ChildNodes[0].InsertBefore(signature,
                        doc.DocumentElement.ChildNodes[0].ChildNodes[1]);

                responseStr = doc.OuterXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return responseStr;
        }

        private static CspParameters GetCspParameters()
        {
            CspParameters cspParams = new CspParameters(1);
            cspParams.KeyContainerName = "SAMLTestContainer";
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";

            return cspParams;
        }
    }

    public class SamlSignedXml : SignedXml
    {
        public SamlSignedXml() : base() { }

        public SamlSignedXml(XmlDocument document)
            : base(document)
        {
        }

        public override XmlElement GetIdElement(XmlDocument document, string idValue)
        {
            XmlElement elem = document.SelectSingleNode("//*[@ID=\"" + idValue + "\"]") as XmlElement;
            if (elem != null)
            {
                return elem;
            }
            return document.GetElementById(idValue);
        }
    }
}
