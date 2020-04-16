namespace Lipid_Pull_Down
{
    partial class UserInterface
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
            this.uxPath = new System.Windows.Forms.TextBox();
            this.uxPathLabel = new System.Windows.Forms.Label();
            this.uxBrowse = new System.Windows.Forms.Button();
            this.uxEnter = new System.Windows.Forms.Button();
            this.uxDialog = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.uxPD = new System.Windows.Forms.RadioButton();
            this.uxCD = new System.Windows.Forms.RadioButton();
            this.uxFDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // uxPath
            // 
            this.uxPath.Location = new System.Drawing.Point(13, 27);
            this.uxPath.Name = "uxPath";
            this.uxPath.Size = new System.Drawing.Size(252, 20);
            this.uxPath.TabIndex = 0;
            this.uxPath.TextChanged += new System.EventHandler(this.checkPath);
            // 
            // uxPathLabel
            // 
            this.uxPathLabel.AutoSize = true;
            this.uxPathLabel.Location = new System.Drawing.Point(12, 11);
            this.uxPathLabel.Name = "uxPathLabel";
            this.uxPathLabel.Size = new System.Drawing.Size(52, 13);
            this.uxPathLabel.TabIndex = 1;
            this.uxPathLabel.Text = "CSV path";
            // 
            // uxBrowse
            // 
            this.uxBrowse.Location = new System.Drawing.Point(271, 27);
            this.uxBrowse.Name = "uxBrowse";
            this.uxBrowse.Size = new System.Drawing.Size(57, 20);
            this.uxBrowse.TabIndex = 2;
            this.uxBrowse.Text = "^";
            this.uxBrowse.UseVisualStyleBackColor = true;
            this.uxBrowse.Click += new System.EventHandler(this.uxBrowse_Click);
            // 
            // uxEnter
            // 
            this.uxEnter.Enabled = false;
            this.uxEnter.Location = new System.Drawing.Point(12, 53);
            this.uxEnter.Name = "uxEnter";
            this.uxEnter.Size = new System.Drawing.Size(316, 23);
            this.uxEnter.TabIndex = 3;
            this.uxEnter.Text = "Enter";
            this.uxEnter.UseVisualStyleBackColor = true;
            this.uxEnter.Click += new System.EventHandler(this.uxEnter_Click);
            // 
            // uxDialog
            // 
            this.uxDialog.Filter = "CSV Files|*.CSV|All files|*.*";
            // 
            // uxPD
            // 
            this.uxPD.AutoSize = true;
            this.uxPD.Checked = true;
            this.uxPD.Location = new System.Drawing.Point(94, 4);
            this.uxPD.Name = "uxPD";
            this.uxPD.Size = new System.Drawing.Size(132, 17);
            this.uxPD.TabIndex = 4;
            this.uxPD.TabStop = true;
            this.uxPD.Text = "Analyze Pulldown CSV";
            this.uxPD.UseVisualStyleBackColor = true;
            this.uxPD.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // uxCD
            // 
            this.uxCD.AutoSize = true;
            this.uxCD.Location = new System.Drawing.Point(235, 4);
            this.uxCD.Name = "uxCD";
            this.uxCD.Size = new System.Drawing.Size(93, 17);
            this.uxCD.TabIndex = 5;
            this.uxCD.TabStop = true;
            this.uxCD.Text = "Compare Data";
            this.uxCD.UseVisualStyleBackColor = true;
            this.uxCD.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // UserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 82);
            this.Controls.Add(this.uxCD);
            this.Controls.Add(this.uxPD);
            this.Controls.Add(this.uxEnter);
            this.Controls.Add(this.uxBrowse);
            this.Controls.Add(this.uxPathLabel);
            this.Controls.Add(this.uxPath);
            this.Name = "UserInterface";
            this.Text = "Pull Down";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox uxPath;
        private System.Windows.Forms.Label uxPathLabel;
        private System.Windows.Forms.Button uxBrowse;
        private System.Windows.Forms.Button uxEnter;
        private System.Windows.Forms.OpenFileDialog uxDialog;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RadioButton uxPD;
        private System.Windows.Forms.RadioButton uxCD;
        private System.Windows.Forms.FolderBrowserDialog uxFDialog;
    }
}

