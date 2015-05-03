using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core.Dtos
{
    public class FileDto : IDirectoryItem
    {
        public string Name { get { return Path.GetFileName(FullPath); } }
        public string FullPath { get; set; }
        public override string ToString()
        {
            return "FILE " + Name;
        }


        public byte[] Hash { get; set; }
        public long Size { get; set; }
    }
}
