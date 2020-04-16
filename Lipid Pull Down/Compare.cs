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

            Dictionary<string, StringBuilder> list = new Dictionary<string, StringBuilder>();

            foreach (string folder in Directory.GetDirectories(path))
            {
                foreach (string listfolder in Directory.GetDirectories(folder))
                {
                    string[] files = Directory.GetFiles(listfolder, "*.csv");
                    foreach (string csv in files)
                    {
                        using (TextFieldParser csvParser = new TextFieldParser(csv))
                        {
                            csvParser.CommentTokens = new string[] { "#" };
                            csvParser.SetDelimiters(new string[] { "," });
                            csvParser.HasFieldsEnclosedInQuotes = true;
                            csvParser.TrimWhiteSpace = true;
                            string[] line = csvParser.ReadFields();


                            bool t = true;
                            while (t)
                            {
                                foreach (string l in line)
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
                            if (headers.Contains("Compound.ID"))
                            {
                                int colIndex = Array.IndexOf(headers, "Compound.ID");

                                while (!csvParser.EndOfData)
                                {
                                    string[] fields = csvParser.ReadFields();
                                    string name = fields[colIndex];
                                    if(name == "") { continue; }
                                    string location = folder.Substring(folder.LastIndexOf("\\"));
                                    location += csv.Substring(csv.LastIndexOf("\\"));
                                    if (list.Keys.Contains(fields[colIndex]))
                                    {
                                        list[name] = list[name].Append(location + ",");
                                    }
                                    else
                                    {
                                        StringBuilder data = new StringBuilder();
                                        data.Append(name + ",");
                                        data.Append(location + ",");
                                        list[name] = data;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string output = path + "\\Analysis " + DateTime.Now.ToString("M.d.yy") + ".csv";

            // Check if folder already exists. If yes, add number.
            int run = 0;
            while (File.Exists(output))
            {
                run++;
                for (int c = output.LastIndexOf("Analysis"); c < output.Length; c++)
                {
                    if (output[c] == '.' && output[c + 1] == 'c' && output[c + 2] == 's' && output[c + 3] == 'v')
                    {
                        int num = c;
                        if (run > 1)
                            num = c - 3 - run.ToString().Length;
                        output = output.Remove(num);
                    }
                }
                output += " (" + run + ")" + ".csv";
            }

            using (StreamWriter swriter = new StreamWriter(output))
            {
                foreach(StringBuilder s in list.Values)
                swriter.WriteLine(s.ToString());
            }

            Console.WriteLine("Path to output: " + output);
            try
            {
                Process.Start(output);
            }
            catch (Exception f)
            {
                Console.WriteLine(f);
            }

        }
    }
}
