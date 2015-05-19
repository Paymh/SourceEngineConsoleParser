using System;
using System.IO;

namespace SourceEngineConsoleParser
{
    public class HudSwitcher : Parser
    {
        public void Load()
        {
            Program.logger.WriteLine("Loaded Hud Switcher extension", Logger.LogLevel.Success);
        }
        public void Parse(String text)
        {
            if (text.Contains("parser_hudswitcher_changehud"))
            {
                string folder = text.Split(' ')[4];
                ChangeHud(folder);
            }
            else if (text.Contains("Shutdown function ShutdownMixerControls() not in list!!!"))
            {
                //On game close restore hud to original
                ChangeHud(@"\hud_default\");
            }
        }

        public void ChangeHud(String path)
        {
            Program.logger.WriteLine("Attempting to change hud to: " + path, Logger.LogLevel.Warn);
            //Path is relative to running directory
            if (Directory.Exists(Directory.GetCurrentDirectory() + path))
            {
                Program.logger.WriteLine("Changing hud to: " + path, Logger.LogLevel.Warn);
                //Delete all scripts files to make way for new hud
                foreach (String file in Directory.GetFiles(Program.customPath + @"scripts\"))
                {
                    File.Delete(file);
                }
                //Copy over new script files
                foreach (String file in Directory.GetFiles(Directory.GetCurrentDirectory() + path + @"scripts\"))
                {
                    File.Copy(file, Program.customPath + @"scripts\" + Path.GetFileName(file), true);
                }
                //Delete all resource files to make way for new hud
                //Ignore files in main resource directory because they require a restart
                foreach (var file in Directory.GetFiles(Program.customPath + @"resource\ui\", "*", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }
                //Copy over new resource files, ignore main directory again
                foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory() + path + @"resource\ui\", "*", SearchOption.AllDirectories))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(Program.customPath + @"resource\ui\" + Path.GetFullPath(file).Replace(Directory.GetCurrentDirectory() + path + @"resource\ui\", ""))))
                        Directory.CreateDirectory((Path.GetDirectoryName(Program.customPath + @"resource\ui\" + Path.GetFullPath(file).Replace(Directory.GetCurrentDirectory() + path + @"resource\ui\", ""))));
                    File.Copy(file, Program.customPath + @"resource\ui\" + Path.GetFullPath(file).Replace(Directory.GetCurrentDirectory() + path + @"resource\ui\", ""), true);
                }
                Program.ExecuteIngame("echo Parser > Changed hud to " + path);
            }
            else
            {
                Program.logger.WriteLine("Hud not found!", Logger.LogLevel.Error);
            }
        }
    }
}