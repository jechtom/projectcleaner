using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public abstract class TreeVisitor
    {
        protected virtual void Visit(IDirectoryItem entry)
        {
            if (entry is DirectoryDto)
                VisitDirectory((DirectoryDto)entry);

            if (entry is FileDto)
                VisitFile((FileDto)entry);
        }

        protected virtual void VisitDirectory(DirectoryDto directory)
        {
            foreach (var item in directory.Entries)
            {
                Visit(item);
            }
        }

        protected virtual void VisitFile(FileDto file)
        {
            // nothing to do
        }
    }
}
