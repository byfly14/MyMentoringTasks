using System.ServiceProcess;
using System.Threading;
using MyKafka.Common;
using MyProxyLoggerNameSpace;

namespace MyKafkaProducerService
{
    public partial class MyKafkaProducerService : ServiceBase
    {
        private const string BrokerEndpoints = "localhost:9092";
        private const string FolderToProduce = @"D:\FolderToProduce";
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private MyKafkaProducer_Proxy _producer;

        public MyKafkaProducerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            IMyKafkaProducer target = new MyKafkaProducer(BrokerEndpoints, FolderToProduce);
            _producer = new MyKafkaProducer_Proxy(target);
            _producer.Start(_cts.Token);
        }

        protected override void OnStop()
        {
            _cts.Cancel();
        }
    }
}
