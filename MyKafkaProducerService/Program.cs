using System.ServiceProcess;

namespace MyKafkaProducerService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            System.Diagnostics.Debugger.Launch();
            var servicesToRun = new ServiceBase[]
            {
                new MyKafkaProducerService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
