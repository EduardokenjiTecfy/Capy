using ConsoleApplication18.OcrImpletentacao;
using Google.Cloud.Vision.V1;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleApplication18
{
    class Program
    {

        public Program()
        {
        }

        private static void Main()
        {




            //// ServiceCanhotos.timer_Elapsed_machine(null,null);
            //while (true) {



            new ServiceCanhotos().timer_Elapsed(null, null);
            //  Thread.Sleep(10000);
            // }


            ServiceBase.Run(new ServiceBase[] { new ServiceCanhotos() });
        }

        //    static void Main()
        //    {
        //        ServiceBase[] ServicesToRun;
        //        ServicesToRun = new ServiceBase[]
        //        {
        //            new ServiceCanhotos()
        //        };
        //        ServiceBase.Run(ServicesToRun);
        //        //  new ServiceCanhotos().timer_Elapsed(null, null);
        //    } 

        //}
    }
}