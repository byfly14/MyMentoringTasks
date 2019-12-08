using System.ServiceProcess;
using Autofac;
using Castle.DynamicProxy;

namespace MyKafkaConsumerService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            System.Diagnostics.Debugger.Launch();
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ProxyGenerator()).SingleInstance();
            builder.RegisterInstance(new CallLoggingInterceptor()).SingleInstance();
            builder.RegisterType(typeof(MyKafkaConsumerService)).SingleInstance();
            var container = builder.Build();
            var servicesToRun = new ServiceBase[]
            {
                container.Resolve<MyKafkaConsumerService>()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
