using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TestWindowsService
{
    // https://www.c-sharpcorner.com/UploadFile/naresh.avari/develop-and-install-a-windows-service-in-C-Sharp/?
    // Developer command prompt for visual studio
    // Set path to debug forlder - cd D:\Study projects\TestWindowsService\TestWindowsService\bin\Debug
    // InstallUtil.exe "TestWindowsService.exe"
    // InstallUtil.exe /u "TestWindowsService.exe"
    public partial class Scheduler : ServiceBase
    {
        private Timer _timer1;
        

        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _timer1 = new Timer { Interval = 5000 };    // every 5 seconds

                _timer1.Elapsed += Timer1_Tick;
                _timer1.Enabled = true;
                Thread.CurrentThread.Name = "Main Thread";
                Library.WriteLog("Test Windows Service Started");
            }
            catch (Exception e)
            {
                Library.WriteLog("Error on starting windows service, excpetion details will follow - ");
                Library.WriteErrorLog(e);
            }
        }

        private void Timer1_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                Library.WriteLog("Timer Ticked");
                Library.ManageLongRunningTask();
            }
            catch (Exception exception)
            {
                Library.WriteLog("Error on ticking windows service, excpetion details will follow - ");
                Library.WriteErrorLog(exception);
            }
        }

        protected override void OnStop()
        {
            try
            {
                _timer1.Enabled = false;
                Library.WriteLog("Test Windows Service Stopped");
            }
            catch (Exception e)
            {
                Library.WriteLog("Error on stopping windows service, excpetion details will follow - ");
                Library.WriteErrorLog(e);
            }
        }
    }
}
