using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Omniture.CustomToken
{
    internal class SecurityToken : System.IdentityModel.Tokens.SecurityToken
    {
        #region Fields

        private readonly Info _info;
        private readonly ReadOnlyCollection<SecurityKey> _securityKeys;
        private readonly DateTime _created = DateTime.Now;
        private readonly DateTime _expiration = DateTime.Now + new TimeSpan( 10, 0, 0 );
        private readonly Guid _id = Guid.NewGuid();
        private readonly byte[] _nonce = new byte[ 16 ];

        #endregion

        #region Properties

        internal Info Info
        {
            get { return _info; }
        }
        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get { return _securityKeys; }
        }

        #endregion

        #region Constructors

        internal SecurityToken( Info info, string nonce, string created )
        {
            if ( info == null )
                throw new ArgumentNullException( "info" );

            _info = info;

            if ( nonce != null )
            {
                _nonce = Convert.FromBase64String( nonce );
            }

            if ( created != null )
            {
                _created = DateTime.Parse( created );
            }

            // the user name token is not capable of any crypto
            _securityKeys = new ReadOnlyCollection<SecurityKey>( new List<SecurityKey>() );
        }
        internal SecurityToken( Info usernameInfo )
            : this( usernameInfo, null, null )
        {
        }

        #endregion

        #region Internal Methods

        internal string GetPasswordDigestAsBase64()
        {
            // generate a cryptographically strong random value
            RandomNumberGenerator rndGenerator = new RNGCryptoServiceProvider();
            rndGenerator.GetBytes( _nonce );

            // get other operands to the right format
            byte[] time = Encoding.UTF8.GetBytes( GetCreatedAsString() );
            byte[] pwd = Encoding.UTF8.GetBytes( _info.Password );
            var operand = new byte[ _nonce.Length + time.Length + pwd.Length ];
            Array.Copy( _nonce, operand, _nonce.Length );
            Array.Copy( time, 0, operand, _nonce.Length, time.Length );
            Array.Copy( pwd, 0, operand, _nonce.Length + time.Length, pwd.Length );

            // create the hash
            SHA1 sha1 = SHA1.Create();
            return Convert.ToBase64String( sha1.ComputeHash( operand ) );
        }
        internal string GetNonceAsBase64()
        {
            return Convert.ToBase64String( _nonce );
        }
        internal string GetCreatedAsString()
        {
            return XmlConvert.ToString( _created.ToUniversalTime(), "yyyy-MM-ddTHH:mm:ssZ" );
        }
        internal bool ValidateToken( string password )
        {
            byte[] pwd = Encoding.UTF8.GetBytes( password );
            byte[] createdBytes = Encoding.UTF8.GetBytes( GetCreatedAsString() );
            var operand = new byte[ _nonce.Length + createdBytes.Length + pwd.Length ];
            Array.Copy( _nonce, operand, _nonce.Length );
            Array.Copy( createdBytes, 0, operand, _nonce.Length, createdBytes.Length );
            Array.Copy( pwd, 0, operand, _nonce.Length + createdBytes.Length, pwd.Length );
            SHA1 sha1 = SHA1.Create();
            string trueDigest = Convert.ToBase64String( sha1.ComputeHash( operand ) );

            return String.CompareOrdinal( trueDigest, _info.Password ) == 0;
        }

        #endregion

        #region Overrides

        public override DateTime ValidFrom
        {
            get { return _created; }
        }
        public override DateTime ValidTo
        {
            get { return _expiration; }
        }
        public override string Id
        {
            get { return _id.ToString(); }
        }

        #endregion
    }
}
