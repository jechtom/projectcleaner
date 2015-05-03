using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public class SizeFormatter
    {
        public static SizeFormatter Default = new SizeFormatter();

        static readonly string[] suffixes = new[] { "B", "KB", "MB", "GB", "TB", "PB" };

        public string Format(long sizeInBytes)
        {
            double size = sizeInBytes;

            foreach (var suf in suffixes)
            {

                if (size < 1024 || suf == suffixes.Last())
                    return string.Format("{0:0.0} {1}", size, suf);

                size /= 1024;
            }

            return null;
        }
    }
}
