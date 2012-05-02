using System.IdentityModel.Selectors;

namespace Omniture.CustomToken
{
    internal class ServiceCredentialsSecurityTokenManager : System.ServiceModel.Security.ServiceCredentialsSecurityTokenManager
    {
        #region Fields

        private readonly ServiceCredentials _serviceCredentials;

        #endregion

        #region Constructors

        internal ServiceCredentialsSecurityTokenManager( ServiceCredentials serviceCredentials )
            : base( serviceCredentials )
        {
            _serviceCredentials = serviceCredentials;
        }

        #endregion

        #region Overrides

        public override System.IdentityModel.Selectors.SecurityTokenAuthenticator CreateSecurityTokenAuthenticator( SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver )
        {
            if ( tokenRequirement.TokenType == Constants.UsernameTokenType )
            {
                outOfBandTokenResolver = null;
                return new SecurityTokenAuthenticator( _serviceCredentials.Validator );
            }

            return base.CreateSecurityTokenAuthenticator( tokenRequirement, out outOfBandTokenResolver );
        }
        public override System.IdentityModel.Selectors.SecurityTokenSerializer CreateSecurityTokenSerializer( SecurityTokenVersion version )
        {
            return new SecurityTokenSerializer( version );
        }

        #endregion
    }
}
