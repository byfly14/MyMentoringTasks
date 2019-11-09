using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using MyKafka.Common;

namespace MyKafkaProducerService
{
    public class MyKafkaProducer : IDisposable
    {
        private bool _disposed;
        private Task _mainThread;

        private readonly KafkaProducer<byte[]> _kafkaProducer;
        private readonly KafkaProducer<string> _broadcastProducer;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private readonly BlockingCollection<FileSystemEventArgs> _blockingCollection = new BlockingCollection<FileSystemEventArgs>();

        public MyKafkaProducer(string brokerEndpoints, string folderToProduce)
        {
            if (Directory.Exists(folderToProduce))
            {
                _fileSystemWatcher = new FileSystemWatcher(folderToProduce);

                _fileSystemWatcher.Created += FileSystemWatcherOnCreated;
                _fileSystemWatcher.EnableRaisingEvents = true;
            }

            _kafkaProducer = new KafkaProducer<byte[]>(brokerEndpoints);
            _broadcastProducer = new KafkaProducer<string>(brokerEndpoints);
        }

        public void Start(CancellationToken cancellationToken)
        {
            _mainThread = StartMainThread(cancellationToken);
        }

        private Task StartMainThread(CancellationToken cancellationToken)
        {
            // ReSharper disable once MethodSupportsCancellation
            return Task.Run(() =>
            {
                foreach (var file in _blockingCollection.GetConsumingEnumerable())
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _blockingCollection.CompleteAdding();
                    }

                    StartFileProcessing(file, cancellationToken);
                }
            });
        }

        private async Task StartFileProcessing(FileSystemEventArgs fileSystemEventArgs, CancellationToken cancellationToken)
        {
            using (var file = File.Open(fileSystemEventArgs.FullPath, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[102400];
                var topic = Guid.NewGuid().ToString();
                await _broadcastProducer.ProduceAsync(topic, "broadcast").ConfigureAwait(false);

                var bf = new BinaryFormatter();

                // ReSharper disable once MethodSupportsCancellation
                while (!cancellationToken.IsCancellationRequested && file.Read(buffer, 0, buffer.Length) > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        bf.Serialize(ms, new MessageObject { Data = buffer, Offset = file.Position - buffer.Length, Name = fileSystemEventArgs.Name});
                        var data = ms.ToArray();

                        var deliveryResult = await _kafkaProducer.ProduceAsync(data, topic).ConfigureAwait(false);
                        File.AppendAllLines(@"D:\1.txt",
                            new List<string>
                            {
                                $"Message '{deliveryResult.Value}' produced to '{deliveryResult.Topic}'"
                            });
                    }
                }
            }
        }

        private void FileSystemWatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            _blockingCollection.Add(e);
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
                _fileSystemWatcher.Created -= FileSystemWatcherOnCreated;
                _fileSystemWatcher.Dispose();
                _broadcastProducer.Dispose();
                _kafkaProducer.Dispose();
            }

            _disposed = true;
        }
    }
}
