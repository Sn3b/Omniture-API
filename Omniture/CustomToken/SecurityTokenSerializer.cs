using System;
using System.IdentityModel.Selectors;
using System.ServiceModel.Security;
using System.Xml;

namespace Omniture.CustomToken
{
    /// <summary>
    /// SecurityTokenSerializer for use with the Custom Token
    /// </summary>
    /// 
    internal class SecurityTokenSerializer : WSSecurityTokenSerializer
    {
        #region Constructors

        internal SecurityTokenSerializer( SecurityTokenVersion version )
        { }

        #endregion

        #region Overrides

        protected override bool CanReadTokenCore( XmlReader reader )
        {
            //XmlDictionaryReader localReader = XmlDictionaryReader.CreateDictionaryReader( reader );

            if ( reader == null )
                throw new ArgumentNullException( "reader" );

            if ( reader.IsStartElement( Constants.UsernameTokenName, Constants.UsernameTokenNamespace ) )
                return true;

            return base.CanReadTokenCore( reader );
        }
        protected override System.IdentityModel.Tokens.SecurityToken ReadTokenCore( XmlReader reader, SecurityTokenResolver tokenResolver )
        {
            if ( reader == null )
                throw new ArgumentNullException( "reader" );

            if ( reader.IsStartElement( Constants.UsernameTokenName, Constants.UsernameTokenNamespace ) )
            {
                //string id = reader.GetAttribute( Constants.IdAttributeName, Constants.WsUtilityNamespace );

                reader.ReadStartElement();

                // read the user name
                string userName = reader.ReadElementString( Constants.UsernameElementName, Constants.UsernameTokenNamespace );

                // read the password hash
                string password = reader.ReadElementString( Constants.PasswordElementName, Constants.UsernameTokenNamespace );

                // read nonce
                string nonce = reader.ReadElementString( Constants.NonceElementName, Constants.UsernameTokenNamespace );

                // read created
                string created = reader.ReadElementString( Constants.CreatedElementName, Constants.WsUtilityNamespace );

                reader.ReadEndElement();

                var info = new Info( userName, password );

                return new SecurityToken( info, nonce, created );
            }
            
            return DefaultInstance.ReadToken( reader, tokenResolver );
        }
        protected override bool CanWriteTokenCore( System.IdentityModel.Tokens.SecurityToken token )
        {
            if ( token is SecurityToken )
                return true;
            
            return base.CanWriteTokenCore( token );
        }

        protected override void WriteTokenCore( XmlWriter writer, System.IdentityModel.Tokens.SecurityToken token )
        {
            if ( writer == null )
                throw new ArgumentNullException( "writer" );

            if ( token == null )
                throw new ArgumentNullException( "token" );

            var c = token as SecurityToken;
            if ( c != null )
            {
                writer.WriteStartElement( Constants.UsernameTokenPrefix, Constants.UsernameTokenName, Constants.UsernameTokenNamespace );
                writer.WriteAttributeString( Constants.WsUtilityPrefix, Constants.IdAttributeName, Constants.WsUtilityNamespace, token.Id );
                writer.WriteElementString( Constants.UsernameElementName, Constants.UsernameTokenNamespace, c.Info.Username );
                writer.WriteStartElement( Constants.UsernameTokenPrefix, Constants.PasswordElementName, Constants.UsernameTokenNamespace );
                writer.WriteAttributeString( Constants.TypeAttributeName, Constants.PasswordDigestType );
                writer.WriteValue( c.GetPasswordDigestAsBase64() );
                writer.WriteEndElement();
                writer.WriteElementString( Constants.NonceElementName, Constants.UsernameTokenNamespace, c.GetNonceAsBase64() );
                writer.WriteElementString( Constants.CreatedElementName, Constants.WsUtilityNamespace, c.GetCreatedAsString() );
                writer.WriteEndElement();
                writer.Flush();
            }
            else
                base.WriteTokenCore( writer, token );
        }

        #endregion
    }
}
