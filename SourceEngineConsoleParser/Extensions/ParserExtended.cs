using System;

namespace SourceEngineConsoleParser
{
    //This class is compiled at runtime to allow easy additions / changes to be made without the need to recompile the main program
    public class ParserExtended : Parser
    {
  	public void Load()
        {
		Program.logger.WriteLine("Loaded example extension", Logger.LogLevel.Success);
        }

        public void Parse(string text)
        {
            //Change logic below
            Console.WriteLine("Extended Parse: " + text);
        }
    }
}