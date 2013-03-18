using System.IO;
using System.Linq;
using ManyConsole;

namespace OmnitureConsole.CommandParameters
{
    public class ApiCredentialParameters
    {
        public const string FilenameForOmnitureSecret = "omniture-api-secret.txt";

        public string Username;
        public string Secret;

        public void ApplyTo(ConsoleCommand command)
        {
            FindAndLoadCredentialsIfAvailable();

            if (string.IsNullOrEmpty(Username))
            {
                ApplyWithoutCheckingFile(command);
            }
            else
            {
                command.HasOption("u=", string.Format("Username (default is {0})", Username), v => Username = v);
                command.HasOption("s=", string.Format("Secret key (default is for {0})", Username), v => Secret = v);
            }
        }

        public void ApplyWithoutCheckingFile(ConsoleCommand command)
        {
            command.HasRequiredOption("u=", "Username", v => Username = v);
            command.HasRequiredOption("s=", "Secret key", v => Secret = v);
        }

        private void FindAndLoadCredentialsIfAvailable()
        {
            var path = Path.GetFullPath(".");
            var root = Path.GetPathRoot(path);

            while (path != root)
            {
                var credentialsPath = Path.Combine(path, FilenameForOmnitureSecret);
                if (File.Exists(credentialsPath))
                {
                    var lines = File.ReadAllLines(credentialsPath).Where(l => !string.IsNullOrEmpty(l)).ToArray();

                    Username = lines[0];
                    Secret = lines[1];
                    break;
                }
                else
                {
                    path = new DirectoryInfo(path).Parent.FullName;
                }
            }
        }

        public override string ToString()
        {
            return Username ?? "<null>";
        }
    }
}
