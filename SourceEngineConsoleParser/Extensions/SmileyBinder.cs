using System;

namespace SourceEngineConsoleParser
{
    //This class is compiled at runtime to allow easy additions / changes to be made without the need to recompile the main program
    public class SmileyBinder : Parser
    {
        bool enabled = false;
        string playerName = "";

	public void Load()
        {
		Program.logger.WriteLine("Loaded Smiley Binder extension", Logger.LogLevel.Success);
        }

        public void Parse(string text)
        {
            //Change logic below
            if (text.Contains("parser_shittalk_enable"))
            {
                Program.logger.WriteLine("Smiley Binding enabled",Logger.LogLevel.Warn);
                enabled = true;
                Program.ExecuteIngame(new String[] { "name", "echo Parser > Shittalk enabled" });

            }
            else if (text.Contains("parser_shittalk_disable"))
            {
                Program.logger.WriteLine("Smiley Binding disabled", Logger.LogLevel.Warn);
                Program.ExecuteIngame("echo Parser > Shittalk disabled");
                enabled = false;
            }
            if (text.Contains("\"name\" = \""))
            {
                String[] temp = text.Split('\"');
                playerName = temp[3];
            }
            if (enabled && text.Contains(playerName + " killed"))
            {
                Program.ExecuteIngame("say =)");
                Program.logger.WriteLine("Smashed an idiot (Sent shittalk)",Logger.LogLevel.Success);
            }
        }
    }
}