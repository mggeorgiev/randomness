using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace afn_random_functions
{
    public class GenerateRandomPoints
    {
        private readonly ILogger<GenerateRandomPoints> _logger;

        public GenerateRandomPoints(ILogger<GenerateRandomPoints> logger)
        {
            _logger = logger;
        }

        [Function("points")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            _logger.LogInformation(req.ToString());
            string points = req.Query["points"];
            int executions = 8;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            points = points ?? data?.points;

            if (points == null || points == "")
                return new BadRequestObjectResult("Points parameter that scpecifies the number of points is requried. Cannot return less then 1 and more than 1000 points");

            executions = Convert.ToInt32(points);

            if (executions > 1000 || executions < 1)
                return new BadRequestObjectResult("Cannot return less then 1 and more than 1000 points");

            var listOfpoints = new List<Point>();

            for (int i = 0; i < executions; i++)
            {
                Point point = new Point
                {
                    x = GeneratePoint(),
                    y = GeneratePoint()
                };

                listOfpoints.Add(point);
            }

            string responseMessage = JsonConvert.SerializeObject(listOfpoints, Formatting.Indented);

            return new OkObjectResult(responseMessage);
        }

        private double GeneratePoint()
        {
            Random r = new Random();
            var point = r.NextDouble();
            return (double)point;
        }
    }
    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
    }
}
