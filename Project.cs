using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Memory_Manager
{
    public class Project
    {
        private List<Tuple<string, string>> directories = new List<Tuple<string, string>>();
        private string name;
        private bool intern;

        public Project(string name, bool intern)
        {
            this.name = name;
            this.intern = intern;
        }

        public void AddDirectory(string orig, string ext)
        {
            directories.Add(new Tuple<string, string>(orig, ext));
        }

        public void SwitchInternExtern()
        {
            if (intern)
            {        //from intern to extern
                foreach (Tuple<string, string> dir in directories)
                {
                    DirectoryMove(dir.Item1, dir.Item2, true);
                }
            }
            else
            {       //from extern to intern
                foreach (Tuple<string, string> dir in directories)
                {
                    DirectoryMove(dir.Item2, dir.Item1, true);
                }
            }
            intern = !intern;
            
        }

        private static void DirectoryMove(string sourceDirName, string destDirName, bool copySubDirs)
        {
            //from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
            
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
                file.Delete();
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryMove(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    
        public string Name
        {
            get => name;
            set => name = value;
        }

        public bool Intern
        {
            get => intern;
            set => intern = value;
        }

        public List<Tuple<string, string>> Directories
        {
            get => directories;
            set => directories = value;
        }

        public void ListAllDirectories()
        {
            int i = 0;
            foreach (var variable in directories)
            {
                Console.WriteLine(i + ": " + variable.Item1 +" " + variable.Item2);
            }
        }

        public void RemoveDirectory()
        {
            int i = 0;
            foreach (var variable in directories)
            {
                Console.WriteLine(i + ": " + variable.Item1 +" " + variable.Item2);
            }
            Console.WriteLine("enter the index of the directory that you want to delete");
            int index = Convert.ToInt32(Console.ReadLine()); 
            //TODO Error handling
            directories.RemoveAt(index);
        }
        
        
        
    }
}