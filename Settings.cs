using System;
using System.IO;

namespace Memory_Manager
{
    public class Settings
    {
        private string standardOriginalPath = "";
        private string standardExternPath = "";

        public string OriginalPath 
        {
            set
            {
                standardOriginalPath = value;
            }
            get
            {
                return standardOriginalPath;
            }
        }

        public string ExternPath
        {
            set
            {
                standardExternPath = value;
            }
            get
            {
                return standardExternPath;
            }
        }
    }
}