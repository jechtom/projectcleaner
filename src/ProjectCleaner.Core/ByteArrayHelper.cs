using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public static class ByteArrayHelper
    {
        public static byte[] StringToByteArray(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");

            hex = hex.Trim();

            if (hex.Length % 2 != 0)
                throw new InvalidOperationException("Invalid length.");

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
