using System;
using System.Collections.Generic;
using System.IO;

namespace Memory_Manager
{
    public class Project
    {
        private List<Tuple<string, string>> directories = new List<Tuple<string, string>>();
        private string name;        //project name
        private bool intern;        //currently the files are intern / extern stored

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
                    //Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\" + dir.Item1 );
                    DirectoryMove(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +"\\" +
                                  dir.Item1, Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName+ "\\" +
                                    dir.Item2, true);        //for testing
                    //DirectoryMove(dir.Item1, dir.Item2, true);        //original
                }
            }
            else
            {       //from extern to intern
                foreach (Tuple<string, string> dir in directories)
                {
                    DirectoryMove(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName+ "\\" +
                                  dir.Item2, Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName+ "\\" +
                                    dir.Item1, true);    //for testing
                    //DirectoryMove(dir.Item2, dir.Item1, true);    //original
                }
            }
            intern = !intern;
            
        }
        
        public void PrintsAllDirectories()
        {
            int i = 0;
            foreach (var variable in directories)
            {
                Console.WriteLine(i + ": " + variable.Item1 +" " + variable.Item2);
            }
        }

        public void RemoveDirectory()
        {
            PrintsAllDirectories();
            Console.WriteLine("enter the index of the directory that you want to delete");
            try
            {
                directories.RemoveAt(Convert.ToInt32(Console.ReadLine()));        //deletes the dir
            }
            catch (Exception){
                throw new Exception("could not cast input");
            }
        }
        
        private static void DirectoryMove(string sourceDirName, string destDirName, bool copySubDirs)
        {
            //Copies the directories an all sub directories and files and deletes the source files afterwards.
            //just moving does not work in between different devices (obviously)
            
            //from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories adapted

            Console.WriteLine("move: " + sourceDirName + "\n to: " + destDirName);

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

        
        //geter & Setter

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
        
    }
}