using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public class DuplicateFileFinder : TreeVisitor
    {
        public DuplicateFileFinder(HashReader hashReader)
        {
            this.hashReader = hashReader ?? throw new ArgumentNullException(nameof(hashReader));
        }

        private class FilesOfSize
        {
            public FileDto FirstFileWithSize { get; set; }
            public List<FileDto> Files { get; set; }
            public bool FoundMoreThanOne => Files?.Any() ?? false;
        }

        HashReader hashReader;
        private int filesCount, filesCountHash;
        Dictionary<long, FilesOfSize> filesOfSize { get; set; }

        public DuplicateFileDto[] FindDuplicates(IDirectoryItem root)
        {
            filesOfSize = new Dictionary<long, FilesOfSize>();
            Visit(root);
            Console.WriteLine("Collecting duplicates...");
            var result = filesOfSize
                .Where(fi => fi.Value.FoundMoreThanOne)
                .SelectMany(fi => fi.Value.Files
                    .GroupBy(fil => fil.HashAndSize)
                    .Where(g => g.Count() > 1)
                    .Select(g => new DuplicateFileDto() {  
                        Instances = g.ToList(),
                        HashWithSize = g.Key
                    }))
                .OrderByDescending(i => i.HashWithSize.Size * (i.Instances.Count - 1))
                .ToArray(); // order by duplicate size

            return result;
        }

        protected override void VisitFile(FileDto file)
        {
            filesCount++;
            if (filesCount % 5000 == 0)
            {
                Console.WriteLine($" - progress: Processed {filesCount} files. Hash calculated for {filesCountHash} files.");
                //int cmax = filesOfSize.Aggregate(new List<FileDto>(), (s, fi) => fi.Value.FoundMoreThanOne && fi.Value.Files.Count > s.Count ? fi.Value.Files : s).Count;
                //Console.WriteLine($" - progress: Max files with same size: {cmax}");
            }

            if (file.Size == 0)
                return;

            // add to group but dont calculate hash until second file with same size is found
            if(!filesOfSize.TryGetValue(file.Size, out FilesOfSize fileOfSize))
            {
                filesOfSize.Add(file.Size, new FilesOfSize() { FirstFileWithSize = file });
                return;
            }
            
            // add first value to list and calculate hash
            if(fileOfSize.Files == null)
            {
                fileOfSize.Files = new List<FileDto>();
                hashReader.ReadHashIfNotKnown(fileOfSize.FirstFileWithSize);
                fileOfSize.Files.Add(fileOfSize.FirstFileWithSize);
                filesCountHash++;
            }

            // calculate hash of new file
            hashReader.ReadHashIfNotKnown(file);
            fileOfSize.Files.Add(file);
            filesCountHash++;
        }
    }
}
