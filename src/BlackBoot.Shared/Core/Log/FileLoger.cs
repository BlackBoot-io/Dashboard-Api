using System;
using System.IO;
using System.Threading;

namespace BlackBoot.Shared.Core
{
    public class FileLoger
    {
        private static readonly object _infoLock = new object();
        private static readonly object _errorLock = new object();
        private static readonly object _messageLock = new object();

        public static void Info(string log, string path = "")
        {
            if (string.IsNullOrEmpty(path)) path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Log";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Monitor.Enter(_infoLock);
            try
            {
                using (var stream = File.AppendText($"{path}\\Info-{DateTime.Now.Date.ToString().Replace("/", "-")}.txt"))
                {
                    stream.WriteLine($"{DateTime.Now} :: {log}");
                    stream.Close();
                }
            }
            finally
            {
                Monitor.Exit(_infoLock);
            }
        }

        public static void Error(Exception e, string path = "")
        {
            var exceptionDetails = ExceptionBusiness.GetCallerMethodName(e);
            if (string.IsNullOrEmpty(path)) path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Log";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Monitor.Enter(_errorLock);
            try
            {
                using (var stream = File.AppendText($"{path}\\Error-{DateTime.Now.Date.ToString().Replace("/", "-")}.txt"))
                {
                    stream.WriteLine(
                        $" DateTime : {DateTime.Now}" + Environment.NewLine +
                        $" MethodName : {exceptionDetails.MethodName}" + Environment.NewLine +
                        $" Parameters : {exceptionDetails.Parameters}" + Environment.NewLine +
                        $" ExceptionLineNumber : {exceptionDetails.ExceptionLineNumber}" + Environment.NewLine +
                        $" Message : {e.Message}" + Environment.NewLine +
                        $" InnerException : {e.InnerException}" + Environment.NewLine + Environment.NewLine);
                    stream.Close();
                }
            }
            finally
            {
                Monitor.Exit(_errorLock);
            }
        }

    }
}