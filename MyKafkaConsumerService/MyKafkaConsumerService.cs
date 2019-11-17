using System.ServiceProcess;
using System.Threading;

namespace MyKafkaConsumerService
{
    public partial class MyKafkaConsumerService : ServiceBase
    {
        private const string BrokerEndpoints = "localhost:9092";
        private const string FolderToConsume = @"D:\FolderToConsume";
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private MyKafkaConsumer _consumer;

        public MyKafkaConsumerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _consumer = new MyKafkaConsumer(BrokerEndpoints, FolderToConsume);
            _consumer.Listen(_cts.Token);
        }

        protected override void OnStop()
        {
            _cts.Cancel();
            _consumer.Dispose();
        }
    }
}
