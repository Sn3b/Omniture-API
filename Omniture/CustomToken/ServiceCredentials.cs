using System;
using System.IdentityModel.Selectors;

namespace Omniture.CustomToken
{
    /// <summary>
    /// ServiceCredentials for use with the Username Token. It serves up a Custom SecurityTokenManager
    /// ServiceCredentialsSecurityTokenManager - that knows about our custom token.
    /// </summary>
    /// 
    internal class ServiceCredentials : System.ServiceModel.Description.ServiceCredentials
    {
        #region Fields

        private readonly PasswordProvider _validator;

        #endregion

        #region Properties

        internal PasswordProvider Validator { get { return _validator; } }

        #endregion

        #region Constructors

        internal ServiceCredentials( PasswordProvider validator )
        {
            if ( validator == null )
                throw new ArgumentNullException( "validator" );
            _validator = validator;
        }

        #endregion

        #region Overrides

        protected override System.ServiceModel.Description.ServiceCredentials CloneCore()
        {
            return new ServiceCredentials( _validator );
        }
        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new ServiceCredentialsSecurityTokenManager( this );
        }

        #endregion
    }
}
