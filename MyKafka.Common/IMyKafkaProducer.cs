using System;
using System.Threading;

namespace MyKafka.Common
{
    public interface IMyKafkaProducer : IDisposable
    {
        void Start(CancellationToken cancellationToken);
    }
}
