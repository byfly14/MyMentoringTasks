using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace MyKafkaConsumerService
{
    public class KafkaConsumer<T> : IKafkaConsumer<T>
    {
        private readonly IConsumer<Null, T> _consumer;

        private bool _disposed;

        public EventHandler<ConsumeResult<Null, T>> ItemConsumed { get; set; }

        public KafkaConsumer(string brokerEndpoints)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = brokerEndpoints,
                GroupId = "my-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumerBuilder = new ConsumerBuilder<Null, T>(config);
            _consumer = consumerBuilder.Build();
        }

        public void Subscribe(string topic)
        {
            System.Diagnostics.Debugger.Launch();
            _consumer.Subscribe(topic);
        }

        public Task StartConsuming(CancellationToken cancellationToken)
        {
            // ReSharper disable once MethodSupportsCancellation
            return Task.Run(() => {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var data = _consumer.Consume(cancellationToken);
                    ItemConsumed?.Invoke(_consumer, data);
                }
            });
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _consumer.Unsubscribe();
                _consumer.Dispose();
            }

            _disposed = true;
        }
    }

    public interface IKafkaConsumer<T> : IDisposable
    {
        void Subscribe(string topic);
        Task StartConsuming(CancellationToken cancellationToken);
        EventHandler<ConsumeResult<Null, T>> ItemConsumed { get; set; }
    }
}
