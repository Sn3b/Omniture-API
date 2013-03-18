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
        public SavedApiCredentials Credentials;
        public string Endpoint = "https://api.omniture.com/admin/1.3/";

        public TestCommand()
        {
            this.IsCommand("test-api-access", "Visit https://developer.omniture.com/en_US/content_page/enterprise-api/c-get-web-service-access-to-the-enterprise-api to setup API access for a user and to find your API secret key.");
            this.HasOption("e=", "Endpoint url used for requests.", v => Endpoint = v);

            Credentials = new SavedApiCredentials();
            Credentials.ApplyTo(this);
        }

        public override int Run(string[] remainingArguments)
        {
            var client = ClientHelper.GetClient(Credentials.Username, Credentials.Secret, Endpoint);

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
