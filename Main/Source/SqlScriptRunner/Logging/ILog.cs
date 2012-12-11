using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Logging
{
    /// <summary>
    /// A simple logging interface abstracting logging APIs. Inspired by log4net.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Debug(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Debug(object message, Exception exception);

        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Debug"/> level.
        //		/// </summary>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void DebugFormat(string format, params object[] args); 
        //
        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Debug"/> level.
        //		/// </summary>
        //		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void DebugFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Error(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Error(object message, Exception exception);

        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Error"/> level.
        //		/// </summary>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void ErrorFormat(string format, params object[] args); 
        //
        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Error"/> level.
        //		/// </summary>
        //		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Fatal(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Fatal(object message, Exception exception);

        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Fatal"/> level.
        //		/// </summary>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void FatalFormat(string format, params object[] args); 
        //
        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Fatal"/> level.
        //		/// </summary>
        //		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void FatalFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Info(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Info(object message, Exception exception);

        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Info"/> level.
        //		/// </summary>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void InfoFormat(string format, params object[] args); 
        //
        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Info"/> level.
        //		/// </summary>
        //		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void InfoFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Warn(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Warn(object message, Exception exception);

        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Warn"/> level.
        //		/// </summary>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void WarnFormat(string format, params object[] args); 
        //
        //		/// <summary>
        //		/// Logs a formatted message string with the <see cref="LogLevel.Warn"/> level.
        //		/// </summary>
        //		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
        //		/// <param name="format">A String containing zero or more format items</param>
        //		/// <param name="args">An Object array containing zero or more objects to format</param>
        //		void WarnFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        bool IsDebugEnabled
        {
            get;
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Error"/> level.
        /// </summary>
        bool IsErrorEnabled
        {
            get;
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        bool IsFatalEnabled
        {
            get;
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Info"/> level.
        /// </summary>
        bool IsInfoEnabled
        {
            get;
        }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        bool IsWarnEnabled
        {
            get;
        }
    }
}
