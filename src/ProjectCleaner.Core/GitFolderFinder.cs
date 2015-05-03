using ProjectCleaner.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCleaner.Core
{
    public class GitFolderFinder : TreeVisitor
    {
        private List<DirectoryDto> repos;
        private Stack<DirectoryDto> dirStack;

        public DirectoryDto[] FindRepositories(DirectoryDto root)
        {
            repos = new List<DirectoryDto>();
            dirStack = new Stack<DirectoryDto>();
            Visit(root);
            return repos.ToArray();
        }


        protected override void VisitDirectory(Dtos.DirectoryDto directory)
        {
            if(dirStack.Count > 0 && directory.Name.Equals(".git", StringComparison.InvariantCultureIgnoreCase))
            {
                repos.Add(dirStack.Peek() /* parent dir */);
                return; // ignore nested
            }

            dirStack.Push(directory);
            base.VisitDirectory(directory);
            dirStack.Pop();
        }
    }
}
