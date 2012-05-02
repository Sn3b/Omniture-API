using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;

namespace Omniture.CustomToken
{
    internal class ClientCredentialsSecurityTokenManager : System.ServiceModel.ClientCredentialsSecurityTokenManager
    {
        #region Fields

        private readonly ClientCredentials _clientCredentials;

        #endregion

        #region Constructors

        internal ClientCredentialsSecurityTokenManager( ClientCredentials clientCredentials )
            : base( clientCredentials )
        {
            _clientCredentials = clientCredentials;
        }

        #endregion

        #region Overrides

        public override System.IdentityModel.Selectors.SecurityTokenProvider CreateSecurityTokenProvider(
            SecurityTokenRequirement tokenRequirement )
        {
            // handle this token for Custom
            if ( tokenRequirement.TokenType == Constants.UsernameTokenType )
                return new SecurityTokenProvider( _clientCredentials.Info );

            // return other tokens, e.g. server cert if needed
            if ( tokenRequirement is InitiatorServiceModelSecurityTokenRequirement )
            {
                if ( tokenRequirement.TokenType == SecurityTokenTypes.X509Certificate )
                {
                    return new X509SecurityTokenProvider( _clientCredentials.ServiceCertificate.DefaultCertificate );
                }
            }

            return base.CreateSecurityTokenProvider( tokenRequirement );
        }

        public override System.IdentityModel.Selectors.SecurityTokenSerializer CreateSecurityTokenSerializer(
            SecurityTokenVersion version )
        {
            return new SecurityTokenSerializer( version );
        }

        #endregion
    }
}
