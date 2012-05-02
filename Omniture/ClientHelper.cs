using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Omniture.Api13;
using Omniture.Channels;
using Omniture.CustomToken;

namespace Omniture
{
    public static class ClientHelper
    {
        public static OmnitureWebServicePortTypeClient GetClient( string username, string secret, string endpoint )
        {
            var httpsTransportBindingElement = new HttpsTransportBindingElement
                                                   {
                                                       UseDefaultWebProxy = false,
                                                       MaxReceivedMessageSize = 2147483647
                                                   };

            var transportSecurityBindingElement = new TransportSecurityBindingElement();
            transportSecurityBindingElement.EndpointSupportingTokenParameters.SignedEncrypted.Add( new SecurityTokenParameters() );
            transportSecurityBindingElement.IncludeTimestamp = false;

            var customTextMessageBindingElement = new CustomTextMessageBindingElement { MessageVersion = MessageVersion.Soap11 };

            var bindingElements = new List<BindingElement>
                                  {
                                      transportSecurityBindingElement,
                                      customTextMessageBindingElement,
                                      httpsTransportBindingElement
                                  };

            Binding customBinding = new CustomBinding( bindingElements.ToArray() );
            var endpointAddress = new EndpointAddress( endpoint );
            var clientCredential = new ClientCredentials( new Info( username, secret ) );

            var omnitureWebServicePortTypeClient = new OmnitureWebServicePortTypeClient( customBinding, endpointAddress );
            omnitureWebServicePortTypeClient.Endpoint.Behaviors.Remove( typeof( System.ServiceModel.Description.ClientCredentials ) );
            omnitureWebServicePortTypeClient.Endpoint.Behaviors.Add( clientCredential );

            return omnitureWebServicePortTypeClient;
        }
    }
}
