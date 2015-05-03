using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCleaner.Core
{
    public class HashReaderProgressArgs : EventArgs
    {
        public long BytesTotal { get; set; }
        public long BytesCompleted { get; set; }
        public long FilesTotal { get; set; }
        public long FilesCompleted { get; set; }

        public override string ToString()
        {
            return string.Format("{0:000.0}% {1} / {2}; remaining {3} of {4} files", 
                (double)BytesCompleted / (double)BytesTotal * (double)100,
                SizeFormatter.Default.Format(BytesCompleted),
                SizeFormatter.Default.Format(BytesTotal),
                FilesTotal - FilesCompleted,
                FilesTotal
            );
        }
    }
}
