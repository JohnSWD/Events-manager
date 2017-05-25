using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace EventsManager
{
    public class LoggingTool
    {
        public void WriteErrorLog(Exception e)
        {
            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logFolder)) Directory.CreateDirectory(logFolder);
            string logFile = Path.Combine(logFolder, string.Format("EventsManager.Errors_{0}.log", DateTime.Now.ToString("dd/MM/yyyy"))); //DateTime.Now.ToString("dd/MM/yyyy"), ".log"
            using (var sw = new StreamWriter(logFile, true))
            {
                sw.WriteLine("Название ошибки: " + e.Message);
                sw.WriteLine(string.Format("Источник ошибки: {0}.{1}()", e.TargetSite.DeclaringType, e.TargetSite.Name));
                sw.WriteLine("Время возниковения ошибки: " + DateTime.Now);
            }
        }

        public void WriteActionLog(string info)
        {
            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logFolder)) Directory.CreateDirectory(logFolder);
            string logFile = Path.Combine(logFolder, string.Format("EventsManager.Actions_{0}.log", DateTime.Now.ToString("dd/MM/yyyy")));
            using (var sw = new StreamWriter(logFile, true))
            {
                sw.WriteLine(string.Format("[{0}] {1}",DateTime.Now, info));
            }
        }
    }
}
