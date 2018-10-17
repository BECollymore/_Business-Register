using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatsBR
{
    public class BRDataSource
    {
        public bool FileLoaded;
        public bool FileValidated;
        public DBController DBController;
        public ComponentController ComponentController;

        public BRDataSource()
        {
            throw new System.NotImplementedException();
        }
    
        private String FileName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool LoadFile()
        {
            throw new System.NotImplementedException();
        }

        public bool Update()
        {
            throw new System.NotImplementedException();
        }

        public bool Search(string pQuery)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidateFileStructure()
        {
            throw new System.NotImplementedException();
        }

        public Object GetDataSet()
        {
            throw new System.NotImplementedException();
        }
    }
}
