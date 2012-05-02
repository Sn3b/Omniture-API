using System;
using System.IdentityModel.Selectors;

namespace Omniture.CustomToken
{
    /// <summary>
    /// ClientCredentials for use with Custom Token
    /// </summary>
    internal class ClientCredentials : System.ServiceModel.Description.ClientCredentials
    {
        #region Fields

        private readonly Info _info;

        #endregion

        #region Properties

        internal Info Info
        {
            get { return _info; }
        }

        #endregion

        #region Constructors

        internal ClientCredentials( Info info )
        {
            if ( info == null )
                throw new ArgumentNullException( "info" );

            _info = info;
        }

        #endregion

        #region Overrides

        protected override System.ServiceModel.Description.ClientCredentials CloneCore()
        {
            return new ClientCredentials( _info );
        }

        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new ClientCredentialsSecurityTokenManager( this );
        }

        #endregion
    }
}
