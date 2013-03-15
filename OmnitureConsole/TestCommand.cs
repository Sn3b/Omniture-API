using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManyConsole;
using Omniture;
using Omniture.Api13;

namespace OmnitureConsole
{
    public class TestCommand : ConsoleCommand
    {
        public string Username;
        public string Secret;
        public string Endpoint = "https://api.omniture.com/admin/1.3/";

        public TestCommand()
        {
            this.IsCommand("test-api-access", "Visit https://developer.omniture.com/en_US/content_page/enterprise-api/c-get-web-service-access-to-the-enterprise-api to setup API access for a user and to find your API secret key.");
            this.HasRequiredOption("u=", "Username", v => Username = v);
            this.HasRequiredOption("s=", "Secret key", v => Secret = v);
            this.HasOption("e=", "Endpoint url used for requests.", v => Endpoint = v);
        }

        public override int Run(string[] remainingArguments)
        {
            var client = ClientHelper.GetClient(Username, Secret, Endpoint);

            //https://developer.omniture.com/en_US/documentation/omniture-administration/r-getreportsuites
            simple_report_suites_rval results = client.CompanyGetReportSuites(new[] {"standard"}, "");

            Console.WriteLine("Response received, success!");
            Console.WriteLine("Entire response: " + Newtonsoft.Json.JsonConvert.SerializeObject(results));

            Console.WriteLine("Suites listed:");
            foreach (var suite in results.report_suites)
            {
                Console.WriteLine("Suite: {0}, rsid: {1}", suite.site_title, suite.rsid);

                var calculatedMetrics = client.ReportSuiteGetCalculatedMetrics(new [] { suite.rsid });

                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(calculatedMetrics));
            }

            return 0;
        }
    }
}
