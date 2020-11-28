
namespace BhavFinder
{
    partial class BhavFinderAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.tbCopyright = new System.Windows.Forms.TextBox();
            this.tbRights = new System.Windows.Forms.TextBox();
            this.btnAboutOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbProduct
            // 
            this.tbProduct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbProduct.Location = new System.Drawing.Point(12, 12);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.ReadOnly = true;
            this.tbProduct.Size = new System.Drawing.Size(260, 13);
            this.tbProduct.TabIndex = 0;
            this.tbProduct.TabStop = false;
            this.tbProduct.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbCopyright
            // 
            this.tbCopyright.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCopyright.Location = new System.Drawing.Point(12, 52);
            this.tbCopyright.Name = "tbCopyright";
            this.tbCopyright.ReadOnly = true;
            this.tbCopyright.Size = new System.Drawing.Size(260, 13);
            this.tbCopyright.TabIndex = 0;
            this.tbCopyright.TabStop = false;
            this.tbCopyright.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbRights
            // 
            this.tbRights.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbRights.Location = new System.Drawing.Point(12, 77);
            this.tbRights.Name = "tbRights";
            this.tbRights.ReadOnly = true;
            this.tbRights.Size = new System.Drawing.Size(260, 13);
            this.tbRights.TabIndex = 0;
            this.tbRights.TabStop = false;
            this.tbRights.Text = "All Rights Reserved";
            this.tbRights.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAboutOK
            // 
            this.btnAboutOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAboutOK.Location = new System.Drawing.Point(80, 120);
            this.btnAboutOK.Name = "btnAboutOK";
            this.btnAboutOK.Size = new System.Drawing.Size(125, 26);
            this.btnAboutOK.TabIndex = 0;
            this.btnAboutOK.Text = "OK";
            this.btnAboutOK.UseVisualStyleBackColor = true;
            this.btnAboutOK.Click += new System.EventHandler(this.OnAboutOkClicked);
            // 
            // BhavFinderAbout
            // 
            this.AcceptButton = this.btnAboutOK;
            this.ClientSize = new System.Drawing.Size(285, 160);
            this.Controls.Add(this.btnAboutOK);
            this.Controls.Add(this.tbRights);
            this.Controls.Add(this.tbCopyright);
            this.Controls.Add(this.tbProduct);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BhavFinderAbout";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbProduct;
        public System.Windows.Forms.TextBox tbCopyright;
        private System.Windows.Forms.TextBox tbRights;
        private System.Windows.Forms.Button btnAboutOK;
    }
}
