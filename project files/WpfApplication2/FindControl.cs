using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatsBR
{
    class FindControl
    {
        FormCollection fc = null;
        Control c = null;
        Control f = null;

        //this method will return any form in the project based on the name...
        public Control TheForm(string name)
        {
           fc = Application.OpenForms;  

            for (int i =0; i< fc.Count; i++)
            {
                c = null;
                if(fc[i].Name == name)
                {
                    f = fc[i];
                    break;
                }
            }
            // a form is a control so it can cast to any form to a simple Control object...
            //example: Control x= TheForm("Form1"); i.e. the name of the form...
            return ((Control)f);
        }
        
        //this method/function locates the control to accesss on the form retured by the TheForm method...
        public Control Ctrl(Control f, string name)
        {
            if (f != null)
            {
                for (int i = 0; i < f.Controls.Count; i++)
                {
                    //look for the control by name...
                    if (f.Controls[i].Name == name)
                    {
                        c = f.Controls[i];
                        break;
                    }
                    //the control may be a container control on the form, look for it there...
                    if (c == null)
                    {
                        if (f.Controls[i].Controls.Count > 0)
                        {
                            Ctrl(f.Controls[i], name); //recursive call to function on level deeper serching...
                        }
                    }
                    //found the control, get out ...
                    if (c != null)
                    {
                        break;
                    }

                }
            }

            return c;
        }

    }
}
