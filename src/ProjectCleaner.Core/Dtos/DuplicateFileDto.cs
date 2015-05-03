using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core.Dtos
{
    public class DuplicateFileDto
    {
        public HashAndSize HashWithSize { get; set; }

        public List<FileDto> Instances { get; set; }
    }
}
