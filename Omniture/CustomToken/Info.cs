namespace Omniture.CustomToken
{
    internal class Info
    {
        #region Fields

        private readonly string _userName;
        private readonly string _password;

        #endregion

        #region Properties

        internal string Username { get { return _userName; } }
        internal string Password { get { return _password; } }

        #endregion

        #region Constructors

        internal Info( string userName, string password )
        {
            _userName = userName;
            _password = password;
        }

        #endregion
    }
}
