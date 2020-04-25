using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;

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
            this.intern = !intern;
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