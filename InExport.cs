using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

namespace Memory_Manager
{
    public static class InExport
    {
        private static XDocument docSettings;

        public static Settings ImportSettings(string path)
        {
            Settings settings = new Settings();
            docSettings = XDocument.Load(path);
            
            //load standard settings
            if (docSettings != null)
            {
                settings.OriginalPath = docSettings.Root.Element("standardOriginalPath").Value;
                settings.ExternPath = docSettings.Root.Element("standardExternPath").Value;
                return settings;
            } else
            {
                //TODO why is else not active
                throw new InvalidDataException("The Setting.xml file is not correct");
            }
        }

        public static List<Project> ImportProjects(string pathProjects)
        {
            //set up Projects
            List<Project> projects = new List<Project>();
            XDocument docProjects = XDocument.Load(pathProjects);

            //load Projects
            if (docProjects != null)
            {
                foreach (XElement projectNode in docProjects.Descendants("project"))
                {
                    Project project = new Project(projectNode.Attribute("name").Value,
                        projectNode.Attribute("intern").Value.Equals("True"));
                    foreach (XElement dir in projectNode.Descendants("directories"))
                    {
                        project.AddDirectory(dir.Element("OriginalPath").Value, dir.Element("ExternPath").Value);
                    }
                    projects.Add(project);
                }
                return projects;
            }
            else
            {
                //TODO why is else not active
                throw new InvalidDataException("The projects.xml file is not correct");
            }

        }

        public static void ExportSettings(Settings settings)
        {
            docSettings.Root.Element("standardOriginalPath").Value = settings.OriginalPath;
            docSettings.Root.Element("standardExternPath").Value = settings.ExternPath;
        }
 
        public static void ExportProjects(List<Project> projects, string path)
        {
            XDocument docProjects = new XDocument();
            XElement xmlData = new XElement("projects",
                from proj in projects
                select new XElement("project",
                    new XAttribute("name", proj.Name),
                    new XAttribute("intern", proj.Intern.ToString()),
                    from dir in proj.Directories
                    select new XElement("directories",
                        new XElement("OriginalPath", dir.Item1),
                        new XElement("ExternPath", dir.Item2)
                    )
                )
            );
            docProjects.Add(xmlData);
            docProjects.Save(path);
        }
    }
}