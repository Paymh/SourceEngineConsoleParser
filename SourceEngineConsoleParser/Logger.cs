using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceEngineConsoleParser
{
    /// <summary>
    /// Logs information to the console with colour coding
    /// </summary>
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

        /// <summary>
        /// Writes a line to the console in the color corresponding to the LogLevel
        /// </summary>
        /// <param name="Message">The message to log to the console</param>
        /// <param name="logLevel">The LogLevel of the message</param>
        public void WriteLine(String Message, LogLevel logLevel)
        {
            Console.ForegroundColor = LogColor(LogLevel.Info);
            Console.Write(">  ");
            Console.ForegroundColor = LogColor(logLevel);
            Console.WriteLine(Message);
            Console.ForegroundColor = DefaultColor;
        }

        /// <summary>
        /// Writes a message to the console in the color corresponding to the LogLevel without a newline after the message
        /// </summary>
        /// <param name="Message">The message to log to the console</param>
        /// <param name="logLevel">The LogLevel of the messages</param>
        public void Write(String Message, LogLevel logLevel)
        {
            Console.ForegroundColor = LogColor(LogLevel.Info);
            Console.Write(">  ");
            Console.ForegroundColor = LogColor(logLevel);
            Console.Write(Message);
            Console.ForegroundColor = DefaultColor;
        }

        /// <summary>
        /// Clears the console
        /// </summary>
        public void ClearConsole()
        {
            Console.Clear();
        }

        /// <summary>
        /// Returns the colour of the console corresponding to the LogLevel
        /// </summary>
        /// <param name="level">The LogLevel to get the colour of</param>
        /// <returns>The colour of the console corresponding to the LogLevel</returns>
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
