using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public class CountVisitor : TreeVisitor
    {
        public void Recalculate(DirectoryDto root)
        {
            Visit(root);
        }

        protected override void VisitDirectory(Dtos.DirectoryDto directory)
        {
            base.VisitDirectory(directory);

            // count directories
            directory.NestedDirectories =
                directory.Entries.OfType<Dtos.DirectoryDto>().Sum(d => d.NestedDirectories)
                + directory.Entries.OfType<Dtos.DirectoryDto>().Count();

            // count files
            directory.NestedFiles =
                directory.Entries.OfType<Dtos.DirectoryDto>().Sum(d => d.NestedFiles)
                + directory.Entries.OfType<Dtos.FileDto>().Count();

            // aggregate size
            directory.Size =
                directory.Entries.Sum(e => e.Size);
        }
    }
}
