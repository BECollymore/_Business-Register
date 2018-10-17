using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;



namespace StatsBR
{
    class TelerikRadControls
    {

        //private StatsBR.BRDataClassesDataContext
        

         public IEnumerable<Business> GetBusinessData()
        {

            BusinessRegisterEntities dbContent = new BusinessRegisterEntities();
            
            var query = from CoreBusinessData in dbContent.Businesses
                        select CoreBusinessData;

            IEnumerable<Business> pdata = query;
            return pdata; 

        }
        


    }
}
