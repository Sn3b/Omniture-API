using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;

namespace Omniture.CustomToken
{
    /// <summary>
    /// SecurityTokenParameters for use with the Custom Token
    /// </summary>
    internal class SecurityTokenParameters : System.ServiceModel.Security.Tokens.SecurityTokenParameters
    {
        #region Properties

        // A username token has no crypto, no windows identity and supports only client authentication
        protected override bool HasAsymmetricKey { get { return false; } }
        protected override bool SupportsClientAuthentication { get { return true; } }
        protected override bool SupportsClientWindowsIdentity { get { return false; } }
        protected override bool SupportsServerAuthentication { get { return false; } }

        #endregion

        #region Overrides

        protected override System.ServiceModel.Security.Tokens.SecurityTokenParameters CloneCore()
        {
            return new SecurityTokenParameters();
        }
        protected override void InitializeSecurityTokenRequirement( SecurityTokenRequirement requirement )
        {
            requirement.TokenType = Constants.UsernameTokenType;
        }
        protected override SecurityKeyIdentifierClause CreateKeyIdentifierClause( System.IdentityModel.Tokens.SecurityToken token, SecurityTokenReferenceStyle referenceStyle )
        {
            if ( referenceStyle == SecurityTokenReferenceStyle.Internal )
                return token.CreateKeyIdentifierClause<LocalIdKeyIdentifierClause>();
            
            throw new NotSupportedException( "External references are not supported for tokens" );
        }

        #endregion
    }
}
