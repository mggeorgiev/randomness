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

namespace rondomness
{
    public class GenerateRandomPoints
    {
        private readonly ILogger<GenerateRandomPoints> _logger;

        public GenerateRandomPoints(ILogger<GenerateRandomPoints> log)
        {
            _logger = log;
        }

        [FunctionName("Clusterring")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "points", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Points** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "The 400 response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string points = req.Query["points"];
            int executions = 8;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            points = points ?? data?.points;

            if(points != null)
                executions = Convert.ToInt32(points);
            if (executions > 1000)
                return new BadRequestObjectResult("Cannot return more than 1000 points");

            var listOfpoints = new List<Point>();

            for (int i = 0; i < executions; i++)
            {
                Point point = new Point();
                point.x = GeneratePoint();
                point.y = GeneratePoint();

                listOfpoints.Add(point);
            }

            string responseMessage = JsonConvert.SerializeObject(listOfpoints, Formatting.Indented);

            return new OkObjectResult(responseMessage);
        }


        private double GeneratePoint()
        {
            Random r = new Random();
            var point = r.NextDouble();
            //int rInt = r.Next(0, 100); //for ints
            //int range = 100;
            //double rDouble = r.NextDouble() * range;
            return (double)point;
        }
    }

    public class Point  {
        public double x { get; set; }
        public double y { get; set; }
    }

}

