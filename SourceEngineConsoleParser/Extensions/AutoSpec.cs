using System;

namespace SourceEngineConsoleParser
{
    //Auto spectate extension for console parser

    public class AutoSpec : Parser
    {
        bool isEnabled = false;

        public void Load()
        {
		Program.logger.WriteLine("Loaded Auto Spectate extension", Logger.LogLevel.Success);
        }

        public void Parse(string text)
        {
            if (text.Contains("parser_autospec_enabled"))
            {
                isEnabled = true;
                Program.logger.WriteLine("Auto-spectate Enabled", Logger.LogLevel.Warn);
                Program.ExecuteIngame("echo Parser > Auto-spectate enabled");
                return;
            }
            else if (text.Contains("parser_autospec_disabled"))
            {
                isEnabled = false;
                Program.logger.WriteLine("Auto-spectate Disabled", Logger.LogLevel.Warn);
                Program.ExecuteIngame("echo Parser > Auto-spectate disabled");
                return;
            }
            else if (isEnabled && text.ToLowerInvariant().Contains("spec") && !text.Contains("Auto-spectate"))
            {
                Program.ExecuteIngame("spectate");
                Program.logger.WriteLine("Going spectate", Logger.LogLevel.Info);
            }
        }
    }
}