using log4net;
using System;
using System.IO;
using System.Reflection;
using log4net.Config;
using System.Collections.Generic;

namespace Laplace.Framework.Log
{
    /// <summary>
    /// Provides methods for logging.
    /// </summary>
    public static class Logger
    {
        private static bool isInitialized;

        static Logger()
        {
            isInitialized = false;
        }

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        /// <exception cref="LoggingInitializationException">Thrown if logger is already initialized.</exception>
        public static void Initialize()
        {
            Initialize(null);
        }

        /// <summary>
        /// Initializes the logger to use a specific config file.
        /// </summary>
        /// <param name="configFile">The path of the config file.</param>
        /// <exception cref="LoggingInitializationException">Thrown if logger is already initialized.</exception>
        public static void Initialize(string configFile)
        {
            if (!isInitialized)
            {
                if (!String.IsNullOrEmpty(configFile))
                    XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
                else
                    XmlConfigurator.Configure();
                isInitialized = true;
            }
            else
                throw new LoggingInitializationException("Logging has already been initialized.");
        }
        /// <summary>
        /// 特别指定的Logger名称
        /// </summary>
       // public static List<string> _listSpecifiedLoggerName = new List<string>();

        //public static void AddSpecifiedLoggerName(string name)
        //{
        //    if (!_listSpecifiedLoggerName.Contains(name))
        //    {
        //        _listSpecifiedLoggerName.Add(name);
        //    }
        //}
        /// <summary>
        /// Logs an entry to all logs.
        /// </summary>
        /// <param name="loggingLevel">The logging level.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="LoggingInitializationException">Thrown if logger has not been initialized.</exception>
        public static void Log(LoggingLevel loggingLevel, string message)
        {
            Log(loggingLevel, message, null, null);
        }
        public static void LogDebug(string message)
        {
            Log(LoggingLevel.Debug, message, null, null);
        }


        public static void LogInfo(string message)
        {
            Log(LoggingLevel.Info, message, null, null);
        }
        public static void LogWarning(string message)
        {
            Log(LoggingLevel.Warning, message, null, null);
        }
        public static void LogError(string message)
        {
            Log(LoggingLevel.Error, message, null, null);
        }
        public static void LogFatal(string message)
        {
            Log(LoggingLevel.Fatal, message, null, null);
        }
        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="ex"></param>
        public static void LogError4Exception(Exception ex)
        {
            var message = GetExceptionMessage(ex);
            Log(LoggingLevel.Fatal, message, null, null);
        }
        public static string GetExceptionMessage(Exception ex)
        {
            return string.Format("StackTrace:{0}{1}InnerException:{2}{3}Message:{4}"
                                    , ex.StackTrace, Environment.NewLine
                                    , ex.InnerException, Environment.NewLine
                                    , ex.Message);
        }
        public static void LogDebug(string message, string logName)
        {
            Log(logName, LoggingLevel.Debug, message, null);
        }

        public static void LogDebugWithAppLogger(string message, string logName)
        {
            LogDebug(message,new string[] { "AppLogger",logName});
        }
        public static void LogDebug(string message, string[] logNames)
        {
            foreach (var logName in logNames)
            {
                Log(logName, LoggingLevel.Debug, message, null);
            }
        }
        public static void LogInfo(string message,string logName)
        {
            Log(logName, LoggingLevel.Info, message, null);
        }
        public static void LogInfo(string message, string[] logNames)
        {
            foreach (var logName in logNames)
            {
                Log(logName, LoggingLevel.Info, message, null);
            }
        }
        public static void LogWarning(string message, string logName)
        {
            Log(logName, LoggingLevel.Warning, message, null);
        }
        public static void LogWarning(string message, string[] logNames)
        {
            foreach (var logName in logNames)
            {
                Log(logName, LoggingLevel.Warning, message, null);
            }
        }
        public static void LogError(string message, string logName)
        {
            Log(logName, LoggingLevel.Error, message, null);
        }
        public static void LogError(string message, string[] logNames)
        {
            foreach (var logName in logNames)
            {
                Log(logName, LoggingLevel.Error, message, null);
            }
        }
        public static void LogFatal(string message, string logName)
        {
            Log(logName, LoggingLevel.Fatal, message, null);
        }
        public static void LogFatal(string message, string[] logNames)
        {
            foreach (var logName in logNames)
            {
                Log(logName, LoggingLevel.Fatal, message, null);
            }
        }
        public static void LogError4Exception(Exception ex, string logName)
        {
            var message = GetExceptionMessage(ex);
            Log(logName,LoggingLevel.Fatal, message, null, null);
        }
        public static void LogError4Exception(Exception ex, string[] logNames)
        {
            var message = GetExceptionMessage(ex);
            foreach (var logName in logNames)
            {
                Log(logName, LoggingLevel.Fatal, message, null, null);
            }
        }
        /// <summary>
        /// Logs an entry to all logs.
        /// </summary>
        /// <param name="loggingLevel">The logging level.</param>
        /// <param name="message">The message.</param>
        /// <param name="loggingProperties">Any additional properties for the log as defined in the logging configuration.</param>
        /// <exception cref="LoggingInitializationException">Thrown if logger has not been initialized.</exception>
        public static void Log(LoggingLevel loggingLevel, string message, object loggingProperties)
        {
            Log(loggingLevel, message, loggingProperties, null);
        }

