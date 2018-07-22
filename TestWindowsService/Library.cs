using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestWindowsService
{
    public static class Library
    {
        static readonly object _object = new object();
        private static Thread thread;
        public static void WriteErrorLog(Exception ex)
        {
            try
            {
                var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now + " - Exception : " + ex.Source.Trim() + ": " + ex.Message.Trim());
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public static void WriteLog(string message)
        {
            try
            {
                var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                WriteErrorLog(e);
            }
        }


      

        private static void DoThreadJob()
        {
            try
            {
                lock (_object)
                {
                    WriteLog(Thread.CurrentThread.Name + " - STARTS");
                    for (var i = 1; i < 20; i++)
                    {
                        Thread.Sleep(1000);
                        WriteLog(Thread.CurrentThread.Name + " - " + i);
                    }
                    WriteLog(Thread.CurrentThread.Name + " - COMPLETES");
                    thread = null;
                }
            }
            catch (Exception ex)
            {
                WriteLog("EXCEPTION in DoThreadJob");
                WriteErrorLog(ex);
            }
        }

        public static void ManageLongRunningTask()
        {
            try
            {
                //var lockedBySomeoneElse = !Monitor.TryEnter(_object);
                if (thread == null && !Monitor.IsEntered(_object)) //if (thread == null && !lockedBySomeoneElse) 
                {
                    thread = new Thread(DoThreadJob) { IsBackground = false, Name = Guid.NewGuid().ToString() };
                    thread.Start();
                }
                //DoThreadJob();


            }
            catch (Exception e)
            {
                WriteLog("EXCEPTION in ManageLongRunningTask");
                WriteErrorLog(e);
            }
        }
    }
}
