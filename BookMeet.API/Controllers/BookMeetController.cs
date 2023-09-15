using Azure;
using BookMeet.API.APIModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;
using BookMeet.API.Extensions;

namespace BookMeet.API.Controllers
{
    [Route("api/bookmeet")]
    [ApiController]
    [Authorize]
    public class BookMeetController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _token;

        public BookMeetController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _token = httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault() ?? "";
        }


        /// <summary>
        /// Get Avaialble Timeslot
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAvaialbleTimeslot")]
        public async Task<OperationResult<ScheduleResponse>> GetSchedule([FromBody] ScheduleRequest request)
        {
            string json = String.Empty;
            ScheduleResponse? schResponse = new ScheduleResponse();
            OperationResult<ScheduleResponse> response = new OperationResult<ScheduleResponse>();
            var instance = _configuration.GetValue<string>("AzureAd:GraphAPI");
            var postScheduleUrl = instance + request.Mail + "/calendar/getSchedule";
  
            var body = 
            "{\"schedules\": [\"" +  request.Mail + "\"],\"startTime\" : { \"dateTime\" :\"" +   request.StartDate.ToString() + "\",\"timeZone\" : \"UTC\" }, \"endTime\" : { \"dateTime\" :\"" +  request.EndDate.ToString() + "\", \"timeZone\" : \"UTC\"},\"availabilityViewInterval\" :" +  request.AvailabilityViewInterval + "}";

            var requestContent = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                string url = $"{postScheduleUrl}";
                using (var tempRequest = new HttpRequestMessage(new HttpMethod("POST"), url))
                {
                    tempRequest.Headers.TryAddWithoutValidation("Authorization", _token);
                    tempRequest.Headers.TryAddWithoutValidation("Accept", "*/*");
                    tempRequest.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                    tempRequest.Content = requestContent;
                    var result = await httpClient.SendAsync(tempRequest);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        using (HttpContent content = result.Content)
                        {
                            json = content.ReadAsStringAsync().Result;
                            schResponse.response = json;
                        }
                        response.Status = "200";
                        response.Error = "Data Received Successfully.!!!";
                        response.Data = schResponse;
                        response.HttpStatus = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Status = "400";
                        response.Error = "Someting went wrong";
                        response.HttpStatus = HttpStatusCode.BadRequest;
                    }
                    return response;
                }

            }
        }
    }
}