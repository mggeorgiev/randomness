using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace coins
{
    public class GenerateRandonCoinSequence
    {
        private readonly ILogger<GenerateRandonCoinSequence> _logger;

        public GenerateRandonCoinSequence(ILogger<GenerateRandonCoinSequence> log)
        {
            _logger = log;
        }

        [FunctionName("coins")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "throws", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Throw** parameter.")]
        [OpenApiParameter(name: "groups", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Groups** parameter. How many sets of throws to be returned.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(RootObject), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "The 400 response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            //var datas = new ResultData();

            string throws = req.Query["throws"];
            string groups = req.Query["groups"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            throws = throws ?? data?.throws;
            groups = groups ?? data?.groups;

            int tossToDeliver = 16;

            if (throws != null)
                tossToDeliver = Convert.ToInt32(throws);
            if (tossToDeliver > 100)
                return new BadRequestObjectResult("Cannot return more than 100 coin throws.");

            int groupsToDeliver = 8;
            if (groups != null)
                groupsToDeliver = Convert.ToInt32(throws);
            if (groupsToDeliver > 100 && groupsToDeliver < 1)
                return new BadRequestObjectResult("Cannot return less then 1 and more than 100 groups of throws.");

            var listofListOfThrows = new List<List<string>>();

            for (int n = 0; n < groupsToDeliver; n++)
            { 
                var listOfThrows = new List<string>();
                for (int i = 0; i < tossToDeliver; i++)
                {
                    Random r = new Random();
                    var tossCoin = r.NextDouble();
                    var result = "T";

                    if (tossCoin > 0.5) result = "H";

                    listOfThrows.Add(result);
                    //datas.Add(result);
                }
                listofListOfThrows.Add(listOfThrows);
                //listOfThrows.Clear();
            }

            string defaultResponseMessage = string.IsNullOrEmpty(throws)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, toy requested a sequence of {throws}. This HTTP triggered function executed successfully.";

            var rb = new RootObject();
            rb.TossesPerGroup = tossToDeliver;
            rb.NumberOfGroups = groupsToDeliver;
            rb.Data = listofListOfThrows;
            string responseMessage = JsonConvert.SerializeObject(rb, Formatting.Indented);

            return new OkObjectResult(responseMessage);
        }
    }

    public class RootObject
    {
        public int NumberOfGroups { get; set; }
        public int TossesPerGroup { get; set; }
        public List<List<string>> Data { get; set; }
    }
}

