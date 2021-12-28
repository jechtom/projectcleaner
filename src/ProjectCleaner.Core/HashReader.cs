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
    public class HashReader
    {
        public void ReadHashIfNotKnown(FileDto file)
        {
            if (file.IsHashKnown) return;
            using (var stream = File.OpenRead(file.FullPath))
            {
                using (var hashAlg = SHA1.Create())
                {
                    file.HashAndSize = new HashAndSize(new Hash(hashAlg.ComputeHash(stream)), file.Size);
                    file.IsHashKnown = true;
                }
            }
        }
    }
}
