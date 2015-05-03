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
        public DuplicateFileFinder()
        {
        }

        public Dictionary<HashAndSize, List<FileDto>> files { get; set; }

        public DuplicateFileDto[] FindDuplicates(IDirectoryItem root)
        {
            files = new Dictionary<HashAndSize, List<FileDto>>();
            Visit(root);

            var result = files.Where(i => i.Value.Count > 1)
                .OrderByDescending(i => i.Key.Size * (i.Value.Count - 1))
                .Select(i => new DuplicateFileDto()
                {
                    HashWithSize = i.Key,
                    Instances = i.Value
                }).ToArray(); // order by duplicate size

            return result;
        }

        protected override void VisitFile(FileDto file)
        {
            if (file.Size == 0)
                return;

            // add to group
            var sizeAndHash = new HashAndSize(new Hash(file.Hash), file.Size);
            List<FileDto> list;
            if(!files.TryGetValue(sizeAndHash, out list))
            {
                list = new List<FileDto>();
                files.Add(sizeAndHash, list);
            }
            list.Add(file);
        }
    }
}
