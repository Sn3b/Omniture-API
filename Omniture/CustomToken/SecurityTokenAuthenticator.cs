using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;

namespace Omniture.CustomToken
{
    /// <summary>
    /// SecurityTokenAuthenticator for use with the Custom Token
    /// This validates the username token against a directory of username-password pairs
    /// </summary>
    internal class SecurityTokenAuthenticator : System.IdentityModel.Selectors.SecurityTokenAuthenticator
    {
        #region Fields

        private readonly PasswordProvider _passwordProvider;

        #endregion

        #region Constructors

        internal SecurityTokenAuthenticator( PasswordProvider passwordProvider )
        {
            _passwordProvider = passwordProvider;
        }

        #endregion

        #region Overrides

        protected override bool CanValidateTokenCore( System.IdentityModel.Tokens.SecurityToken token )
        {
            return ( token is SecurityToken );
        }
        protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore( System.IdentityModel.Tokens.SecurityToken token )
        {
            var securityToken = token as SecurityToken;

            if ( securityToken != null )
            {
                // Note that we cannot authenticate the token w/o a password, so it must be retrieved from somewhere
                if ( securityToken.ValidateToken( _passwordProvider.RetrievePassword( "User1" ) ) != true )
                    throw new SecurityTokenValidationException( "Token validation failed" );

                // Add claims about user here
                var userClaimSet = new DefaultClaimSet( new Claim( ClaimTypes.Name, securityToken.Info.Username, Rights.PossessProperty ) );

                var policies = new List<IAuthorizationPolicy>( 1 ) { new SecurityTokenAuthorizationPolicy( userClaimSet ) };
                return policies.AsReadOnly();
            }

            return null;
        }

        #endregion
    }
}
