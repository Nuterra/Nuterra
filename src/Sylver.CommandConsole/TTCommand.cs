using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sylver.CommandConsole
{
    public abstract class TTCommand
    {
        public abstract string Call(Dictionary<string, string> arguments);

        public abstract void Init();

        //public Dictionary<string, string> argumentsDescriptions = new Dictionary<string, string>();

        public abstract Dictionary<string, string> ArgumentsDescriptions { get; }

        public abstract string Name { get; }

        public abstract string Description { get; }
    }
}
