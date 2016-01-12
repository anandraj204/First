using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AskMJane.LeaflyScraper.Logging;
using AskMJane.LeaflyScraper.WorkerNodes;
using log4net;
using log4net.Config;
using SimpleConfigSections;

namespace AskMJane.LeaflyScraper
{
    static class Program
    {
        private static readonly string AssemblyDirectory;
        private static volatile bool _stopExecution;
        private static readonly ILogger _logger;
        static Program()
        {
            AssemblyDirectory =
                Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            _logger = new Logger();
            XmlConfigurator.Configure();

        }
        static void Main(string[] args)
        {
            Go();
            //SetProcessSettings();
            //SetConnectionLimits();
            //if (Environment.UserInteractive)
            //{
            //    RunAsConsole(args).Wait();
            //}
            //else
            //{
            //    // ServiceBase 
            //    ServiceBase.Run(
            //        new ServiceBase[] 
            //        { 
            //            new ConversionService(),  
            //        }
            //    );
            //}
        }

        public static async void Go()
        {
            
            var crawler = new CrawlerNode();
            var parser = new ReviewParserNode();
            var data = new DataNode();
            var id = 198565;
            
            while (id < 500000)
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = crawler.RequestPage("https://www.leafly.com/hybrid/space-queen/reviews/" + id);
                if (!String.IsNullOrWhiteSpace(res))
                {
                    var review = parser.ParseLeaflyReview(id, res);
                    var t = await data.PopulateDatabase(review);
                    
                }
                id++;
                sw.Stop();
                _logger.DebugFormat("{0} seconds elapsed processing review {1}",sw.ElapsedMilliseconds / 1000f,id);
               // if (sw.ElapsedMilliseconds < 500)
               // {
               //     Thread.Sleep(500);
               // }
            }
        }
        private static void SetProcessSettings()
        {
            var p = Process.GetCurrentProcess();
            p.PriorityClass = ProcessPriorityClass.High;
        }
        private static async Task RunAsConsole(string[] args)
        {
            //await Task.Run(() => Logger.Info("Running in the console mode."));

            //if (Console.BufferHeight < 10000) { Console.BufferHeight = 10000; }
            //if (Console.BufferWidth < 120) { Console.BufferWidth = 120; }
            ////if (Console.WindowWidth < 150) { Console.WindowWidth = 150; }
            ////if (Console.WindowHeight < 50) { Console.WindowHeight = 50; }

            //Console.Out.WriteLine("Press [Ctrl + C] to quit.");

            //var service = new ConversionService();
            //Console.CancelKeyPress += async (sender, eventArgs) =>
            //{
            //    _stopExecution = true;
            //    await service.StopAsync();
            //};

            //await service.Run(args);

            //while (!_stopExecution)
            //{
            //    await Task.Delay(50);
            //}
        }

        /// <summary>
        /// The following link points to a great article to understand how ServicePointManager affects app performance:
        /// http://blogs.msdn.com/b/jpsanders/archive/2009/05/20/understanding-maxservicepointidletime-and-defaultconnectionlimit.aspx
        /// </summary>
        private static void SetConnectionLimits()
        {
            //var config = Configuration.Get<IServicePointManagerSection>();
            //ServicePointManager.DefaultConnectionLimit = config.ServicePointManager.DefaultConnectionLimit;
            ////  indicates how long a socket will be kept open after a request and response, before the client gracefully closes the socket down. 
            //ServicePointManager.MaxServicePointIdleTime = config.ServicePointManager.MaxServicePointIdleTime;
            //ServicePointManager.Expect100Continue = config.ServicePointManager.Expect100Continue;
            //ServicePointManager.CheckCertificateRevocationList = config.ServicePointManager.CheckCertificateRevocationList;
        }
    
    }
}
