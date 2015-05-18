using System;
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
        static string cfgPath = null;
        static string gameDir = null;
        static string keyValue = null;
        static FileStream fs;
        static StreamReader sReader;
        static string prevText = "";

        [DllImport(@"C:\KeypressLibrary.dll",CallingConvention = CallingConvention.Cdecl)]
        private static extern void Presskey(ushort key);

        [DllImport(@"C:\KeypressLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Releasekey(ushort key);

        static void Main(string[] args)
        {
            ReadConfig();
            UpdateAutoexec();
            ClassCompiler.CompileExtensionMethod(args[0]);

            fs = new FileStream(gameDir + "out.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                        ClassCompiler.parserExtensionClass.Parse(text);
                    }
                    prevText = text;
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        static void ReadConfig()
        {
            if (!File.Exists("config.cfg"))
            {
                logger.ClearConsole();
                Console.Write("Please enter path to Source game directory:  ");
                gameDir = Console.ReadLine();
                logger.WriteLine("", Logger.LogLevel.Nothing);
                logger.Write("Please enter path to cfg folder:  ", Logger.LogLevel.Info);
                cfgPath = Console.ReadLine();
                logger.WriteLine("", Logger.LogLevel.Nothing);
                logger.Write("Please enter a Source engine Key value for executing commands:  ",Logger.LogLevel.Info);
                keyValue = Console.ReadLine();
                Stream stream = File.Create("config.cfg");
                StreamWriter sw = new StreamWriter(stream);
                sw.WriteLine(gameDir);
                sw.WriteLine(cfgPath);
                sw.WriteLine(keyValue);
                sw.Close();
                stream.Close();
            }
            else
            {
                StreamReader sr = new StreamReader("config.cfg");
                gameDir = sr.ReadLine();
                cfgPath = sr.ReadLine();
                keyValue = sr.ReadLine();
                sr.Close();
            }
        }

        public static void ExecuteIngame(String[] commands)
        {
            //Write commands to config file
            File.WriteAllLines(cfgPath + "executer.cfg", commands);
            //Press bind to execute config
            SendKeyPress();
        }

        public static void ExecuteIngame(String command)
        {
            //Write commands to config file
            File.WriteAllText(cfgPath + "executer.cfg", command);
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
            string autoexec = File.ReadAllText(cfgPath + "autoexec.cfg");
            StreamWriter sw = new StreamWriter(cfgPath + "autoexec.cfg", true);
            //Ensure logging is enabled
            if (!autoexec.Contains("con_logfile out.log"))
            {
                sw.WriteLine();
                sw.WriteLine("con_logfile out.log");
            }
            else if (!autoexec.Contains("con_timestamp 1"))
            {
                sw.WriteLine();
                sw.WriteLine("con_timestamp 1");
            }
            //Ensure bind is set for executing
            else if (!autoexec.Contains("bind " + keyValue + " \"exec executer.cfg\""))
            {
                sw.WriteLine();
                sw.WriteLine("bind " + keyValue + " \"exec executer.cfg\"");
            }
            //Dispose of stream
            sw.Close();
        }
    }
}
