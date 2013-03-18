using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ManyConsole;

namespace OmnitureConsole
{
    public class SavedApiCredentials
    {
        public string Username;
        public string Secret;

        public SavedApiCredentials()
        {
            var path = Path.GetFullPath(".");
            var root = Path.GetPathRoot(path);

            while (path != root)
            {
                Console.Write("trying " + path);
                var credentialsPath = Path.Combine(path, "omniture-api-secret.txt");
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

        public void ApplyTo(ConsoleCommand command)
        {
            if (string.IsNullOrEmpty(Username))
            {
                command.HasRequiredOption("u=", "Username", v => Username = v);
                command.HasRequiredOption("s=", "Secret key", v => Secret = v);
            }
            else
            {
                command.HasOption("u=", string.Format("Username (default is {0})", Username), v => Username = v);
                command.HasOption("s=", string.Format("Secret key (default is for {0})", Username), v => Secret = v);
            }
        }

        public override string ToString()
        {
            return Username ?? "<null>";
        }
    }
}
