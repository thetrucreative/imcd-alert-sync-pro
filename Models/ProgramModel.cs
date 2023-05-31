using Nest;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Elasticsearch.Net;
using Elasticsearch;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using HttpMethod = System.Net.Http.HttpMethod;
using Newtonsoft.Json.Linq;
using imcd_alert_sync_pro.Controllers;

namespace imcd_alert_sync_pro.Models
{
    public class ProgramModel
    {
        //1.GET OpsGenie alerts
        //2.PUT or POST to Opensearch and create an index
        //3.Display results of indexed alerts on a browser
        //4.Cron job to run steps 1-3 every 60sec
        //5.Clear content on browser
        //6.Display new content
        //7.Repeat steps 1-7
        private readonly ILogger<ProgramModel> _logger; 
        private readonly IConfiguration _configuration;
        public ProgramModel(ILogger<ProgramModel> logger, IConfiguration configuration) 
        { 
            _logger = logger;
            _configuration = configuration; 
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();  
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<StartupModel>();
            });

        public static async Task<List<AlertData>?> GetOpsgenieAlerts()
        {
            //_logger.LogInformation($"[START] GET alerts from OpsGenie...");

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "YOUR_OPSGENIE_ENDPOINT");
            
            request.Headers.Add("Authorization", "GenieKey YOUR_OPSGENIE_APIKEY");  
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseContent);
                var opsgenieAlertsList = responseObject["data"]?.ToObject<List<AlertData>>();

                //convert List<AlertData> to List<AlertsModel>
                //_logger.LogInformation($"[END] GET alerts from OpsGenie");
                return opsgenieAlertsList;
            }
            else
            {
                return null;
            }
        }

        public static async Task PostToOpensearch(ILogger<ProgramModel> logger)
        {

            logger.LogInformation($"[START] Indexing to OpenSearch...");

            var connectionSettings = new ConnectionSettings(new Uri("YOUR_OPENSEARCH_ENDPOINT"))
            .DefaultIndex("your_opensearch_index")
            .BasicAuthentication("your_username", "your_password");

            var client = new ElasticClient(connectionSettings);
            List<AlertData> opsgenieAlertsList = await GetOpsgenieAlerts();

            for (int i = 0; i < opsgenieAlertsList?.Count; i++)
            {
                var indexedOpsgenieAlert = opsgenieAlertsList[i];
                var indexResponse = await client.IndexDocumentAsync(indexedOpsgenieAlert);

                if (!indexResponse.IsValid)
                {
                    //log failure to index msg
                    logger.LogInformation($"Failed to index OpsGenie Alerts");
                }
                {
                    //log success msg
                    logger.LogInformation($"Alert indexed: {indexedOpsgenieAlert.Id}");
                }
            }
            logger.LogInformation($"[END] Indexing to OpenSearch");
        }
    }
}
