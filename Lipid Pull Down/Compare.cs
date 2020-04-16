using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;


namespace Lipid_Pull_Down
{
    static class Compare
    {
        public static void parse(string path, Form one) //main function. run once folder has been selected.
        {
            one.Hide();



            List<StringBuilder> stringbuilders = new List<StringBuilder>();

            foreach(string folder in Directory.GetDirectories(path))
            {
                string[] files = Directory.GetFiles(folder, "*.csv");
                foreach(string csv in files)
                {




                }
            }
        }
}
