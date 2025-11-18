namespace OrderingSystem.KioskApplication.Component
{
    partial class OrderTypeFrm
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
            this.components = new System.ComponentModel.Container();
            this.b2 = new Guna.UI2.WinForms.Guna2Button();
            this.b1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // b2
            // 
            this.b2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.b2.BackColor = System.Drawing.Color.Transparent;
            this.b2.BorderRadius = 20;
            this.b2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.b2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.b2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.b2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.b2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(97)))), ((int)(((byte)(29)))));
            this.b2.Font = new System.Drawing.Font("Sitka Small", 16.2F, System.Drawing.FontStyle.Bold);
            this.b2.ForeColor = System.Drawing.Color.White;
            this.b2.Image = global::OrderingSystem.Properties.Resources.dinein;
            this.b2.ImageOffset = new System.Drawing.Point(25, -20);
            this.b2.ImageSize = new System.Drawing.Size(130, 130);
            this.b2.Location = new System.Drawing.Point(284, 65);
            this.b2.Name = "b2";
            this.b2.ShadowDecoration.BorderRadius = 20;
            this.b2.ShadowDecoration.Color = System.Drawing.Color.Silver;
            this.b2.ShadowDecoration.Depth = 100;
            this.b2.ShadowDecoration.Enabled = true;
            this.b2.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(5, 5, 10, 10);
            this.b2.Size = new System.Drawing.Size(250, 250);
            this.b2.TabIndex = 7;
            this.b2.Text = "Dine In";
            this.b2.TextOffset = new System.Drawing.Point(-33, 80);
            this.b2.Click += new System.EventHandler(this.dineIn);
            // 
            // b1
            // 
            this.b1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.b1.BackColor = System.Drawing.Color.Transparent;
            this.b1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(97)))), ((int)(((byte)(29)))));
            this.b1.BorderRadius = 20;
            this.b1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.b1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.b1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.b1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.b1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.b1.Font = new System.Drawing.Font("Sitka Small", 16.2F, System.Drawing.FontStyle.Bold);
            this.b1.ForeColor = System.Drawing.Color.Black;
            this.b1.Image = global::OrderingSystem.Properties.Resources.takeout;
            this.b1.ImageOffset = new System.Drawing.Point(35, -20);
            this.b1.ImageSize = new System.Drawing.Size(130, 130);
            this.b1.Location = new System.Drawing.Point(12, 65);
            this.b1.Name = "b1";
            this.b1.ShadowDecoration.BorderRadius = 20;
            this.b1.ShadowDecoration.Color = System.Drawing.Color.Silver;
            this.b1.ShadowDecoration.Depth = 100;
            this.b1.ShadowDecoration.Enabled = true;
            this.b1.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(5, 5, 10, 10);
            this.b1.Size = new System.Drawing.Size(250, 250);
            this.b1.TabIndex = 8;
            this.b1.Text = "Take Out";
            this.b1.TextOffset = new System.Drawing.Point(-33, 80);
            this.b1.Click += new System.EventHandler(this.takeOut);
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 25;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = global::OrderingSystem.Properties.Resources.exit;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(507, 3);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(32, 32);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox1.TabIndex = 10;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.Click += new System.EventHandler(this.guna2PictureBox1_Click);
            // 
            // OrderTypeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 344);
            this.Controls.Add(this.guna2PictureBox1);
            this.Controls.Add(this.b2);
            this.Controls.Add(this.b1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OrderTypeFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OrderTypeFrm";
            this.Load += new System.EventHandler(this.OrderTypeFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button b2;
        private Guna.UI2.WinForms.Guna2Button b1;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
    }
}