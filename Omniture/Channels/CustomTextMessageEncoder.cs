using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace Omniture.Channels
{
    internal class CustomTextMessageEncoder : MessageEncoder
    {
        #region Fields

        private readonly CustomTextMessageEncoderFactory _factory;
        private readonly XmlWriterSettings _writerSettings;
        private readonly string _contentType;

        #endregion

        #region Properties

        public override string ContentType
        {
            get { return _contentType; }
        }
        public override string MediaType
        {
            get { return _factory.MediaType; }
        }
        public override MessageVersion MessageVersion
        {
            get { return _factory.MessageVersion; }
        }

        #endregion

        #region Constructors

        public CustomTextMessageEncoder( CustomTextMessageEncoderFactory factory )
        {
            _factory = factory;
            _writerSettings = new XmlWriterSettings { Encoding = Encoding.GetEncoding( factory.CharSet ) };
            _contentType = string.Format( "{0}; charset={1}", _factory.MediaType, _writerSettings.Encoding.HeaderName );
        }

        #endregion

        #region Overrides

        public override Message ReadMessage( ArraySegment<byte> buffer, BufferManager bufferManager, string contentType )
        {
            var msgContents = new byte[ buffer.Count ];
            Array.Copy( buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length );
            bufferManager.ReturnBuffer( buffer.Array );

            var stream = new MemoryStream( msgContents );
            return ReadMessage( stream, int.MaxValue );
        }
        public override Message ReadMessage( Stream stream, int maxSizeOfHeaders, string contentType )
        {
            XmlReader reader = XmlReader.Create( new StreamReader( stream, new UTF8Encoding( false, false ) ) );
            return Message.CreateMessage( reader, maxSizeOfHeaders, MessageVersion );
        }
        public override ArraySegment<byte> WriteMessage( Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset )
        {
            var stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create( stream, _writerSettings );
            message.WriteMessage( writer );
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            var messageLength = ( int )stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer( totalLength );
            Array.Copy( messageBytes, 0, totalBytes, messageOffset, messageLength );

            var byteArray = new ArraySegment<byte>( totalBytes, messageOffset, messageLength );
            return byteArray;
        }
        public override void WriteMessage( Message message, Stream stream )
        {
            XmlWriter writer = XmlWriter.Create( stream, _writerSettings );
            message.WriteMessage( writer );
            writer.Close();
        }

        #endregion
    }
}
