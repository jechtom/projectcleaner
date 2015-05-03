using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ProjectCleaner.Core
{
    public class VisualStudioCleaner : TreeVisitor
    {
        private List<string> foldersToDelete;
        private Stack<DirectoryDto> dirStack;

        public string[] FindFoldersToClean(DirectoryDto root)
        {
            foldersToDelete = new List<string>();
            dirStack = new Stack<DirectoryDto>();
            Visit(root);
            return foldersToDelete.ToArray();
        }


        protected override void VisitDirectory(Dtos.DirectoryDto directory)
        {
            dirStack.Push(directory);
            base.VisitDirectory(directory);
            dirStack.Pop();
        }

        protected override void VisitFile(FileDto file)
        {
            var ext = Path.GetExtension(file.FullPath) ?? string.Empty;
            bool isCsproj = string.Equals(ext, ".csproj", StringComparison.InvariantCultureIgnoreCase);
            bool isVbproj = string.Equals(ext, ".vbproj", StringComparison.InvariantCultureIgnoreCase);

            if (isCsproj || isVbproj)
            {
                ProcessVbOrCsProjFile(file);
            }

            base.VisitFile(file);
        }

        private void ProcessVbOrCsProjFile(FileDto file)
        {
            string projectDir = Path.GetDirectoryName(file.FullPath);
            
            var doc = new XmlDocument();
            doc.Load(file.FullPath);
            var names = new XmlNamespaceManager(doc.NameTable);
            names.AddNamespace("p2003", "http://schemas.microsoft.com/developer/msbuild/2003");
            
            // obj dirs
            foldersToDelete.Add(Path.Combine(projectDir, "obj"));

            // output dirs
            foreach (XmlElement outputPathNode in doc.SelectNodes("/p2003:Project/p2003:PropertyGroup/p2003:OutputPath", names))
            {
                var refPath = outputPathNode.InnerText;
                var outputhPathComplete = Path.Combine(projectDir, refPath);
                foldersToDelete.Add(outputhPathComplete);
            }
        }
    }
}
