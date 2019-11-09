using System;

namespace MyKafka.Common
{
    [Serializable]
    public class MessageObject
    {
        public long Offset { get; set; }
        public byte[] Data { get; set; }
        public string Name { get; set; }
    }
}
