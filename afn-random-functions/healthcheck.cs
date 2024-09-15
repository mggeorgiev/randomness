using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Net;

namespace afn_random_functions
{
    public class HealthCheck
    {
        private readonly ILogger<HealthCheck> _logger;

        public HealthCheck(ILogger<HealthCheck> logger)
        {
            _logger = logger;
        }

        [OpenApiOperation(operationId: "Run", tags: new[] { "name" }, Summary = "Healthcheck", Description = "This endpoint returns the healthcheck of the function.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "The 400 response")]
        [Function("healthcheck")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string responce = "Randomness API is running OK!";

            string responseMessage = JsonConvert.SerializeObject(responce, Newtonsoft.Json.Formatting.Indented);

            return new OkObjectResult(responseMessage);
        }
    }
}
