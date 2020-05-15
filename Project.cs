using System;
using System.Collections.Generic;
using System.IO;

namespace Memory_Manager
{
    public class Project
    {
        public Project(string name, bool intern)
        {
            Name = name;
            Intern = intern;
        }

        public void AddDirectory(string orig, string ext)
        {
            Directories.Add(new Tuple<string, string>(orig, ext));
        }

        public void SwitchInternExtern()
        {
            if (Intern)
            {        //from intern to extern
                foreach (var dir in Directories)
                {
                    //Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\" + dir.Item1 );
                    DirectoryMove(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +"\\" +
                                  dir.Item1, Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName+ "\\" +
                                    dir.Item2, true);
                    //for testing
                    //DirectoryMove(dir.Item1, dir.Item2, true);        //original
                }
            }
            else
            {       //from extern to intern
                foreach (var dir in Directories)
                {
                    DirectoryMove(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName+ "\\" +
                                  dir.Item2, Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName+ "\\" +
                                    dir.Item1, true);    //for testing
                    //DirectoryMove(dir.Item2, dir.Item1, true);    //original
                }
            }
            Intern = !Intern;        //switch to the opposite
            
        }
        
        public void PrintsAllDirectories()
        {
            int i = 0;
            foreach (var variable in Directories)
            {
                Console.WriteLine($"\n{i++}: {variable.Item1} {variable.Item2}");
            }
            if (i ==0) {Console.WriteLine("\nNo directory entered in this project");}
        }

        public void RemoveDirectory()
        {
            PrintsAllDirectories();
            Console.WriteLine("enter the index of the directory that you want to delete");
            int index;
            try
            {
                index = Convert.ToInt32(Console.ReadLine());        //deletes the dir
                
            }
            catch (Exception){
                throw new Exception("could not cast input");
            }

            if (Directories.Count > index)
            {
                Directories.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Noting to delete at this index");
            }
        }
        
        private static void DirectoryMove(string sourceDirName, string destDirName, bool copySubDirs)
        {
            //Copies the directories an all sub directories and files and deletes the source files afterwards.
            //just moving does not work in between different devices (obviously)
            
            //from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories adapted

            Console.WriteLine($"move: {sourceDirName}\n to: {destDirName}");

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    $"Source directory does not exist or could not be found: {sourceDirName}");
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
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
                file.Delete();
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subDir.Name);
                    DirectoryMove(subDir.FullName, tempPath, copySubDirs);
                }
            }
        }
        
        //geter & Setter
        public string Name { get; set; }

        public bool Intern { get; set; }

        public List<Tuple<string, string>> Directories { get; set; } = new List<Tuple<string, string>>();
    }
}