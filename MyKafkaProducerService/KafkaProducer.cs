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
        private readonly FileSystemWatcher _configFileSystemWatcher;
        private readonly ProducerConfig _config;

        private bool _disposed;
        public KafkaProducer(string brokerEndpoints)
        {
            _config = new ProducerConfig { BootstrapServers = brokerEndpoints };
            //System.Diagnostics.Debugger.Launch();
            //if (typeof(T) != typeof(string))
            //{
            //    _configFileSystemWatcher = new FileSystemWatcher(@"D:\ConfigFolder");
            //    _configFileSystemWatcher.EnableRaisingEvents = true;
            //    _configFileSystemWatcher.Changed += ConfigFileSystemWatcherOnChanged;
            //    UpdateMessageMaxBytes(@"D:\ConfigFolder\config.txt");
            //}

            var producerBuilder = new ProducerBuilder<Null, T>(_config);
            _producer = producerBuilder.Build();
        }

        private void ConfigFileSystemWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            _configFileSystemWatcher.EnableRaisingEvents = false;
            if (e.Name == "config.txt")
            {
                UpdateMessageMaxBytes(e.FullPath);
            }
            _configFileSystemWatcher.EnableRaisingEvents = true;
        }

        private void UpdateMessageMaxBytes(string filePath)
        {
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    var previousValue = _config.MessageMaxBytes;
                    var data = reader.ReadLine();
                    if (!int.TryParse(data, out var result))
                    {
                        return;
                    }

                    _config.MessageMaxBytes = result;
                    File.AppendAllLines(@"D:\ConfigChanges.txt",
                        new List<string>
                            {$"{DateTime.Now}: PreviousValue = {previousValue}, CurrentValue = {_config.MessageMaxBytes}"});
                }
            }
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
                throw new KafkaException(e.Error);
            }
            catch (Exception ex)
            {
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
                if (_configFileSystemWatcher != null)
                {
                    _configFileSystemWatcher.EnableRaisingEvents = false;
                    _configFileSystemWatcher.Changed -= ConfigFileSystemWatcherOnChanged;
                    _configFileSystemWatcher.Dispose();
                }

                _producer.Dispose();
            }

            _disposed = true;
        }
    }
}
