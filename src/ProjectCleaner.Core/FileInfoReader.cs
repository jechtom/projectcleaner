using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public class FileInfoReader : TreeVisitor
    {
        public void ReadFileInfos(DirectoryDto root)
        {
            Visit(root);
            new CountVisitor().Recalculate(root);
        }

        protected override void Visit(IDirectoryItem entry)
        {
            base.Visit(entry);
        }

        protected override void VisitDirectory(Dtos.DirectoryDto directory)
        {
            base.VisitDirectory(directory);

            // calculate total size of folder
            directory.Size = directory.Entries.Sum(e => e.Size);
        }

        protected override void VisitFile(FileDto file)
        {
            var info = new FileInfo(file.FullPath);
            file.Size = info.Length;
        }
    }
}
