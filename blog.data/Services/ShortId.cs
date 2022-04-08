using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace blog.data.Services
{
    internal static class ShortId
    {
        private static char[] values = { '0', '1', '2', '3', '4', '5', '6', '7', '8','9', 'a', 'b',
            'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
            't', 'u', 'v', 'w', 'x', 'y', 'z', '-' };

        public static string NewId()
        {
            using (var rand = RandomNumberGenerator.Create())
            {
                var bytes = new byte[4];
                rand.GetBytes(bytes);
                var value = BitConverter.ToInt32(bytes, 0);
                var random = new Random(value);
                var result = new StringBuilder();
                while (result.Length < 12)
                {
                    result.Append(values[random.Next(values.Length)]);
                }
                return result.ToString();
            }
        }
    }
}