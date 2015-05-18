using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceEngineConsoleParser
{
    public class Logger
    {
        private ConsoleColor DefaultColor = ConsoleColor.White;
        public enum LogLevel
        {
            Debug,
            Info,
            Success,
            Warn,
            Error,
            Interface, 
            Nothing
        }

        public void WriteLine(String Message, LogLevel logLevel)
        {
            Console.ForegroundColor = LogColor(LogLevel.Info);
            Console.Write(">  ");
            Console.ForegroundColor = LogColor(logLevel);
            Console.WriteLine(Message);
            Console.ForegroundColor = DefaultColor;
        }

        public void Write(String Message, LogLevel logLevel)
        {
            Console.ForegroundColor = LogColor(LogLevel.Info);
            Console.Write(">  ");
            Console.ForegroundColor = LogColor(logLevel);
            Console.Write(Message);
            Console.ForegroundColor = DefaultColor;
        }

        public void ClearConsole()
        {
            Console.Clear();
        }
        private ConsoleColor LogColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                case LogLevel.Debug:
                    return ConsoleColor.Gray;
                case LogLevel.Success:
                    return ConsoleColor.Green;
                case LogLevel.Warn:
                    return ConsoleColor.Yellow;
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Interface:
                    return ConsoleColor.DarkCyan;
                default:
                    return DefaultColor;
            }
        }
    }
}
