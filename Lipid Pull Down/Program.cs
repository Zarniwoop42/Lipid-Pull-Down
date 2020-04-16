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
    static class Program
    {
        static Program()
        {
            CosturaUtility.Initialize();
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UserInterface());
        }

        private static void RunRScript(string rpath, string scriptpath) //Runs the R script which the code generates to create the Venn Diagram.
        {
            try
            {
                scriptpath = PathExtensions.GetShortPathName(scriptpath);

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = rpath;
                info.WorkingDirectory = Path.GetDirectoryName(scriptpath);
                info.Arguments = scriptpath;
                info.RedirectStandardOutput = true;
                info.CreateNoWindow = true;
                info.UseShellExecute = false;

                using (var proc = new Process { StartInfo = info })
                {
                    proc.Start();
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static string rpathfunc = "";
        public static void parse(string path, Form one) //main function. run once csv has been selected.
        {
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.TrimWhiteSpace = true;
                string[] line = csvParser.ReadFields();


                bool t = true;
                while (t) {
                    foreach(string l in line)
                    {
                        if (l != "")
                        {
                            t = false;
                            break;
                        }
                    }
                    if (t) { line = csvParser.ReadFields(); } 
                } //skip to first line with data

                string[] headers = line;
                foreach(string h in headers)
                     headers[Array.IndexOf(headers, h)] = h.Replace("?", "D").Replace(" ", "_");

                int n = 0;
                for (int i = 0; i < headers.Length; i++)
                {
                    if (headers[i] != "") { n++; }
                }

                string[] headersStripped = new string[n]; //get list of just headers, no blanks
                int j = 0;
                for (int i = 0; i < headers.Length; i++)
                {
                    if (headers[i] != "")
                    {
                        headersStripped[j] = headers[i];
                        j++;
                    }
                }

                int dataTagsNum = 0;
                for (int i = 0; i < headers.Length; i++)
                {
                    if (headers[i] != "")
                    {
                        dataTagsNum = i;
                        break;
                    }
                }
                int experimentsNum = 0;
                for (int i = dataTagsNum + 1; i < headers.Length; i++)
                {
                    if (headers[i] != "")
                    {
                        experimentsNum = i - dataTagsNum;
                        break;
                    }
                }

                string[] fields = csvParser.ReadFields();
       
                one.Hide();
                    
                string[] magConGroup = getSpecifics(headersStripped);
                string[] magCon = new string[Array.IndexOf(magConGroup, "Marker")];
                Array.Copy(magConGroup, magCon, Array.IndexOf(magConGroup, "Marker"));
                string[] groups = new string[magConGroup.Length - magCon.Length - 1];
                for(int i = Array.IndexOf(magConGroup, "Marker") + 1; i < magConGroup.Length; i++)
                {
                    groups[i - (1 + Array.IndexOf(magConGroup, "Marker"))] = magConGroup[i];
                }

                //set up txt file
                string primaryCSV = path.Remove(path.LastIndexOf("\\")) + "/LipidScriptOutput";
                if (!Directory.Exists(primaryCSV))
                {
                    System.IO.Directory.CreateDirectory(primaryCSV);
                }
                primaryCSV += "/ (Mag " + magCon[0] + ")";

                // Check if folder already exists. If yes, add number.
                int run = 0;
                while (Directory.Exists(primaryCSV))
                {
                    run++;
                    for (int c = primaryCSV.LastIndexOf("(Mag") + 1; c < primaryCSV.Length; c++)
                    {
                        if (primaryCSV[c] == '(')
                        {
                            primaryCSV = primaryCSV.Remove(c - 1);
                        }
                    }
                    primaryCSV += " (" + run + ")";
                }

                System.IO.Directory.CreateDirectory(primaryCSV);

                System.IO.Directory.CreateDirectory(primaryCSV + "/CSV Lists " + DateTime.Now.ToString("M.d.yy"));
                string csvHeader = "/Raw-R-Venn-Diagram-Data-" + DateTime.Now.ToString("M.d.yy") + ".csv";
                string outputFolder = primaryCSV;
                primaryCSV += csvHeader;

                // Check if file already exists. If yes, delete it.     
                if (File.Exists(primaryCSV))
                {
                    File.Delete(primaryCSV);
                }

                StringBuilder sb = new StringBuilder();

                string[] datacats = new string[dataTagsNum];
                for (int i = 0; i < dataTagsNum; i++)
                {
                    sb.Append(fields[i] + ",");
                    datacats[i] = fields[i];
                }
                foreach (string field in groups)
                {
                    if(!magCon.Contains(field)){
                        sb.Append(field + ",");
                    }
                }
                sb.AppendLine();

                //Generate all permutations of venn categories
                int combos = 0;
                for (int mask = 0; mask < 1 << (groups.Length); mask++)
                {
                    combos++;
                }
                string[] categories = new string[combos - 1];
                for (int mask = 0; mask < 1 << (groups.Length); mask++)
                {
                    string permutation = "";
                    for (int i = 0; i < groups.Length; i++)
                    {
                        if ((mask & (1 << (groups.Length - 1 - i))) != 0)
                        {
                            permutation += groups[i] + "+";
                        }
                    }
                    if(mask == 0) { continue; }
                    permutation = permutation.TrimEnd('+');
                    categories[mask - 1] = permutation;
                }

                string[] controls = new string[magCon.Length - 1]; Array.Copy(magCon, 1, controls, 0, magCon.Length - 1);
                double[] conAvg = new double[controls.Length];

                List<StringBuilder> stringbuilders = new List<StringBuilder>(combos - 1);
                foreach(string cat in categories)
                {
                    StringBuilder sbp = new StringBuilder();
                    foreach(string s in datacats)
                    {
                        sbp.Append(s + ",");
                    }

                    foreach (string con in controls)
                    {
                        sbp.Append(con + " Control Value,");
                    }

                    string[] catheads = cat.Split('+');

                    foreach(string cathead in catheads)
                    {
                        sbp.Append(cathead + " Value,");
                        foreach (string con in controls)
                        {
                            sbp.Append(cathead + " / " + con + " Magnitude,");
                        }
                    }
                    sbp.AppendLine();
                    stringbuilders.Add(sbp);
                }

                while (!csvParser.EndOfData) //begin parsing each line
                {
                    string fullname = "";
                    fields = csvParser.ReadFields();
                    for (int i = 0; i < dataTagsNum; i++)
                    {
                        sb.Append(fields[i] + ",");
                        fullname += fields[i] + ",";
                    }
                    int index = 0;
                    foreach (string con in controls)
                    {
                        double avg = 0; double num = 0;
                        for (int di = Array.IndexOf(headers, con); di < experimentsNum + Array.IndexOf(headers, con); di++)
                        {
                            if (fields[di] != "")
                            {
                                try
                                {
                                    avg += Convert.ToDouble(fields[di]);
                                }catch(Exception e)
                                {
                                    MessageBox.Show(e.ToString());
                                }
                                    num++;
                            }
                        }
                        if (num != 0) { avg /= num; }
                        conAvg[index] = avg; index++;
                        fullname += avg + ",";
                    }
                    string ingroup = "";
                    foreach(string group in groups) 
                    {
                        if (controls.Contains(group)) { continue; }

                        double avg = 0; double num = 0;
                        for(int di = Array.IndexOf(headers, group); di < experimentsNum + Array.IndexOf(headers, group); di++)
                        {
                            if (fields[di] != "")
                            {
                                avg += Convert.ToDouble(fields[di]);
                                num++;
                            }
                        }
                        if (num != 0) { avg /= num; }

                        string tempmag = ""; double[] storeavg = new double[conAvg.Length]; int count = 0;
                        foreach(double ca in conAvg)
                        {
                            double ca2 = ca;
                            if (ca == 0) { ca2 = 1; }
                            tempmag += avg / ca2 +",";
                            storeavg[count] = avg / ca2;
                            count++;
                        }
                        double davg = storeavg.Min();
                        sb.Append(davg + ",");
                        if(davg >= Convert.ToDouble(magCon[0]))
                        {
                            ingroup += group + "+";
                            fullname += avg + ",";
                            fullname += tempmag;
                        }
                        
                    }
                    ingroup = ingroup.TrimEnd('+');

                    //match ingroup string to categories[]
                    foreach(string category in categories)
                    {
                        if(ingroup == category)
                        {
                            stringbuilders[Array.IndexOf(categories, category)].Append(fullname);
                            stringbuilders[Array.IndexOf(categories, category)].AppendLine();
                        }
                    }
                    sb.AppendLine();
                }
                //Create primary CSV
                try
                {
                    using (StreamWriter sw = File.CreateText(primaryCSV))
                    {
                        sw.Write(sb.ToString());
                    }

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
                //Create secondary CSVs
                try
                {
                    for (int i = 0; i<categories.Length; i++)
                    {
                        string currentString = stringbuilders[i].ToString(); //check if CSVlist has clusters, otherwise do not create.
                        int newlineCount = 0; int index = -1;
                        while (-1 != (index = currentString.IndexOf(Environment.NewLine, index + 1)))
                        {
                            newlineCount++;
                            if (newlineCount > 1) { break; }
                        }

                        if (newlineCount < 2) { continue; }

                        string CsvlistPath = outputFolder + "\\CSV Lists " + DateTime.Now.ToString("M.d.yy");

                        CsvlistPath += "\\" + categories[i] + ".csv";

                        using (StreamWriter sw = File.CreateText(CsvlistPath))
                        {
                            sw.Write(stringbuilders[i].ToString());
                        }
                    }

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }

                //generate R script
                string RscriptPath = outputFolder + "\\RScript -" + DateTime.Now.ToString("M.d.yy") + ".R";

                // Check if file already exists. If yes, delete it.     
                if (File.Exists(RscriptPath))
                {
                    File.Delete(RscriptPath);
                }
                try
                {
                    using (StreamWriter sw = File.CreateText(RscriptPath))
                    {
                        sw.WriteLine("library(limma)");
                        string pathformat = RscriptPath.Remove(RscriptPath.LastIndexOf("\\"));
                        pathformat = pathformat.Replace("\\", "/");
                        sw.WriteLine("excelsheet <- read.csv(\"" + pathformat + csvHeader + "\")");
                        string cbind = "c2 <- cbind(";
                        foreach (string g in groups)
                        {
                            sw.WriteLine(g + " <- (excelsheet$" + g + " >= " + magCon[0] + ")");
                            cbind += g + ", ";
                        }
                        cbind = cbind.Substring(0, cbind.Length-2);
                        sw.WriteLine(cbind+")");
                        sw.WriteLine("v2 <- vennCounts(c2)");
                        sw.WriteLine("vennDiagram(v2)");
                    }

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
                try
                {
                    string programFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
                    string programfiles = programFiles + "\\R";
                    string[] paths = Directory.GetDirectories(programfiles, "R*", System.IO.SearchOption.TopDirectoryOnly);
                    string binpath = paths[0] + @"\bin";

                    var rpath = Directory.EnumerateFiles(binpath, "*Rscript.exe", System.IO.SearchOption.AllDirectories);

                    string rpath2;

                    if (rpath.FirstOrDefault() == null)
                    {
                        requestRpath();
                        rpath2 = rpathfunc;
                    }
                    else
                    {
                       rpath2 = rpath.FirstOrDefault();
                    }

                    var scriptpath = RscriptPath;
                    RunRScript(rpath2, scriptpath);
                }
                catch (Exception ex)
                {
                    requestRpath();
                    string rpath2 = rpathfunc;
                    var scriptpath = RscriptPath;
                    RunRScript(rpath2, scriptpath);
                }

                
                Console.WriteLine("Path to output: " + outputFolder);
                try
                {
                    Process.Start(outputFolder);
                }
                catch(Exception f)
                {
                    Console.WriteLine(f);
                }
                
            }
        }


        public static void requestRpath()
        {
            requestRpath rform = new requestRpath();
            rform.ShowDialog();
        }

        public static void result(string text, Form three)
        {
            rpathfunc = text;
            three.Hide();
        }

        public static bool conC = false;
        public static bool groC = false;
        public static bool magC = false;

        public static void enterCheck(Button ok)
        {
            if (conC && groC && magC) {
                ok.Enabled = true;
            }
            else {
                ok.Enabled = false;
            }
        }

        public static string[] getSpecifics(string[] headers) //create secondary form from csv to determine controls and magnitude
        {
            Form2 specifics = new Form2();

            Button ok = new Button();

            Label control = new Label();
            control.Text = "Select Control Groups";
            control.Left = 10;
            control.AutoSize = true;
            control.Top = 5;
            specifics.Controls.Add(control);

            int opencount = 0;

            int l = 15; int top = 25;
            for (int i = 0; i < headers.Length; i++)
            {
                CheckBox r = new CheckBox();
                r.Name = i + "Control";
                r.Text = headers[i];
                r.AutoSize = true;
                r.Left = 10;
                r.Top = top;
                r.Height = 10;

                r.Click += (object sender, EventArgs e) =>
                {
                    CheckBox cur = (CheckBox)sender;
                    string name = cur.Name;
                    int num = Convert.ToInt32(Convert.ToString(name[0]));
                    string type = string.Concat(name.Where(char.IsLetter));

                    if (cur.Checked)
                    {
                        foreach (Control c in specifics.Controls)
                            if (c is CheckBox)
                                if (((CheckBox)c).Name == Convert.ToString(num) + "Group")
                                {
                                    ((CheckBox)c).Checked = false;
                                    ((CheckBox)c).Enabled = false;
                                }
                    }
                    else
                    {
                        foreach (Control c in specifics.Controls)
                            if (c is CheckBox)
                                if (((CheckBox)c).Name == Convert.ToString(num) + "Group" && opencount < 5)
                                {
                                    ((CheckBox)c).Enabled = true;
                                }
                    }
                    bool checkcon = false;
                    foreach (Control c in specifics.Controls)
                        if (c is CheckBox)
                        {
                            if (((CheckBox)c).Name.Contains("Control") && ((CheckBox)c).Checked)
                            {
                                checkcon = true;
                            }
                        }
                    if (checkcon) { conC = true; } else { conC = false; }
                    enterCheck(ok);
                };

                top += r.Height + 10;
                specifics.Controls.Add(r);

                if (headers[i].Length > l) { l = headers[i].Length; }

            }

            Label group = new Label();
            group.Text = "Select Groups To Analyze";
            group.Left = 10;
            group.AutoSize = true;
            group.Top = top + 5;
            specifics.Controls.Add(group);

            top += 20; 
            for (int i = 0; i < headers.Length; i++)
            {
                CheckBox r = new CheckBox();
                r.Name = i + "Group";
                r.Text = headers[i];
                r.AutoSize = true;
                r.Left = 10;
                r.Top = top;
                r.Height = 10;

                r.Click += (object sender, EventArgs e) =>
                {
                    CheckBox cur = (CheckBox)sender;
                    string name = cur.Name;
                    int num = Convert.ToInt32(Convert.ToString(name[0]));
                    string type = string.Concat(name.Where(char.IsLetter));
                    if (cur.Checked)
                    {
                        opencount++;
                        if (opencount == 5)
                        {
                            foreach (Control c in specifics.Controls)
                                if (c is CheckBox)
                                    if (((CheckBox)c).Name.Contains("Group") && ((CheckBox)c).Checked == false)
                                    {
                                        ((CheckBox)c).Enabled = false;
                                    }
                        }
                        foreach (Control c in specifics.Controls)
                            if (c is CheckBox)
                                if (((CheckBox)c).Name == Convert.ToString(num) + "Control")
                                {
                                    ((CheckBox)c).Checked = false;
                                    ((CheckBox)c).Enabled = false;
                                }
                    }
                    else
                    {
                        opencount--;
                        if (opencount < 5)
                        {
                            foreach (Control c in specifics.Controls)
                            {
                                if (c is CheckBox && ((CheckBox)c).Name.Contains("Group"))
                                {
                                    string name2 = c.Name;
                                    int num2 = Convert.ToInt32(Convert.ToString(name2[0]));
                                    foreach (Control c2 in specifics.Controls)
                                    {
                                        if (c2 is CheckBox)
                                        {
                                            if (((CheckBox)c2).Name == Convert.ToString(num2) + "Control" && ((CheckBox)c2).Checked == false)
                                            {
                                                ((CheckBox)c).Enabled = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        foreach (Control c in specifics.Controls)
                            if (c is CheckBox)
                                if (((CheckBox)c).Name == Convert.ToString(num) + "Control")
                                {
                                    ((CheckBox)c).Enabled = true;
                                }
                    }

                    bool checkgro = false;
                    foreach (Control c in specifics.Controls)
                        if (c is CheckBox)
                            if (c is CheckBox)
                            {
                                if (((CheckBox)c).Name.Contains("Group") && ((CheckBox)c).Checked)
                                {
                                    checkgro = true;
                                }
                            }
                    if (checkgro) { groC = true; } else { groC = false; }

                    enterCheck(ok);

                };

                top += r.Height + 10;
                specifics.Controls.Add(r);
            }

            top += 7;

            specifics.Width = l * 10;
            specifics.Height = headers.Length * 50 + 50;

            Label maglabel = new Label();
            maglabel.Text = "Enter Magnitude";
            maglabel.Left = 10;
            maglabel.AutoSize = true;
            maglabel.Top = top - 5;
            specifics.Controls.Add(maglabel);

            top += 5;

            TextBox mag = new TextBox();
            mag.Text = "";
            mag.Top = top + 5;
            mag.Left = 10;
            mag.Width = 70;
            mag.KeyUp += (object sender, KeyEventArgs e) =>
            {
                try
                {
                    Convert.ToDouble(mag.Text);
                    if (mag.Text != "")
                    {
                        magC = true;
                    }
                    else
                    {
                        magC = false;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Enter only numbers.");
                    magC = false;
                }
                enterCheck(ok); 
            };
                specifics.Controls.Add(mag);
            
            string[] check = new string[0];
            ok.Text = "Enter";
            ok.Top = top + 4;
            ok.Left = 84;
            ok.Width = 40;
            ok.Enabled = false;
            ok.Click += (object sender, EventArgs e) =>
            {
                specifics.Hide();
                check = new string[specifics.Controls.Count * 2 + 2]; check[0] = mag.Text;
                int ind = 0;
                foreach (Control c in specifics.Controls)
                {
                    if (c is CheckBox)
                    {
                        if (((CheckBox)c).Checked && ((CheckBox)c).Name.Contains("Control"))
                        {
                            ind++;
                            check[ind] = c.Text;
                        }
                    }
                }
                ind++; check[ind] = "Marker";
                foreach (Control c in specifics.Controls)
                {
                    if (c is CheckBox)
                    {
                        if (((CheckBox)c).Checked && ((CheckBox)c).Name.Contains("Group"))
                        {
                            ind++;
                            check[ind] = c.Text;
                        }
                    }
                }
            };

           

            specifics.Controls.Add(ok);

            specifics.ShowDialog();

            int count = 0;
            for(int i = 0; i < check.Length; i++)
            {
                if(check[i] != null)
                {
                    count++;
                }
            }
            string[] ret = new string[count];
            for (int i = 0; i < check.Length; i++)
            {
                if (check[i] != null)
                {
                    ret[i] = check[i];
                }
            }
            return ret;
        }
    }
}
