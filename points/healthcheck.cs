using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace rondomness
{
    public class Healthcheck
    {
        public Healthcheck()
        {
        }

        [Function("healthcheck")]
        public Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, FunctionContext context)
        {
            var logger = context.GetLogger<Healthcheck>();
            logger.LogInformation("C# HTTP trigger function processed a request.");
            string responce = "Random points API is running OK!";

            string responseMessage = JsonConvert.SerializeObject(responce, Formatting.Indented);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(responseMessage);
            return Task.FromResult(response);
        }
    }
}

