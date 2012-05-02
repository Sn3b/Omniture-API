using System.ServiceModel.Channels;

namespace Omniture.Channels
{
    internal class CustomTextMessageEncoderFactory : MessageEncoderFactory
    {
        #region Fields

        private readonly MessageEncoder _encoder;
        private readonly MessageVersion _version;
        private readonly string _mediaType;
        private readonly string _charSet;

        #endregion

        #region Properties

        public override MessageEncoder Encoder
        {
            get { return _encoder; }
        }
        public override MessageVersion MessageVersion
        {
            get { return _version; }
        }
        internal string MediaType
        {
            get { return _mediaType; }
        }
        internal string CharSet
        {
            get { return _charSet; }
        }

        #endregion

        #region Constructors

        internal CustomTextMessageEncoderFactory( string mediaType, string charSet, MessageVersion version )
        {
            _version = version;
            _mediaType = mediaType;
            _charSet = charSet;
            _encoder = new CustomTextMessageEncoder( this );
        }

        #endregion
    }
}
