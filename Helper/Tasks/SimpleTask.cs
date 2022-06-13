using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFieldBooking.Helper.Tasks
{
    public class SimpleTask : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() => logConsole("Hung's Cron test"));
            return task;
        }
        public void logfile(DateTime time)
        {
            string path = "C:\\log\\sample.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(time);
                writer.Close();
            }
        }

        public void logConsole (string content)
        {
            Console.WriteLine(content);
        }

    }
}
