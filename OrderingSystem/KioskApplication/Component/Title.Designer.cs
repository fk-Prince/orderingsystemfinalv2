namespace OrderingSystem.KioskApplication.Component
{
    partial class Title
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
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.im = new Guna.UI2.WinForms.Guna2PictureBox();
            this.tt = new System.Windows.Forms.Label();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.im)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.AutoRoundedCorners = true;
            this.guna2Panel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(119)))), ((int)(((byte)(206)))));
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.im);
            this.guna2Panel1.Controls.Add(this.tt);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(97)))), ((int)(((byte)(29)))));
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(236, 35);
            this.guna2Panel1.TabIndex = 0;
            // 
            // im
            // 
            this.im.ImageRotate = 0F;
            this.im.Location = new System.Drawing.Point(196, 5);
            this.im.Name = "im";
            this.im.Size = new System.Drawing.Size(25, 25);
            this.im.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.im.TabIndex = 3;
            this.im.TabStop = false;
            // 
            // tt
            // 
            this.tt.AutoEllipsis = true;
            this.tt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tt.ForeColor = System.Drawing.Color.White;
            this.tt.Location = new System.Drawing.Point(16, 7);
            this.tt.Name = "tt";
            this.tt.Size = new System.Drawing.Size(169, 21);
            this.tt.TabIndex = 2;
            this.tt.Text = "label1";
            this.tt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Title
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.guna2Panel1);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.Name = "Title";
            this.Size = new System.Drawing.Size(236, 35);
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.im)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2PictureBox im;
        private System.Windows.Forms.Label tt;
    }
}