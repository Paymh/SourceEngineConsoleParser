using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceEngineConsoleParser
{
        public interface Parser
        {
        void Parse(String text);
        void Load();
        }
}
