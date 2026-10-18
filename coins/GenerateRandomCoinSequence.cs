using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace coins
{
    public class GenerateRandomCoinSequence
    {
        public GenerateRandomCoinSequence()
        {
        }

        [Function("coins")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext context)
        {
            var logger = context.GetLogger<GenerateRandomCoinSequence>();
            logger.LogInformation("C# HTTP trigger function processed a request.");

                        string flips = null;
            string groups = null;

            // parse query
            var query = req.Url.Query;
            if (!string.IsNullOrEmpty(query))
            {
                var kvpairs = query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries);
                foreach (var kv in kvpairs)
                {
                    var parts = kv.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        if (parts[0] == "flips") flips = Uri.UnescapeDataString(parts[1]);
                        if (parts[0] == "groups") groups = Uri.UnescapeDataString(parts[1]);
                    }
                }
            }

            using (var reader = new StreamReader(req.Body))
            {
                string requestBody = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(requestBody))
                {
                    dynamic data = JsonConvert.DeserializeObject(requestBody);
                    flips = flips ?? data?.flips;
                    groups = groups ?? data?.groups;
                }
            }

            int flipsToDeliver = 16;

            if (flips != null)
                flipsToDeliver = Convert.ToInt32(flips);
            if (flipsToDeliver > 100)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                bad.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                bad.WriteString("Cannot return more than 100 coin flips.");
                return bad;
            }

            int groupsToDeliver = 8;
            if (groups != null)
                groupsToDeliver = Convert.ToInt32(groups);
            if (groupsToDeliver > 100 || groupsToDeliver < 1)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                bad.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                bad.WriteString("Cannot return less then 1 and more than 100 groups of flips.");
                return bad;
            }

            var listOfListOfFips = new List<List<string>>();

            var rnd = new Random();
            for (int n = 0; n < groupsToDeliver; n++)
            { 
                var listOfFips = new List<string>();
                for (int i = 0; i < flipsToDeliver; i++)
                {
                    var tossCoin = rnd.NextDouble();
                    var result = "T";

                    if (tossCoin > 0.5) result = "H";

                    listOfFips.Add(result);
                }
                listOfListOfFips.Add(listOfFips);
            }

            var rb = new RootObject();
            rb.FlipsPerGroup = flipsToDeliver;
            rb.NumberOfGroups = groupsToDeliver;
            rb.Data = listOfListOfFips;
            string responseMessage = JsonConvert.SerializeObject(rb, Formatting.Indented);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(responseMessage);
            return response;
        }

        // Exposed for unit testing: generate a coin sequence using same rules
        public RootObject GenerateSequence(int flipsToDeliver, int groupsToDeliver)
        {
            if (flipsToDeliver < 1 || flipsToDeliver > 100)
                throw new ArgumentOutOfRangeException(nameof(flipsToDeliver), "Cannot return more than 100 coin flips.");
            if (groupsToDeliver < 1 || groupsToDeliver > 100)
                throw new ArgumentOutOfRangeException(nameof(groupsToDeliver), "Cannot return less then 1 and more than 100 groups of flips.");

            var listOfListOfFips = new List<List<string>>();
            var rnd = new Random();
            for (int n = 0; n < groupsToDeliver; n++)
            {
                var listOfFips = new List<string>();
                for (int i = 0; i < flipsToDeliver; i++)
                {
                    var tossCoin = rnd.NextDouble();
                    var result = tossCoin > 0.5 ? "H" : "T";
                    listOfFips.Add(result);
                }
                listOfListOfFips.Add(listOfFips);
            }

            var rbOut = new RootObject();
            rbOut.FlipsPerGroup = flipsToDeliver;
            rbOut.NumberOfGroups = groupsToDeliver;
            rbOut.Data = listOfListOfFips;
            return rbOut;
        }
    }

    public class RootObject
    {
        public int NumberOfGroups { get; set; }
        public int FlipsPerGroup { get; set; }
        public List<List<string>> Data { get; set; }
    }
}

