using System.ServiceProcess;
using System.Threading;
using Castle.DynamicProxy;

namespace MyKafkaConsumerService
{
    public partial class MyKafkaConsumerService : ServiceBase
    {
        private const string BrokerEndpoints = "localhost:9092";
        private const string FolderToConsume = @"D:\FolderToConsume";
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly IMyKafkaConsumer _proxy;

        public MyKafkaConsumerService(CallLoggingInterceptor callLoggingInterceptor, ProxyGenerator proxyGenerator)
        {
            InitializeComponent();
            _proxy = (IMyKafkaConsumer)proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                typeof(IMyKafkaConsumer), new MyKafkaConsumer(BrokerEndpoints, FolderToConsume, callLoggingInterceptor, proxyGenerator),
                ProxyGenerationOptions.Default, callLoggingInterceptor);
        }

        protected override void OnStart(string[] args)
        {
            _proxy.Listen(_cts.Token);
        }

        protected override void OnStop()
        {
            _cts.Cancel();
            _proxy.Dispose();
        }
    }
}
