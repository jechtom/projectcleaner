using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core.Dtos
{
    public class DirectoryDto : IDirectoryItem
    {
        public DirectoryDto()
        {
            Entries = new HashSet<IDirectoryItem>(DirectoryItemComparer.IgnoreCase);
        }

        public HashSet<IDirectoryItem> Entries { get; private set; }

        public string Name { get { return Path.GetFileName(FullPath); } }

        public string FullPath { get; set; }

        public long Size { get; set; }

        public int NestedFiles { get; set; }

        public int NestedDirectories { get; set; }

        public override string ToString()
        {
            return "DIR " + Name;
        }
    }
}
