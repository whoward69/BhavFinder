
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BhavFinder
{
    partial class BhavFinderConfig
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
            this.lblSims2Path = new System.Windows.Forms.Label();
            this.textSims2Path = new System.Windows.Forms.TextBox();
            this.btnSims2Select = new System.Windows.Forms.Button();
            this.lblSimPEPath = new System.Windows.Forms.Label();
            this.textSimPEPath = new System.Windows.Forms.TextBox();
            this.btnSimPESelect = new System.Windows.Forms.Button();
            this.btnConfigOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSims2Path
            // 
            this.lblSims2Path.AutoSize = true;
            this.lblSims2Path.Location = new System.Drawing.Point(12, 19);
            this.lblSims2Path.Name = "lblSims2Path";
            this.lblSims2Path.Size = new System.Drawing.Size(96, 13);
            this.lblSims2Path.TabIndex = 0;
            this.lblSims2Path.Text = "Sims 2 Install Path:";
            // 
            // textSims2Path
            // 
            this.textSims2Path.Location = new System.Drawing.Point(114, 16);
            this.textSims2Path.Name = "textSims2Path";
            this.textSims2Path.Size = new System.Drawing.Size(543, 20);
            this.textSims2Path.TabIndex = 1;
            // 
            // btnSims2Select
            // 
            this.btnSims2Select.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSims2Select.Location = new System.Drawing.Point(663, 12);
            this.btnSims2Select.Name = "btnSims2Select";
            this.btnSims2Select.Size = new System.Drawing.Size(125, 26);
            this.btnSims2Select.TabIndex = 2;
            this.btnSims2Select.Text = "Select Sims 2 Path";
            this.btnSims2Select.UseVisualStyleBackColor = true;
            this.btnSims2Select.Click += new System.EventHandler(this.OnSelectSim2PathClicked);
            // 
            // lblSimPEPath
            // 
            this.lblSimPEPath.AutoSize = true;
            this.lblSimPEPath.Enabled = false;
            this.lblSimPEPath.Location = new System.Drawing.Point(12, 56);
            this.lblSimPEPath.Name = "lblSimPEPath";
            this.lblSimPEPath.Size = new System.Drawing.Size(96, 13);
            this.lblSimPEPath.TabIndex = 3;
            this.lblSimPEPath.Text = "SimPE Install Path:";
            // 
            // textSimPEPath
            // 
            this.textSimPEPath.Enabled = false;
            this.textSimPEPath.Location = new System.Drawing.Point(114, 53);
            this.textSimPEPath.Name = "textSimPEPath";
            this.textSimPEPath.Size = new System.Drawing.Size(543, 20);
            this.textSimPEPath.TabIndex = 4;
            // 
            // btnSimPESelect
            // 
            this.btnSimPESelect.Enabled = false;
            this.btnSimPESelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimPESelect.Location = new System.Drawing.Point(663, 49);
            this.btnSimPESelect.Name = "btnSimPESelect";
            this.btnSimPESelect.Size = new System.Drawing.Size(125, 26);
            this.btnSimPESelect.TabIndex = 5;
            this.btnSimPESelect.Text = "Select SimPE Path";
            this.btnSimPESelect.UseVisualStyleBackColor = true;
            this.btnSimPESelect.Click += new System.EventHandler(this.OnSelectSimPEPathClicked);
            // 
            // btnConfigOK
            // 
            this.btnConfigOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigOK.Location = new System.Drawing.Point(663, 87);
            this.btnConfigOK.Name = "btnConfigOK";
            this.btnConfigOK.Size = new System.Drawing.Size(125, 26);
            this.btnConfigOK.TabIndex = 6;
            this.btnConfigOK.Text = "OK";
            this.btnConfigOK.UseVisualStyleBackColor = true;
            this.btnConfigOK.Click += new System.EventHandler(this.OnConfigOkClicked);
            // 
            // BhavFinderConfig
            // 
            this.AcceptButton = this.btnConfigOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 125);
            this.Controls.Add(this.lblSims2Path);
            this.Controls.Add(this.textSims2Path);
            this.Controls.Add(this.btnSims2Select);
            this.Controls.Add(this.lblSimPEPath);
            this.Controls.Add(this.textSimPEPath);
            this.Controls.Add(this.btnSimPESelect);
            this.Controls.Add(this.btnConfigOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BhavFinderConfig";
            this.Text = "BHAV Finder Configuration";
            this.Load += new System.EventHandler(this.OnConfigLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSims2Path;
        private System.Windows.Forms.TextBox textSims2Path;
        private System.Windows.Forms.Button btnSims2Select;
        private System.Windows.Forms.Label lblSimPEPath;
        private System.Windows.Forms.TextBox textSimPEPath;
        private System.Windows.Forms.Button btnSimPESelect;
        private System.Windows.Forms.Button btnConfigOK;
        private CommonOpenFileDialog selectPathDialog;
    }
}