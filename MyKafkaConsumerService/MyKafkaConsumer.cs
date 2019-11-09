using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using MyKafka.Common;

namespace MyKafkaConsumerService
{
    public class MyKafkaConsumer : IDisposable
    {
        private bool _disposed;
        private Task _mainThread;

        private readonly KafkaConsumer<byte[]> _consumer;
        private readonly KafkaConsumer<string> _broadcastConsumer;
        private readonly string _folderToConsume;
        private readonly object _locker = new object();
        private readonly BlockingCollection<ConsumeResult<Null, byte[]>> _queue = new BlockingCollection<ConsumeResult<Null, byte[]>>();

        public MyKafkaConsumer(string brokerEndpoints, string folderToConsume)
        {
            if (!Directory.Exists(folderToConsume))
            {
                Directory.CreateDirectory(folderToConsume);
            }

            _folderToConsume = folderToConsume;

            _broadcastConsumer = new KafkaConsumer<string>(brokerEndpoints);
            _broadcastConsumer.ItemConsumed += BroadcastItemConsumed;

            _consumer = new KafkaConsumer<byte[]>(brokerEndpoints);
            _consumer.ItemConsumed += ConsumerItemConsumed;
        }

        private void BroadcastItemConsumed(object sender, ConsumeResult<Null, string> e)
        {
            if (e.Topic == "broadcast")
            {
                _consumer.Subscribe(e.Value);
            }
        }

        public void Listen(CancellationToken token)
        {
            _broadcastConsumer.Subscribe("broadcast");
            _broadcastConsumer.StartConsuming(token);
            _consumer.StartConsuming(token);
            _mainThread = CreateConsumerThread(token);
        }

        private Task CreateConsumerThread(CancellationToken token)
        {
            // ReSharper disable once MethodSupportsCancellation
            return Task.Run(() =>
            {
                var binForm = new BinaryFormatter();

                foreach (var consumeResult in _queue.GetConsumingEnumerable())
                {
                    if (token.IsCancellationRequested)
                    {
                        _queue.CompleteAdding();
                    }

                    using (var ms = new MemoryStream())
                    {
                        ms.Write(consumeResult.Value, 0, consumeResult.Value.Length);
                        ms.Seek(0, SeekOrigin.Begin);
                        var obj = (MessageObject) binForm.Deserialize(ms);

                        lock (_locker)
                        {
                            using (var file = File.Open(Path.Combine(_folderToConsume, obj.Name), FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                file.Seek(obj.Offset, SeekOrigin.Begin);
                                file.Write(obj.Data, 0, obj.Data.Length);
                            }
                        }
                    }
                }
            });
        }


        private void ConsumerItemConsumed(object sender, ConsumeResult<Null, byte[]> e)
        {
            _queue.Add(e);
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
                _consumer.ItemConsumed -= ConsumerItemConsumed;
                _consumer.Dispose();

                _broadcastConsumer.ItemConsumed -= BroadcastItemConsumed;
                _broadcastConsumer.Dispose();
            }

            _disposed = true;
        }
    }
}
