using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManyConsole;
using Omniture;
using Omniture.Api13;

namespace OmnitureConsole.CommandParameters
{
    public class EndpointParameters
    {
        public ApiCredentialParameters Credentials;
        public string Endpoint = "https://api.omniture.com/admin/1.3/";

        public void ApplyTo(ConsoleCommand command)
        {
            command.HasOption("e=", "Endpoint url used for requests.", v => Endpoint = v);

            Credentials = new ApiCredentialParameters();
            Credentials.ApplyTo(command);
        }

        public override string ToString()
        {
            var uri = new UriBuilder(Endpoint);
            uri.UserName = Credentials.Username;

            return uri.Uri.AbsoluteUri;
        }

        public OmnitureWebServicePortTypeClient GetClient()
        {
            var client = ClientHelper.GetClient(Credentials.Username, Credentials.Secret, Endpoint);
            return client;
        }
    }
}
