using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManyConsole;

namespace OmnitureConsole
{
    public class GetCalculatedMetricsCommand : ConsoleCommand
    {
        public GetCalculatedMetricsCommand()
        {
            this.IsCommand("get-metrics", "Lists calculated metrics.");
        }

        public override int Run(string[] remainingArguments)
        {

            throw new NotImplementedException();
        }
    }
}
