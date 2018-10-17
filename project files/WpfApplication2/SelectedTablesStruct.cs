using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsBR
{
    public struct SelectedTable
    {
       public string Schema;
       public string TableName;
        public SelectedTable(string p1, string p2)
        {
            Schema = p1;
            TableName = p2;
        }
    }
}
