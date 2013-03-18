using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ManyConsole;
using OmnitureConsole.CommandParameters;

namespace OmnitureConsole
{
    public class SaveCredentialsCommand : ConsoleCommand
    {
        public ApiCredentialParameters Credentials;

        public SaveCredentialsCommand()
        {
            this.IsCommand("save-credentials", "Saves omniture API secret to file for later reuse.");

            Credentials = new ApiCredentialParameters();
            Credentials.ApplyWithoutCheckingFile(this);
        }

        public override int Run(string[] remainingArguments)
        {
            var filename = Path.Combine(".", ApiCredentialParameters.FilenameForOmnitureSecret);

            Console.WriteLine("Writing secret to file {0} -- be careful not to check into source control.", filename);

            File.WriteAllLines(filename, new [] {Credentials.Username, Credentials.Secret}, Encoding.UTF8);

            return 0;
        }
    }
}
