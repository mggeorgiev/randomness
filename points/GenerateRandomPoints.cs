using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace points
{
    public class GenerateRandomPoints
    {
        private readonly ILogger<GenerateRandomPoints>? _logger;

        public GenerateRandomPoints()
        {
        }

        public GenerateRandomPoints(ILogger<GenerateRandomPoints> log)
        {
            _logger = log;
        }

        [Function("points")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext? context = null)
        {
            var logger = context != null ? context.GetLogger<GenerateRandomPoints>() : (ILogger<GenerateRandomPoints>?)_logger;
            logger?.LogInformation("C# HTTP trigger function processed a request.");

            // use GeneratePoints to produce result


            string points = null;

            // parse query string for 'points'
            var query = req.Url.Query;
            if (!string.IsNullOrEmpty(query))
            {
                var kvpairs = query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries);
                foreach (var kv in kvpairs)
                {
                    var parts = kv.Split('=', 2);
                    if (parts.Length == 2 && parts[0] == "points")
                    {
                        points = Uri.UnescapeDataString(parts[1]);
                        break;
                    }
                }
            }

            int executions = 8;

            using (var reader = new StreamReader(req.Body))
            {
                string requestBody = await reader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                points = points ?? data?.points;
            }

            var badResponse = req.CreateResponse();

            if (string.IsNullOrEmpty(points))
            {
                badResponse.StatusCode = HttpStatusCode.BadRequest;
                badResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                badResponse.WriteString("Points parameter that specifies the number of points is required. Cannot return less than 1 and more than 1000 points");
                return badResponse;
            }

            if (!int.TryParse((string)points, out executions))
            {
                badResponse.StatusCode = HttpStatusCode.BadRequest;
                badResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                badResponse.WriteString("Points parameter must be an integer");
                return badResponse;
            }

            if (executions > 1000 || executions < 1)
            {
                badResponse.StatusCode = HttpStatusCode.BadRequest;
                badResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                badResponse.WriteString("Cannot return less than 1 and more than 1000 points");
                return badResponse;
            }

            var listOfpoints = new List<Point>();

            var rnd = new Random();
            for (int i = 0; i < executions; i++)
            {
                Point point = new Point
                {
                    x = rnd.NextDouble(),
                    y = rnd.NextDouble()
                };

                listOfpoints.Add(point);
            }

            string responseMessage = JsonConvert.SerializeObject(listOfpoints, Formatting.Indented);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(responseMessage);
            return response;
        }

        // Exposed for unit testing: generate n points and validate input
        public List<Point> GeneratePoints(int executions)
        {
            if (executions < 1 || executions > 1000)
                throw new ArgumentOutOfRangeException(nameof(executions), "Executions must be between 1 and 1000");

            var listOfpoints = new List<Point>();
            var rnd = new Random();
            for (int i = 0; i < executions; i++)
            {
                Point point = new Point
                {
                    x = rnd.NextDouble(),
                    y = rnd.NextDouble()
                };

                listOfpoints.Add(point);
            }

            return listOfpoints;
        }
    }

    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
    }

}

