using System;

namespace Omniture.CustomToken
{
    /// <summary>
    /// SecurityTokenProvider for use with the Custom Token
    /// </summary>
    internal class SecurityTokenProvider : System.IdentityModel.Selectors.SecurityTokenProvider
    {
        #region Fields

        private readonly Info _info;

        #endregion

        #region Constructors

        internal SecurityTokenProvider( Info info )
        {
            if ( info == null )
                throw new ArgumentNullException( "info" );

            _info = info;
        }

        #endregion

        #region Overrides

        protected override System.IdentityModel.Tokens.SecurityToken GetTokenCore( TimeSpan timeout )
        {
            System.IdentityModel.Tokens.SecurityToken result = new SecurityToken( _info );
            return result;
        }

        #endregion
    }
}
