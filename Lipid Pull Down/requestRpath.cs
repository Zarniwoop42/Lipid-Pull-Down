using System;
using System.Windows.Forms;

namespace Lipid_Pull_Down
{
    public partial class requestRpath : Form
    {
        public requestRpath()
        {
            InitializeComponent();
        }
        private void uxBrowse_Click(object sender, EventArgs e)
        {
            if (uxDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = uxDialog.FileName;
                uxPath.Text = fileName;

            }
        }

        private void uxEnter_Click(object sender, EventArgs e)
        {
            Program.result(uxPath.Text, this);
        }
    }
}
