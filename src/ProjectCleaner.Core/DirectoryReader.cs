using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public class DirectoryReader
    {
        public string RootPath { get; set; }

        public Dtos.DirectoryDto ReadDirectory()
        {
            var result = VisitDirectory(RootPath, parent: null);
            new CountVisitor().Recalculate(result);
            return result;
        }

        private Dtos.DirectoryDto VisitDirectory(string dirPath, DirectoryDto parent)
        {
            var dir = new DirectoryDto();
            dir.FullPath = dirPath;

            try
            {
                foreach (var filePath in Directory.EnumerateFiles(dirPath))
                {
                    var nestedFile = new FileDto();
                    nestedFile.FullPath = filePath;
                    dir.Entries.Add(nestedFile);
                }

                foreach (var nestedDirPath in Directory.EnumerateDirectories(dirPath))
                {
                    var nestedDir = VisitDirectory(nestedDirPath, parent: dir);
                    dir.Entries.Add(nestedDir);
                }
            }
            catch(PathTooLongException)
            {
                Console.WriteLine("Ignoring too long path: " + dirPath);
            }

            return dir;
        }
    }
}
