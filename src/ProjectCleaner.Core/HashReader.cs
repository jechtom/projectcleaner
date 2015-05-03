using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public class HashReader : TreeVisitor
    {
        long bytesCompleted;
        long bytesTotal;
        long filesCompleted;
        long filesTotal;
        Stopwatch lastNotify;

        public TimeSpan? NotifyMinimumInterval { get; set; }

        public void ReadHashes(DirectoryDto root)
        {
            // init
            bytesTotal = root.Size;
            bytesCompleted = 0;
            filesTotal = root.NestedFiles;
            filesCompleted = 0;

            // visit
            Visit(root);

            // finish
            bytesTotal = bytesCompleted;
            NotifyProgress(forceNotify: true);
        }

        protected override void VisitFile(FileDto file)
        {
            using(var stream = File.OpenRead(file.FullPath))
            {
                using(var hashAlg = new SHA1Managed())
                {
                    file.Hash = hashAlg.ComputeHash(stream);
                }
            }

            filesCompleted += 1;
            bytesCompleted += file.Size;
            NotifyProgress(forceNotify: false);
        }

        private void NotifyProgress(bool forceNotify)
        {
            if (ProgressChange == null)
                return;

            if(NotifyMinimumInterval.HasValue)
            {
                if(lastNotify == null)
                {
                    lastNotify = Stopwatch.StartNew();
                }
                else if(lastNotify.Elapsed >= NotifyMinimumInterval.Value || forceNotify)
                {
                    lastNotify.Restart();
                }
                else
                {
                    return; // skip
                }
            }
            
            ProgressChange(this, new HashReaderProgressArgs()
            {
                BytesTotal = bytesTotal,
                BytesCompleted = bytesCompleted,
                FilesCompleted = filesCompleted,
                FilesTotal = filesTotal
            });
        }

        public event EventHandler<HashReaderProgressArgs> ProgressChange;
    }
}
