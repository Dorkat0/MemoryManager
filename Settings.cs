using System;
using System.IO;

namespace Memory_Manager
{
    public class Settings
    {
        private string _standardOriginalPath;
        private string _standardExternPath;

        public string OriginalPath 
        {
            set
            {
                _standardOriginalPath = value;
            }
            get
            {
                return _standardExternPath;
            }
        }

        public string ExternPath
        {
            set
            {
                _standardExternPath = value;
            }
            get
            {
                return _standardExternPath;
            }
        }
    }
}