        /// <summary>
        /// Logs an entry to all logs.
        /// </summary>
        /// <param name="loggingLevel">The logging level.</param>
        /// <param name="message">The message.</param>
        /// <param name="loggingProperties">Any additional properties for the log as defined in the logging configuration.</param>
        /// <param name="exception">Any exception to be logged.</param>
        public static void Log(LoggingLevel loggingLevel, string message, object loggingProperties, Exception exception)
        {
            ILog appLogger = LogManager.GetLogger("AppLogger");
            if (appLogger !=null)
            {
                LogBase(appLogger, loggingLevel, message, loggingProperties, exception);
                return;
            }
            foreach (ILog log in LogManager.GetCurrentLoggers())
            {
                //if (!_listSpecifiedLoggerName.Contains(log.Logger.Name))
                {
                    LogBase(log, loggingLevel, message, loggingProperties, exception);
                }
            }
        }

        /// <summary>
        /// Logs an entry to the specified log.
        /// </summary>
        /// <param name="logName">The name of the log.</param>
        /// <param name="loggingLevel">The logging level.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="InvalidLogException">Thrown if <paramref name="logName"/> does not exist.</exception>
        public static void Log(string logName, LoggingLevel loggingLevel, string message)
        {
            Log(logName, loggingLevel, message, null, null);
        }

        /// <summary>
        /// Logs an entry to the specified log.
        /// </summary>
        /// <param name="logName">The name of the log.</param>
        /// <param name="loggingLevel">The logging level.</param>
        /// <param name="message">The message.</param>
        /// <param name="loggingProperties">Any additional properties for the log as defined in the logging configuration.</param>
        /// <exception cref="InvalidLogException">Thrown if <paramref name="logName"/> does not exist.</exception>
        public static void Log(string logName, LoggingLevel loggingLevel, string message, object loggingProperties)
        {
            Log(logName, loggingLevel, message, loggingProperties, null);
        }

        /// <summary>
        /// Logs an entry to the specified log.
        /// </summary>
        /// <param name="logName">The name of the log.</param>
        /// <param name="loggingLevel">The logging level.</param>
        /// <param name="message">The message.</param>
        /// <param name="loggingProperties">Any additional properties for the log as defined in the logging configuration.</param>
        /// <param name="exception">Any exception to be logged.</param>
        /// <exception cref="InvalidLogException">Thrown if <paramref name="logName"/> does not exist.</exception>
        public static void Log(string logName, LoggingLevel loggingLevel, string message, object loggingProperties, Exception exception)
        {
            foreach (ILog log in LogManager.GetCurrentLoggers())
            {
                if (logName ==log.Logger.Name)
                {
                    LogBase(log, loggingLevel, message, loggingProperties, exception);
                    return;
                }
            }
            //ILog log = LogManager.GetLogger(logName);
            //if (log != null)
            //    LogBase(log, loggingLevel, message, loggingProperties, exception);
            //else
            //    throw new InvalidLogException("The log \"" + logName + "\" does not exist or is invalid.", logName);
        }

        private static void LogBase(ILog log, LoggingLevel loggingLevel, string message, object loggingProperties, Exception exception)
        {
            if (ShouldLog(log, loggingLevel))
            {
                PushLoggingProperties(loggingProperties);
                switch (loggingLevel)
                {
                    case LoggingLevel.Debug: log.Debug(message, exception); break;
                    case LoggingLevel.Info: log.Info(message, exception); break;
                    case LoggingLevel.Warning: log.Warn(message, exception); break;
                    case LoggingLevel.Error: log.Error(message, exception); break;
                    case LoggingLevel.Fatal: log.Fatal(message, exception); break;
                }
                PopLoggingProperties(loggingProperties);
            }
        }

        private static void PushLoggingProperties(object loggingProperties)
        {
            if (loggingProperties != null)
            {
                Type attrType = loggingProperties.GetType();
                PropertyInfo[] properties = attrType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < properties.Length; i++)
                {
                    object value = properties[i].GetValue(loggingProperties, null);
                    if (value != null)
                        ThreadContext.Stacks[properties[i].Name].Push(value.ToString());
                }
            }
        }

        private static void PopLoggingProperties(object loggingProperties)
        {
            if (loggingProperties != null)
            {
                Type attrType = loggingProperties.GetType();
                PropertyInfo[] properties = attrType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int i = properties.Length - 1; i >= 0; i--)
                {
                    object value = properties[i].GetValue(loggingProperties, null);
                    if (value != null)
                        ThreadContext.Stacks[properties[i].Name].Pop();
                }
            }
        }

        private static bool ShouldLog(ILog log, LoggingLevel loggingLevel)
        {
            switch (loggingLevel)
            {
                case LoggingLevel.Debug: return log.IsDebugEnabled;
                case LoggingLevel.Info: return log.IsInfoEnabled;
                case LoggingLevel.Warning: return log.IsWarnEnabled;
                case LoggingLevel.Error: return log.IsErrorEnabled;
                case LoggingLevel.Fatal: return log.IsFatalEnabled;
                default: return false;
            }
        }
    }
}
