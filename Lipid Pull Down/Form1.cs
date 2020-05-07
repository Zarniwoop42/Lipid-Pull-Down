using System;
using System.Windows.Forms;
using System.IO;

namespace Lipid_Pull_Down
{
    public partial class UserInterface : Form
    {
        public UserInterface()
        {
            InitializeComponent();
        }
        public bool CSVmode = true;

        private void uxBrowse_Click(object sender, EventArgs e)
        {
            if (CSVmode)
            {
                if (uxDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = uxDialog.FileName;
                    uxPath.Text = fileName;
                }
            }
            else
            {
                if (uxFDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = uxFDialog.SelectedPath;
                    uxPath.Text = fileName;
                }
            }
        }

        private void uxEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (CSVmode)
                    Program.parse(uxPath.Text, this);
                else
                    Compare.parse(uxPath.Text, this);

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton cur = (RadioButton)sender;
            string name = cur.Name;

            if (cur.Checked)
            {
                if (name == "uxPD")
                {
                    uxPathLabel.Text = "CSV path";
                    CSVmode = true;
                    if (uxPath.Text == "\\LipidScriptOutput") { uxPath.Text = ""; }
                }
                if (name == "uxCD")
                {
                    uxPathLabel.Text = "Folder path";
                    CSVmode = false;
                    if(uxPath.Text == "") { uxPath.Text = "\\LipidScriptOutput"; }
                }
            }
            docheckPath();
        }
        private void docheckPath()
        {
            if (CSVmode)
            {
                if (File.Exists(uxPath.Text))
                    uxEnter.Enabled = true;
                else
                    uxEnter.Enabled = false;
            }
            else
            {
                if (Directory.Exists(uxPath.Text))
                    uxEnter.Enabled = true;
                else
                    uxEnter.Enabled = false;
            }
        }
        private void checkPath(object sender, EventArgs e)
        {
            docheckPath();
        }
    }
}
