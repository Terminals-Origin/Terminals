using System;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

namespace Terminals
{
    /// <summary>
    /// 1:1 adapter of logging provider
    /// </summary>
    internal class Logging
    {
        private static readonly ILog log = LogManager.GetLogger("Terminals");

        internal static string LogDirectory
        {
            get
            {
                // different configuration can be defined by setup or by user.
                FileAppender rootAppender = FindLogAppender();
                if (rootAppender != null)
                    return rootAppender.File;

                return String.Empty;
            }
        }

        private static FileAppender FindLogAppender()
        {
            return ((Hierarchy)LogManager.GetRepository()).Root.Appenders
                                                          .OfType<FileAppender>()
                                                          .FirstOrDefault();
        }

        internal static void Fatal(string message)
        {
            log.Fatal(message);
        }

        internal static void Fatal(object message)
        {
            log.Fatal(message);
        }

        internal static void FatalFormat(string format, string message)
        {
            log.FatalFormat(format, message);
        }

        internal static void Fatal(string message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        internal static void Error(string message)
        {
            log.Error(message);
        }

        internal static void Error(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        internal static void Warn(string message)
        {
            log.Warn(message);
        }

        internal static void Info(string message)
        {
            log.Info(message);
        }

        internal static void Info(Exception exception)
        {
            log.Info(exception);
        }

        internal static void Info(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        public static void InfoFormat(string format, string message)
        {
            log.InfoFormat(format, message);
        }

        public static void DebugFormat(string format, string message)
        {
            log.DebugFormat(format, message);
        }
    }
}
