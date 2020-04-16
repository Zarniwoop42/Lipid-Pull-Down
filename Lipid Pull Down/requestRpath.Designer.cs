namespace Lipid_Pull_Down
{
    partial class requestRpath
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uxEnter = new System.Windows.Forms.Button();
            this.uxBrowse = new System.Windows.Forms.Button();
            this.uxPathLabel = new System.Windows.Forms.Label();
            this.uxPath = new System.Windows.Forms.TextBox();
            this.uxDialog = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // uxEnter
            // 
            this.uxEnter.Location = new System.Drawing.Point(31, 49);
            this.uxEnter.Name = "uxEnter";
            this.uxEnter.Size = new System.Drawing.Size(316, 23);
            this.uxEnter.TabIndex = 7;
            this.uxEnter.Text = "Enter";
            this.uxEnter.UseVisualStyleBackColor = true;
            this.uxEnter.Click += new System.EventHandler(this.uxEnter_Click);
            // 
            // uxBrowse
            // 
            this.uxBrowse.Location = new System.Drawing.Point(290, 23);
            this.uxBrowse.Name = "uxBrowse";
            this.uxBrowse.Size = new System.Drawing.Size(57, 20);
            this.uxBrowse.TabIndex = 6;
            this.uxBrowse.Text = "^";
            this.uxBrowse.UseVisualStyleBackColor = true;
            this.uxBrowse.Click += new System.EventHandler(this.uxBrowse_Click);
            // 
            // uxPathLabel
            // 
            this.uxPathLabel.AutoSize = true;
            this.uxPathLabel.Location = new System.Drawing.Point(31, 7);
            this.uxPathLabel.Name = "uxPathLabel";
            this.uxPathLabel.Size = new System.Drawing.Size(238, 13);
            this.uxPathLabel.TabIndex = 5;
            this.uxPathLabel.Text = "Path to Rscript.exe (R folder -> R-[version] -> bin)";
            // 
            // uxPath
            // 
            this.uxPath.Location = new System.Drawing.Point(32, 23);
            this.uxPath.Name = "uxPath";
            this.uxPath.Size = new System.Drawing.Size(252, 20);
            this.uxPath.TabIndex = 4;
            // 
            // uxDialog
            // 
            this.uxDialog.Filter = "EXE Files|*.EXE|All files|*.*";
            // 
            // requestRpath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 78);
            this.Controls.Add(this.uxEnter);
            this.Controls.Add(this.uxBrowse);
            this.Controls.Add(this.uxPathLabel);
            this.Controls.Add(this.uxPath);
            this.Name = "requestRpath";
            this.Text = "requestRpath";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxEnter;
        private System.Windows.Forms.Button uxBrowse;
        private System.Windows.Forms.Label uxPathLabel;
        private System.Windows.Forms.TextBox uxPath;
        private System.Windows.Forms.OpenFileDialog uxDialog;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}