using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace Omniture.Channels
{
    internal class CustomTextMessageBindingElement : MessageEncodingBindingElement, IWsdlExportExtension
    {
        #region Fields

        private MessageVersion _msgVersion;
        private string _mediaType;
        private string _encoding;
        private readonly XmlDictionaryReaderQuotas _readerQuotas;

        #endregion

        #region Properties

        public override MessageVersion MessageVersion
        {
            get { return _msgVersion; }
            set
            {
                if ( value == null )
                    throw new ArgumentNullException( "value" );

                _msgVersion = value;
            }
        }

        public string MediaType
        {
            get { return _mediaType; }
            set
            {
                if ( value == null )
                    throw new ArgumentNullException( "value" );

                _mediaType = value;
            }
        }

        public string Encoding
        {
            get { return _encoding; }

            set
            {
                if ( value == null )
                    throw new ArgumentNullException( "value" );
                _encoding = value;
            }
        }

        // This encoder does not enforces any quotas for the unsecure messages. The 
        // quotas are enforced for the secure portions of messages when this encoder
        // is used in a binding that is configured with security. 
        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get { return _readerQuotas; }
        }

        #endregion

        #region Constructors

        private CustomTextMessageBindingElement( CustomTextMessageBindingElement binding )
            : this( binding.Encoding, binding.MediaType, binding.MessageVersion )
        {
            _readerQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.CopyTo( _readerQuotas );
        }

        public CustomTextMessageBindingElement( string encoding, string mediaType,
                                                MessageVersion msgVersion )
        {
            if ( encoding == null )
                throw new ArgumentNullException( "encoding" );

            if ( mediaType == null )
                throw new ArgumentNullException( "mediaType" );

            if ( msgVersion == null )
                throw new ArgumentNullException( "msgVersion" );

            _msgVersion = msgVersion;
            _mediaType = mediaType;
            _encoding = encoding;
            _readerQuotas = new XmlDictionaryReaderQuotas();
        }

        public CustomTextMessageBindingElement( string encoding, string mediaType )
            : this( encoding, mediaType, MessageVersion.Soap11WSAddressing10 )
        {
        }

        public CustomTextMessageBindingElement( string encoding )
            : this( encoding, "text/xml" )
        {
        }

        public CustomTextMessageBindingElement()
            : this( "UTF-8" )
        {
        }

        #endregion

        #region Overrides

        public override BindingElement Clone()
        {
            return new CustomTextMessageBindingElement( this );
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>( BindingContext context )
        {
            if ( context == null )
                throw new ArgumentNullException( "context" );

            context.BindingParameters.Add( this );
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override bool CanBuildChannelFactory<TChannel>( BindingContext context )
        {
            if ( context == null )
                throw new ArgumentNullException( "context" );

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>( BindingContext context )
        {
            if ( context == null )
                throw new ArgumentNullException( "context" );

            context.BindingParameters.Add( this );
            return context.BuildInnerChannelListener<TChannel>();
        }

        public override bool CanBuildChannelListener<TChannel>( BindingContext context )
        {
            if ( context == null )
                throw new ArgumentNullException( "context" );

            context.BindingParameters.Add( this );
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public override T GetProperty<T>( BindingContext context )
        {
            if ( typeof ( T ) == typeof ( XmlDictionaryReaderQuotas ) )
                return ( T ) ( object ) _readerQuotas;

            return base.GetProperty<T>( context );
        }

        #endregion

        #region IMessageEncodingBindingElement Members

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new CustomTextMessageEncoderFactory( MediaType, Encoding, MessageVersion );
        }

        #endregion

        #region IWsdlExportExtension Members

        void IWsdlExportExtension.ExportContract( WsdlExporter exporter, WsdlContractConversionContext context )
        { }

        void IWsdlExportExtension.ExportEndpoint( WsdlExporter exporter, WsdlEndpointConversionContext context )
        {
            // The MessageEncodingBindingElement is responsible for ensuring that the WSDL has the correct
            // SOAP version. We can delegate to the WCF implementation of TextMessageEncodingBindingElement for this.
            var mebe = new TextMessageEncodingBindingElement { MessageVersion = _msgVersion };
            ( ( IWsdlExportExtension ) mebe ).ExportEndpoint( exporter, context );
        }

        #endregion
    }
}
