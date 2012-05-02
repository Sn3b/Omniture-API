namespace Omniture.CustomToken
{
    /// <summary>
    /// This class provides the stored password for a given username
    /// </summary>
    internal class PasswordProvider
    {
        #region Internal Methods

        internal string RetrievePassword( string username )
        {
            if ( username == "User1" )
                return "P@ssw0rd";

            return null;
        }

        #endregion
    }
}
