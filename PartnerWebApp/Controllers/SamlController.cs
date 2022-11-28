using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PartnerWebApp.Model;
using PartnerWebApp.Models;
using PartnerWebApp.Utilities;
using System.Collections.Specialized;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PartnerWebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SamlController : ControllerBase
    {
        private readonly AppSettings _configuration;
        public SamlController(IOptions<AppSettings> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        [Route("process")]
        public async Task<SAMLResponse> Get([FromQuery] string SAMLRequest, [FromQuery] string relayState)
        {
            bool samlValidation = false;
            {
                //TODO: If SAMLRequest is deflate encoded, then decode it accordingly
                //TODO: If SAMLRequest is signed, validate the signature with McAfee's cert
                samlValidation = true;
            }

            var samlResponse = (new SamlGenerator(_configuration)).GenerateSAML(SAMLRequest, AuthRequestFormat.Base64);

            var dict = new Dictionary<string, string>()
            {
                {"SAMLResponse", samlResponse },
                {"RelayState",  relayState}
            };            

            NameValueCollection bodyData = new NameValueCollection()
            {
                {"SAMLResponse", samlResponse },
                {"RelayState", relayState }
            };

            return new SAMLResponse()
            {
                samlresponse = samlResponse,
                validationstatus = samlValidation
            };            
        }

        private void FormPostSaml(NameValueCollection data)
        {
            string postUrl = "https://idpartner.mcafee.com/login/callback?connection=saml-testsp";
            string formName = "SAML-Form";

            string htmlContent = CreateHtmlForm(data);
            var responseContext = HttpContext.Response;
            responseContext.WriteAsync(htmlContent);

            //throw new NotImplementedException();
        }

        private string CreateHtmlForm(NameValueCollection data)
        {
            string postUrl = "https://idpartner.mcafee.com/login/callback?connection=saml-testsp";
            var schema = new html();
            schema.head = new htmlHead()
            {
                id = "SAML Form",
                runat = "server",
                title = "SAML-POST",
                script = new htmlHeadScript
                {
                    type = "text/javascript",
                    Value = @"function SubmitSAML(timeInterval)    
                        {
                            var frmObj = document.getElementById(""form1"");
                            if (!frmObj)
                                frmObj = GetSamlForm();
                            var txtSAMLObj = document.getElementById(""SAMLResponse"");
                            if (frmObj == null || txtSAMLObj == null)
                                return;
                            txtSAMLObj.value = document.getElementById(""SAMLResponse"").value;
                            frmObj.action = document.getElementById(""txtPostDestinationURL"").value;
                            frmObj.submit();
                        }
                        function GetSamlForm()
                        {
                            var frmObj = document.createElement(""form"");
                            frmObj.id = ""samlForm"";
                            frmObj.name = ""samlForm"";
                            frmObj.method = ""POST"";

                            var txtSAMLObj = document.createElement(""input"");
                            txtSAMLObj.type = ""text"";
                            txtSAMLObj.id = ""SAMLResponse"";
                            txtSAMLObj.name = ""SAMLResponse"";

                            frmObj.appendChild(txtSAMLObj);
                            document.appendChild(frmObj);

                            return frmObj;
                        }"
                }
            };
            schema.body = new htmlBody()
            {
                runat = "server",
                id = "form1",
                form = new htmlBodyForm()
                {
                    action = postUrl,
                    runat = "server",
                    method = "POST",
                    id = "form1",
                    div = new htmlBodyFormDiv()
                    {
                        input = new htmlBodyFormDivInput[]
                        {
                            new htmlBodyFormDivInput() {id = "SAMLResponse", type="hidden", name = "SAMLResponse", runat="server", value= data["SAMLResponse"]},
                            new htmlBodyFormDivInput() {id = "RelayState", type="hidden", name = "RelayState", runat = "server", value=data["RelayState"]}
                        }
                    }
                }
            };

            var html = SamlGenerator.Serialize<html>(schema);
            Console.WriteLine("The html rendered is : " + html);
            return html;
        }
    }
}
