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
        private static XDocument docProjects;

        public static Settings ImportSettings(string path)
        {
            Settings settings = new Settings();
            //set up settings

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
            docProjects = XDocument.Load(pathProjects);

            //load Projects
            if (docProjects != null)
            {
                foreach (XElement projectNode in docProjects.Descendants("project"))
                {
                    Project project = new Project(projectNode.Attribute("name").Value,
                        projectNode.Attribute("intern").Value.Equals("True"));
                    foreach (XElement dir in projectNode.Descendants("directories"))
                    {
                        project.AddDirectory(dir.Descendants("OriginalPath").ToString(), dir.Descendants("ExternPath").ToString());
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
            //docProjects.Root.RemoveAll();
            //XmlDocument docProjects = new XmlDocument();
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
            
            
            
            /*XElement root = docProjects.CreateElement("projects");
            docProjects.AppendChild(root);
            foreach (Project project in projects)
            {
                XmlElement projectNode = docProjects.CreateElement("project");
                projectNode.SetAttribute("name", project.Name);
                projectNode.SetAttribute("intern", project.Intern.ToString());
                foreach (Tuple<string, string> t in project.Directories)
                {
                    XmlElement dirNode = docProjects.CreateElement("directories");
                    XmlElement origNode = docProjects.CreateElement("OriginalPath");
                    XmlElement extNode = docProjects.CreateElement("ExternPath");
                    origNode.InnerText = t.Item1;
                    extNode.InnerText = t.Item2;

                    dirNode.AppendChild(origNode);
                    dirNode.AppendChild(extNode);
                    projectNode.AppendChild(dirNode);
                }
                root.AppendChild(projectNode);
            }
            docProjects.AppendChild(root);
            docProjects.Save(path);*/
        }
    }
}