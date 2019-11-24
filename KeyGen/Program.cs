using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace KeyGen
{
    class Program
    {
        private static byte[] dateTimeBytes;
        static void Main(string[] args)
        {
            var addressBytes = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault().GetPhysicalAddress().GetAddressBytes();
            var key = new int[addressBytes.Length];
            var dateTime = DateTime.Now;
            dateTime = dateTime.Date;
            dateTimeBytes = BitConverter.GetBytes(dateTime.ToBinary());

            for (var i = 0; i < key.Length; i++)
            {
                var val = addressBytes[i] ^ dateTimeBytes[i];
                if (val <= 999)
                {
                    val *= 10;
                }

                key[i] = val;
            }

            Console.WriteLine(string.Join("-", key));
            Console.ReadLine();
        }
    }
}
