using BookMeet.API.APIModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace BookMeet.API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        ///  To generate access token
        /// </summary>
        /// <returns></returns>
        [HttpPost("generatetoken")]
        public async Task<IActionResult> GenerateToken()
        {
            string json = String.Empty;
            TokenResponse? tokenResponse = new TokenResponse();
            var instance = _configuration.GetValue<string>("AzureAd:Instance");
            var client = _configuration.GetValue<string>("AzureAd:ClientId");
            var tenant = _configuration.GetValue<string>("AzureAd:TenantId");
            var sClient = _configuration.GetValue<string>("AzureAd:ClientSecret");
            var type = _configuration.GetValue<string>("AzureAd:grant_type");
            var scope = _configuration.GetValue<string>("AzureAd:Scope");
            var authUrl = instance + tenant + "/oauth2/v2.0/token";

            Dictionary<string, string> req = new Dictionary<string, string>();
            req.Add("client_id", client.ToString());
            req.Add("client_secret", sClient.ToString());
            req.Add("grant_type", type.ToString());
            req.Add("scope", scope.ToString());

            using (var httpClient = new HttpClient())
            {

                var requestContent = new FormUrlEncodedContent(req);
                string url = $"{authUrl}";

                var result = await httpClient.PostAsync(url, requestContent);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    using (HttpContent content = result.Content)
                    {
                        json = content.ReadAsStringAsync().Result;
                        tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
                    }
                }
                else
                {
                    return BadRequest("Error while generating Token.");
                }
                return Ok(new
                {
                    accessToken = "Bearer " + tokenResponse?.access_token

                });
            }

        }
    }
}
