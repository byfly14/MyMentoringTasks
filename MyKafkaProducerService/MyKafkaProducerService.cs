using System.ServiceProcess;
using System.Threading;

namespace MyKafkaProducerService
{
    public partial class MyKafkaProducerService : ServiceBase
    {
        private const string BrokerEndpoints = "localhost:9092";
        private const string FolderToProduce = @"D:\FolderToProduce";
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private MyKafkaProducer _producer;

        public MyKafkaProducerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            _producer = new MyKafkaProducer(BrokerEndpoints, FolderToProduce);
            _producer.Start(_cts.Token);
        }

        protected override void OnStop()
        {
            _cts.Cancel();
            _producer.Dispose();
        }
    }
}
