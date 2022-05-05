using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Logging
{
    public class Logger
    {
        private readonly ILog log4NetAdapter;

        public Logger(Type type)
        {
            this.log4NetAdapter = LogManager.GetLogger(type);
        }
 
        public void LogDebug(string message)
        {
            this.log4NetAdapter.Debug(message);
        }
 
        public void LogDebug(string message, Exception exception)
        {
            this.log4NetAdapter.Debug(message, exception);
        }
 
        public void LogError(string message)
        {
            this.log4NetAdapter.Error(message);
        }
 
        public void LogError(string message, Exception exception)
        {
            this.log4NetAdapter.Error(message, exception);
        }
 
        public void LogFatal(string message)
        {
            this.log4NetAdapter.Fatal(message);
        }
 
        public void LogFatal(string message, Exception exception)
        {
            this.log4NetAdapter.Fatal(message, exception);
        }
 
        public void LogInfo(string message)
        {
            this.log4NetAdapter.Info(message);
        }
 
        public void LogInfo(string message, Exception exception)
        {
            this.log4NetAdapter.Info(message, exception);
        }
 
        public void LogWarning(string message)
        {
            this.log4NetAdapter.Warn(message);
        }
 
        public void LogWarning(string message, Exception exception)
        {
            this.log4NetAdapter.Warn(message, exception);
        }

        public void Log(Exception exception)
        {
            this.log4NetAdapter.Error(exception.Message, exception);
        }
    }
}
