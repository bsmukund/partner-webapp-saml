using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PartnerWebApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PartnerWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppSettings _configuration;
        public LoginController(IOptions<AppSettings> configuration)
        {
            _configuration = configuration.Value;
        }
        // GET: api/<LoginController>
        [HttpGet]
        [Route("user")]
        public string Get()
        {
            string? v = JsonConvert.SerializeObject(new Dictionary<string, string>()
            {
                { "status", "success"},
                { "token" , JsonConvert.SerializeObject(new Dictionary<string, string> { { "token", "test123"} }) }
            });
            return v ?? "{\"error\":\"Object not found\"}";
            //return new string[] { "value1", "value2" };
        }

        // POST api/<LoginController>
        [HttpPost]
        [Route("user")]
        public string Post(UserDetail value)
        {
            string? v = null;
            if (_configuration.AllowedUsers.Any(x => x.UserName.Equals(value.UserName) && x.Password.Equals(value.Password)))  //Contains(value))
            {
                v = JsonConvert.SerializeObject(new Dictionary<string, string>(){
                    { "status", "success"},
                    { "token" , JsonConvert.SerializeObject(new Dictionary<string, string> { { "token", "test123"} }) }
                });
            }
            //string? v = JsonConvert.SerializeObject(new Dictionary<string, string>() { { "token", "test123" } });
            return v ?? "{\"status\":\"Object not found\"}";
        }
    }
}
