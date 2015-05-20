﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace SourceEngineConsoleParser
{
    public static class Program
    {
        public static Logger logger = new Logger();
        public static string gameDir = null;
        public static string keyValue = null;
        public static string customPath = null;
        static FileStream fs;
        static StreamReader sReader;
        static string prevText = "";
        static List<Parser> parserExtensions = new List<Parser>();

        [DllImport("KeyPressLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Presskey(ushort key);

        [DllImport("KeypressLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Releasekey(ushort key);

        static void Main(string[] args)
        {
            ReadConfig();
            UpdateAutoexec();
            Program.logger.WriteLine("Running Source Console Parser", Logger.LogLevel.Info);
            if (args.Length > 0)
            {
                foreach (String arg in args)
                {
                    parserExtensions.Add(ClassCompiler.CompileExtensionMethod(arg));
                }
            }
            foreach (Parser parser in parserExtensions)
            {
                parser.Load();
            }
            ReadConsole();
        }

        static void ReadConsole()
        {
            fs = new FileStream(gameDir + "out.log", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

            sReader = new StreamReader(fs);
            bool readAll = true;
            while (true)
            {
                if (readAll)
                {
                    readAll = false;
                    fs.Seek(0, SeekOrigin.End);
                }
                String s;
                while (!String.IsNullOrWhiteSpace(s = sReader.ReadLine()))
                {
                    String text = s;
                    if (text != prevText)
                    {
                        foreach (Parser extension in parserExtensions)
                        {
                            extension.Parse(text);
                        }
                        if (parserExtensions.Count == 0)
                            logger.WriteLine(text, Logger.LogLevel.Info);
                    }
                    prevText = text;
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        static void ReadConfig()
        {
            bool runSetup = false;
            if (!File.Exists("config.cfg"))
                runSetup = true;
            else {
                var cfgLines = System.IO.File.ReadAllLines("config.cfg").Take(3);
                if (cfgLines.Contains("") || cfgLines.Count() == 0)
                    runSetup = true;
            }

            if (runSetup)
            {
                logger.ClearConsole();
                Console.Write("Please enter path to Source game directory (eg C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Team Fortress 2\\) :  ");
                gameDir = Console.ReadLine();
                if (gameDir.Length > 0 && gameDir[gameDir.Length - 1] != '\\' && gameDir[gameDir.Length - 1] != '/')
                    gameDir += "\\";
                logger.WriteLine("", Logger.LogLevel.Nothing);
                logger.Write("Please enter the subdirectory to the custom folder containing your autoexec. (eg tf\\custom\\customstuff\\) :  ", Logger.LogLevel.Info);
                customPath = Console.ReadLine();
                if (customPath.Length > 0 && customPath[customPath.Length - 1] != '\\' && customPath[customPath.Length - 1] != '/')
                    customPath += "\\";
                // Attempt to handle people putting a slash at the beginning of their custom folder and the end of their game directory
                // This could probably be improved quite a bit
                if (customPath.Length > 0 && gameDir.Length > 0 && gameDir[gameDir.Length - 1] == customPath[0]) {
                    customPath = customPath.Substring(1);
                }
                logger.WriteLine("", Logger.LogLevel.Nothing);
                logger.Write("Please enter a key to use for executing commands (names for non-alphanumeric keys can be found on the wiki). Any existing bind for this key will be overwritten. :  ", Logger.LogLevel.Info);
                keyValue = Console.ReadLine();
                logger.WriteLine("", Logger.LogLevel.Nothing);
                Stream stream = File.Create("config.cfg");
                StreamWriter sw = new StreamWriter(stream);
                sw.WriteLine(gameDir);
                sw.WriteLine(customPath);
                sw.WriteLine(keyValue);
                sw.Close();
                stream.Close();
            }
            else
            {
                StreamReader sr = new StreamReader("config.cfg");
                gameDir = sr.ReadLine();
                customPath = sr.ReadLine();
                keyValue = sr.ReadLine();
                sr.Close();
                // Handle old format custom folder paths
                if (customPath.Contains(gameDir.Remove(gameDir.Length - 1))) {
                    Console.Write("Your config file is in the old format. Press any key to re-run setup.");
                    Console.ReadKey();
                    File.Delete("config.cfg");
                    ReadConfig();
                }
            }
        }

        public static void ExecuteIngame(String[] commands)
        {
            //Write commands to config file
            File.WriteAllLines(customPath + @"cfg\executer.cfg", commands);
            //Press bind to execute config
            SendKeyPress();
        }

        public static void ExecuteIngame(String command)
        {
            //Write commands to config file
            File.WriteAllText(customPath + @"cfg\executer.cfg", command);
            //Press bind to execute config
            SendKeyPress();
        }

        static void SendKeyPress()
        {
            
            Presskey((ushort)KeyUtilities.GetKeyCodeFromDescription(keyValue));
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            while (timer.ElapsedTicks < 6000) ;
            Releasekey((ushort)KeyUtilities.GetKeyCodeFromDescription(keyValue));
        }

        static void UpdateAutoexec()
        {
            String autoexec = "";
            StreamWriter sw = null;
            try {
                autoexec = File.ReadAllText(gameDir + customPath + @"cfg\autoexec.cfg");
                sw = new StreamWriter(gameDir + customPath + @"cfg\autoexec.cfg", true);
            }
            catch (System.IO.DirectoryNotFoundException) {
                Console.Write("Invalid path to custom directory:\n" + gameDir + customPath + "\nPress any key to exit.");
                File.Delete("config.cfg");
                Console.ReadKey();
                Environment.Exit(0);
            }
            catch (System.NotSupportedException) {
                Console.Write("Invalid path to custom directory:\n" + gameDir + customPath + "\nPress any key to exit.");
                File.Delete("config.cfg");
                Console.ReadKey();
                Environment.Exit(0);
            }
            //Ensure logging is enabled
            if (!autoexec.Contains("con_logfile out.log"))
            {
                sw.WriteLine();
                sw.WriteLine("con_logfile out.log");
            }
            if (!autoexec.Contains("con_timestamp 1"))
            {
                sw.WriteLine();
                sw.WriteLine("con_timestamp 1");
            }
            //Ensure bind is set for executing
            if (!autoexec.Contains("bind " + keyValue + " \"exec executer.cfg\""))
            {
                sw.WriteLine();
                sw.WriteLine("bind " + keyValue + " \"exec executer.cfg\"");
            }
            //Dispose of stream
            sw.Close();
        }
    }
}
