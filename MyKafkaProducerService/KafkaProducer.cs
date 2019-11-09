using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace MyKafkaProducerService
{
    public class KafkaProducer<T> : IDisposable
    {
        private readonly IProducer<Null, T> _producer;

        private bool _disposed;
        public KafkaProducer(string brokerEndpoints)
        {
            var config = new ProducerConfig { BootstrapServers = brokerEndpoints };

            var producerBuilder = new ProducerBuilder<Null, T>(config);
            _producer = producerBuilder.Build();
        }

        public async Task<DeliveryResult<Null, T>> ProduceAsync(T value, string topic, int partitionId = -1)
        {
            try
            {
                var message = new Message<Null, T> { Value = value };

                DeliveryResult<Null, T> deliveryResult;
                if (partitionId < 0)
                {
                    deliveryResult = await _producer.ProduceAsync(topic, message).ConfigureAwait(false);
                }
                else
                {
                    var partition = new Partition(partitionId);
                    var topicPartition = new TopicPartition(topic, partition);
                    deliveryResult = await _producer.ProduceAsync(topicPartition, message).ConfigureAwait(false);
                }

                return deliveryResult;
            }
            catch (ProduceException<Null, T> e)
            {
                File.AppendAllLines(@"D:\1.txt", new List<string> { e.ToString() });
                throw new KafkaException(e.Error);
            }
            catch (Exception ex)
            {
                File.AppendAllLines(@"D:\1.txt", new List<string> { ex.ToString() });
                throw;
            }
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
                _producer.Dispose();
            }

            _disposed = true;
        }
    }
}
