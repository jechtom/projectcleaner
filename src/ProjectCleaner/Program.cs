﻿using ProjectCleaner.Core;
using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            // fetch list of files
            var dirReader = new DirectoryReader();
            dirReader.RootPath = Environment.CurrentDirectory;
            Console.WriteLine(string.Format("Root Path:\n{0}", dirReader.RootPath));
            Console.WriteLine(string.Format("Reading directories and files..."));
            var data = dirReader.ReadDirectory();
            Console.WriteLine(string.Format("Folders: {0}; Files: {1}", data.NestedDirectories, data.NestedFiles));

            // read sizes
            Console.WriteLine(string.Format("Reading size..."));
            new FileInfoReader().ReadFileInfos(data);
            Console.WriteLine(string.Format("Total size: {0}", SizeFormatter.Default.Format(data.Size)));

            // read hashes
            Console.WriteLine(string.Format("Calculating hash..."));
            var hashReader = new HashReader();
            hashReader.NotifyMinimumInterval = TimeSpan.FromSeconds(3);
            hashReader.ProgressChange += (s, e) => { Console.WriteLine(string.Format("Hashing progress: {0}", e)); };
            hashReader.ReadHashes(data);

            // analyze
            Console.WriteLine(string.Format("Finding duplicates..."));
            var duplicates = new DuplicateFileFinder().FindDuplicates(data);
            Console.WriteLine(string.Format("Finding GIT repositories to compress..."));
            var gitRepositories = new GitFolderFinder().FindRepositories(data);
            Console.WriteLine(string.Format("Finding Visual Studio projects to clean..."));
            var visualStudioFoldersToClean = new VisualStudioCleaner().FindFoldersToClean(data);
            Console.WriteLine(string.Format("Finished."));

            // generate reports
            string reportBasePath = data.FullPath; // generate to root path

            // report: duplicates
            using(var sw = OpenReportFile(data, reportBasePath, "cleaner_report_duplicates.txt"))
            {
                sw.WriteLine("Duplicates:");
                foreach (var item in duplicates)
                {
                    sw.WriteLine(string.Format(" [duplicate] Count: {0}; Size: {1} Duplicate size: {2}", 
                            item.Instances.Count,
                            SizeFormatter.Default.Format(item.HashWithSize.Size),
                            SizeFormatter.Default.Format(item.HashWithSize.Size * (item.Instances.Count - 1))
                        ));
                    foreach (var itemInstance in item.Instances)
                    {
                        sw.WriteLine(string.Format("  [instance] {0}", itemInstance.FullPath));
                    }
                }
            }

            // report: visual studio folders to clean
            using (var sw = OpenReportFile(data, reportBasePath, "cleaner_report_clean.txt"))
            {
                sw.WriteLine("REM Visual Studio files to clean script");
                sw.WriteLine("REM ! Warning: Review before execution");
                foreach (var item in visualStudioFoldersToClean)
                {
                    sw.WriteLine(string.Format("rmdir /S /Q \"{0}\"", item));
                }
                sw.WriteLine("REM -- End --");
            }

            // prepare GIT GC commands
            using (var sw = OpenReportFile(data, reportBasePath, "cleaner_report_git.txt"))
            {
                sw.WriteLine("REM Script for GIT GC (clean and packing)");
                foreach (var item in gitRepositories)
                {
                    sw.WriteLine(string.Format("cd \"{0}\"", item.FullPath));
                    sw.WriteLine("git gc");
                }
                sw.WriteLine("REM -- End --");
            }
        }

        private static StreamWriter OpenReportFile(DirectoryDto root, string reportBasePath, string fileName)
        {
            string reportFile = Path.Combine(reportBasePath, fileName);
            Console.WriteLine(string.Format("Generating report: {0}", reportFile));
            var stream = File.Open(reportFile, FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(stream, Encoding.UTF8, 512, leaveOpen: false);

            sw.WriteLine("REM REPORT " + root.FullPath);
            sw.WriteLine();

            return sw;
        }
    }
}