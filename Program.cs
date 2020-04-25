using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Memory_Manager
{
    internal static class Program
    {
        private static readonly string pathSettings = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
                                                      "./Data/settings.xml";

        private static readonly string pathProjects = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
                                                      "./Data/projects.xml";

        public static void Main(string[] args)
        {
            //import
            Settings settings = InExport.ImportSettings(pathSettings);
            List<Project> projects = InExport.ImportProjects(pathProjects);

            Console.WriteLine("Memory Manager");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\ns.. settings\na..Add Project\nc..choose Project\nl..list all projects\nx..exit");
                string selected;
                selected = Console.ReadLine();
                switch (selected)
                {
                    case "s":
                        ChangeSettings(settings);
                        break;
                    case "a":
                        Console.WriteLine("Enter the project name");
                        projects.Add(new Project(Console.ReadLine(), true));
                        HandleProject(projects.Last());
                        break;
                    case "c":
                        PrintProjectList(projects);
                        Console.WriteLine("enter the index of the chosen project");
                        int index = Convert.ToInt32(Console.ReadLine());
                        HandleProject(projects[index]);
                        //TODO check cast
                        break;
                    case "d":
                        PrintProjectList(projects);
                        Console.WriteLine("enter the index of the project you want to delete");
                        projects.RemoveAt(Convert.ToInt32(Console.ReadLine()));
                        //TODO check cast
                        break;
                    case "l":
                        PrintProjectList(projects);
                        break;
                    case "x":
                        InExport.ExportSettings(settings);
                        InExport.ExportProjects(projects, pathProjects);
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("You have to enter s, a, c, l, x");
                        break;
                }
            }
        }


        private static void HandleProject(Project project)
        {


            string selected;
            bool exitProject = false;
            while (!exitProject)
            {
                Console.WriteLine(project.Name + " currently " + (project.Intern ? " intern" : " extern"));
                Console.WriteLine("a.. add directories to project");
                Console.WriteLine("l.. list all directories from project");
                Console.WriteLine("d.. delete directories from project");
                Console.WriteLine("c.. switch Intern/Extern");
                Console.WriteLine("x.. leave project");
                selected = Console.ReadLine();
                switch (selected)
                {
                    case "a":
                        Console.WriteLine("Enter the original directory:");
                        string orig = UserInputPath();
                        Console.WriteLine("Enter the external directory:");
                        string ext = UserInputPath();
                        project.AddDirectory(orig, ext);
                        break;
                    case "l":
                        project.ListAllDirectories();
                        break;
                    case "d":
                        project.RemoveDirectory();
                        break;
                    case "c":
                        project.SwitchInternExtern();
                        break;
                    case "x":
                        exitProject = true;
                        break;
                    default:
                        Console.WriteLine("You have to enter s, a, c or x");
                        break;
                }
            }
        }

        private static void PrintProjectList(List<Project> projects)
        {
            int i = 0;
            foreach (var p in projects)
            {
                Console.WriteLine(i + ": " + p.Name + (p.Intern ? " intern" : " extern"));
                i++;
            }
        }

        public static void ChangeSettings(Settings settings)
        {
            string selected;
            bool exitSettings = false;
            while (!exitSettings)
            {
                Console.WriteLine("\no.. change original path");
                Console.WriteLine("e.. change external path");
                selected = Console.ReadLine();
                switch (selected)
                {
                    case "o":
                        settings.OriginalPath = UserInputPath();
                        break;
                    case "e":
                        settings.ExternPath = UserInputPath();
                        break;
                    case "x":
                        exitSettings = true;
                        break;
                    default:
                        Console.WriteLine("You have to enter o, e, or x");
                        break;
                }
            }
        }

        private static string UserInputPath()
        {
            Console.WriteLine("Enter the Path");
            string path = Console.ReadLine();
            //TODO check if path is correct

            return path;
        }
    }


    //Check calling/Naming convention
}