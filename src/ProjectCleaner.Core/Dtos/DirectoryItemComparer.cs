using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core.Dtos
{
    public class DirectoryItemComparer : IEqualityComparer<IDirectoryItem>
    {
        IEqualityComparer<string> comparer;

        public DirectoryItemComparer(bool caseSensitive)
        {
            this.comparer = caseSensitive ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase;
        }

        public bool Equals(IDirectoryItem x, IDirectoryItem y)
        {
            return comparer.Equals(x.FullPath, y.FullPath);
        }

        public int GetHashCode(IDirectoryItem obj)
        {
            return comparer.GetHashCode(obj.FullPath);
        }

        public static readonly DirectoryItemComparer IgnoreCase = new DirectoryItemComparer(caseSensitive: false);
    }
}
