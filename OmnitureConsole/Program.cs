using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmnitureConsole
{
    public class Program
    {
        static int Main(string[] args)
        {
            var commands = ManyConsole.ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof (Program));
            return ManyConsole.ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}
