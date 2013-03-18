using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManyConsole;

namespace OmnitureConsole
{
    public class SavedApiCredentials
    {
        public string Username;
        public string Secret;

        public void ApplyTo(ConsoleCommand command)
        {
            command.HasRequiredOption("u=", "Username", v => Username = v);
            command.HasRequiredOption("s=", "Secret key", v => Secret = v);
        }

        public override string ToString()
        {
            return Username ?? "<null>";
        }
    }
}
