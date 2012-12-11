using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using SqlScriptRunner.Logging;

namespace BuildTasks
{
    public class MSBuildLogger : ILog
    {
        public IBuildEngine BuildEngine { get; set; }
        public MSBuildLogger(IBuildEngine Engine)
        {
            this.BuildEngine = Engine;
        }
        public void Debug(object message)
        {
            this.BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message.ToString(), "","Debug", MessageImportance.Normal));
        }

        public void Debug(object message, Exception exception)
        {
            this.BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message.ToString() + "\n" + exception.ToString(), exception.HelpLink, exception.Source, MessageImportance.Normal));
        }

        public void Error(object message)
        {
            this.BuildEngine.LogErrorEvent(new BuildErrorEventArgs("ERROR", "", "", 0, 0, 0, 0, message.ToString(), "", ""));
        }

        public void Error(object message, Exception exception)
        {
            this.BuildEngine.LogErrorEvent(new BuildErrorEventArgs("ERROR", exception.ToString(), "", 0, 0, 0, 0, message.ToString(), exception.HelpLink, ""));
        }

        public void Fatal(object message)
        {
            this.BuildEngine.LogErrorEvent(new BuildErrorEventArgs("FATAL", "", "", 0, 0, 0, 0, message.ToString(), "", ""));
        }

        public void Fatal(object message, Exception exception)
        {
            this.BuildEngine.LogErrorEvent(new BuildErrorEventArgs("FATAL", exception.ToString(), "", 0,0,0,0, message.ToString(), exception.HelpLink, ""));
        }

        public void Info(object message)
        {
            this.BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message.ToString(), "", "Info", MessageImportance.Normal));
        }

        public void Info(object message, Exception exception)
        {
            this.BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message.ToString() + "\n" + exception.ToString(), exception.HelpLink, exception.Source, MessageImportance.Normal));
        }

        public void Warn(object message)
        {
            this.BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message.ToString(), "", "Warning", MessageImportance.Normal));
        }

        public void Warn(object message, Exception exception)
        {
            this.BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message.ToString() + "\n" + exception.ToString(), exception.HelpLink, exception.Source, MessageImportance.Normal));
        }

        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }
    }
}
