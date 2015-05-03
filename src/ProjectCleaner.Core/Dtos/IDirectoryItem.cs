using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCleaner.Core.Dtos
{
    public interface IDirectoryItem
    {
        string Name { get; }

        long Size { get; }
        
        string FullPath { get; }
    }
}
