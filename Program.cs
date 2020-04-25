using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Memory_Manager
{
    internal static class Program
    {
        //storage of the project and the settings
        private static readonly string PathSettings =
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
            "./Data/settings.xml";

        private static readonly string PathProjects =
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
            "./Data/projects.xml";

        public static void Main(string[] args)
        {
            //import settings and projects
            Settings settings = InExport.ImportSettings(PathSettings);
            List<Project> projects = InExport.ImportProjects(PathProjects);

            Console.WriteLine("Memory Manager");

            //-----------Main UI---------------
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\ns.. settings\na..add project\nc..choose project\nl..list all projects\nx..exit");
                switch (Console.ReadLine()) //loop until x is pressed
                {
                    case "s": //settings
                        ChangeSettings(settings);
                        break;
                    case "a": //add project
                        Console.WriteLine("Enter the project name");
                        projects.Add(new Project(Console.ReadLine(), true));
                        HandleProject(projects.Last());
                        break;
                    case "c": //chose project
                        PrintProjectList(projects);
                        Console.WriteLine("enter the index of the chosen project");
                        HandleProject(projects[CheckIntInput()]);
                        break;
                    case "d": //delete project
                        PrintProjectList(projects);
                        Console.WriteLine("enter the index of the project you want to delete");
                        projects.RemoveAt(CheckIntInput());
                        break;
                    case "l": //list projects
                        PrintProjectList(projects);
                        break;
                    case "x": //save an exit programm
                        InExport.ExportSettings(settings);
                        InExport.ExportProjects(projects, PathProjects);
                        exit = true;
                        break;
                    default: //wrong input
                        Console.WriteLine("You have to enter s, a, c, l, x");
                        break;
                }
            }
        }


        private static void HandleProject(Project project) //chose project
        {
            bool exitProject = false;
            while (!exitProject) //loop until x is pressed
            {
                Console.WriteLine(project.Name + " currently " + (project.Intern ? " intern" : " extern")+
                        "\na.. add directories to project\nl.. list all directories from project" +
                        "\nd.. delete directories from project\nc.. switch Intern/Extern\nx.. leave project"
                    );

                switch (Console.ReadLine())
                {
                    case "a":        //add dir
                        Console.WriteLine("Enter the original directory:");
                        string orig = UserInputPath();
                        Console.WriteLine("Enter the external directory:");
                        project.AddDirectory(orig, UserInputPath());
                        break;
                    case "l":    //list dir
                        project.PrintsAllDirectories();
                        break;
                    case "d":    //delete dir
                        project.RemoveDirectory();
                        break;
                    case "c":    //chose dir
                        project.SwitchInternExtern();
                        break;
                    case "x":    //exit
                        exitProject = true;
                        break;
                    default:    //wrong input
                        Console.WriteLine("You have to enter s, a, c or x");
                        break;
                }
            }
        }

        private static void PrintProjectList(List<Project> projects)        //prints the projects with "index"
        {
            int i = 0;
            foreach (var p in projects)
            {
                Console.WriteLine(i + ": " + p.Name + (p.Intern ? " intern" : " extern"));
                i++;
            }
        }

        private static void ChangeSettings(Settings settings)        //change settings in the class (not in the xml file)
        {
            bool exitSettings = false;
            while (!exitSettings)
            {
                Console.WriteLine("\no.. change original path");
                Console.WriteLine("e.. change external path");
                switch (Console.ReadLine())
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

        private static string UserInputPath()            //takes the path and validates it
        {
            Console.WriteLine("Enter the Path");
            string path = Console.ReadLine();
            while (!Directory.Exists(path))
            {
                Console.WriteLine("The path is not valid, please enter another path:");
                path = Console.ReadLine();
            }
            return path;
        }

        private static int CheckIntInput()        //tries to cast the user input into an int
        {
            while (true)    //TODO not ideal
            {
                try
                {
                    return Convert.ToInt32(Console.ReadLine());        
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine("please enter an valid integer:");
                }
            }
        }
    }
}