namespace OrderingSystem.KioskApplication.Components
{
    partial class SizeLayout
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
            this.pp = new Guna.UI2.WinForms.Guna2Panel();
            this.menuName = new System.Windows.Forms.Label();
            this.subtitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.titleOption = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pp.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pp
            // 
            this.pp.BackColor = System.Drawing.Color.Transparent;
            this.pp.BorderColor = System.Drawing.Color.LightGray;
            this.pp.BorderRadius = 8;
            this.pp.BorderThickness = 1;
            this.pp.Controls.Add(this.menuName);
            this.pp.Controls.Add(this.subtitle);
            this.pp.Controls.Add(this.label2);
            this.pp.Controls.Add(this.titleOption);
            this.pp.Controls.Add(this.guna2Panel2);
            this.pp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pp.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.pp.Location = new System.Drawing.Point(0, 0);
            this.pp.Name = "pp";
            this.pp.Size = new System.Drawing.Size(730, 141);
            this.pp.TabIndex = 0;
            // 
            // menuName
            // 
            this.menuName.AutoSize = true;
            this.menuName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuName.Location = new System.Drawing.Point(137, 21);
            this.menuName.Name = "menuName";
            this.menuName.Size = new System.Drawing.Size(43, 17);
            this.menuName.TabIndex = 19;
            this.menuName.Text = "label3";
            // 
            // subtitle
            // 
            this.subtitle.AutoSize = true;
            this.subtitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subtitle.Location = new System.Drawing.Point(57, 51);
            this.subtitle.Name = "subtitle";
            this.subtitle.Size = new System.Drawing.Size(136, 13);
            this.subtitle.TabIndex = 18;
            this.subtitle.Text = "Select Size of your choice";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.ForeColor = System.Drawing.Color.IndianRed;
            this.label2.Location = new System.Drawing.Point(110, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "*";
            // 
            // titleOption
            // 
            this.titleOption.AutoSize = true;
            this.titleOption.BackColor = System.Drawing.Color.Transparent;
            this.titleOption.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleOption.Location = new System.Drawing.Point(23, 16);
            this.titleOption.Name = "titleOption";
            this.titleOption.Size = new System.Drawing.Size(96, 25);
            this.titleOption.TabIndex = 15;
            this.titleOption.Text = "Option A";
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.AutoRoundedCorners = true;
            this.guna2Panel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.BorderRadius = 11;
            this.guna2Panel2.Controls.Add(this.label1);
            this.guna2Panel2.FillColor = System.Drawing.Color.IndianRed;
            this.guna2Panel2.Location = new System.Drawing.Point(604, 21);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(91, 25);
            this.guna2Panel2.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(21, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Required";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SizeLayout
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.Controls.Add(this.pp);
            this.Name = "SizeLayout";
            this.Size = new System.Drawing.Size(730, 141);
            this.pp.ResumeLayout(false);
            this.pp.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label menuName;
        private System.Windows.Forms.Label subtitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label titleOption;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label label1;
        public Guna.UI2.WinForms.Guna2Panel pp;
    }
}