using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Memory_Manager
{
    public static class InExport
    {

        
        private static XmlDocument docSettings = new XmlDocument();
        private static XmlDocument docProjects = new XmlDocument();

        public static Settings ImportSettings(string path)
        {
            Settings settings = new Settings();
            //set up settings

            docSettings.Load(path);

            //load standard settings
            if (docSettings.DocumentElement != null)
            {
                settings.OriginalPath = docSettings.DocumentElement.SelectSingleNode("standardOriginalPath")?.InnerText;
                settings.OriginalPath = docSettings.DocumentElement.SelectSingleNode("standardExternPath")?.InnerText;
                return settings;
            }
            else
            {
                throw new InvalidDataException("The Setting.xml file is not correct");
            }
        }

        public static List<Project> ImportProjects(string pathProjects)
        {
            //set up Projects
            List<Project> projects = new List<Project>();
            docProjects.Load(pathProjects);

            //load Projects
            if (docProjects.DocumentElement != null)
            {
                foreach (XmlNode projectNode in docProjects.DocumentElement.ChildNodes)
                {
                    Project project = new Project(projectNode.Attributes["name"].Value, projectNode.Attributes["intern"].Value.Equals("True"));
                    foreach (XmlNode dir in projectNode.ChildNodes)
                    {
                        project.AddDirectory(
                            dir.SelectSingleNode("OriginalPath")?.InnerText,
                            dir.SelectSingleNode("ExternPath")?.InnerText);
                    }

                    projects.Add(project);
                }

                return projects;
            }
            else
            {
                throw new InvalidDataException("The projects.xml file is not correct");
            }

        }

        public static void ExportSettings(Settings settings)
        {
            docSettings.DocumentElement.SelectSingleNode("standardOriginalPath").InnerText = settings.OriginalPath;
            docSettings.DocumentElement.SelectSingleNode("standardExternPath").InnerText = settings.ExternPath;
        }

        public static void ExportProjects(List<Project> projects, string path)
        {
            docProjects.RemoveAll();
            //XmlDocument docProjects = new XmlDocument();
            XmlElement root = docProjects.CreateElement("projects");
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
            docProjects.Save(path);
        }
    }
}