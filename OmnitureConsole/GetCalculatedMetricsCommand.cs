using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManyConsole;
using Newtonsoft.Json;
using OmnitureConsole.CommandParameters;

namespace OmnitureConsole
{
    public class GetCalculatedMetricsCommand : ConsoleCommand
    {
        public EndpointParameters Endpoint;
        public string Suite;

        public GetCalculatedMetricsCommand()
        {
            this.IsCommand("get-metrics", "Lists calculated metrics.");
            this.HasRequiredOption("r=", "Report suite to use", v => Suite = v);

            Endpoint = new EndpointParameters();
            Endpoint.ApplyTo(this);
        }

        public override int Run(string[] remainingArguments)
        {
            var client = Endpoint.GetClient();

            //  https://developer.omniture.com/en_US/documentation/omniture-administration/r-getcalculatedmetrics
            var metrics = client.ReportSuiteGetCalculatedMetrics(new[] {Suite});

            foreach (var metric in metrics)

            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(metric, Formatting.Indented));
                Console.WriteLine();
            }

            return 0;
        }
    }
}
