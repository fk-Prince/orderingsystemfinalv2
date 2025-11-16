namespace OrderingSystem.KioskApplication.Forms
{
    partial class Dashboard
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
            this.mm = new Guna.UI2.WinForms.Guna2Panel();
            this.bb = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.Typing = new System.Windows.Forms.Label();
            this.lblTyping = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.b2 = new Guna.UI2.WinForms.Guna2Button();
            this.b1 = new Guna.UI2.WinForms.Guna2Button();
            this.typingTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.mm.SuspendLayout();
            this.bb.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mm
            // 
            this.mm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mm.BackColor = System.Drawing.Color.White;
            this.mm.Controls.Add(this.bb);
            this.mm.Font = new System.Drawing.Font("Segoe UI Black", 27.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mm.Location = new System.Drawing.Point(0, 0);
            this.mm.MaximumSize = new System.Drawing.Size(1920, 2000);
            this.mm.MinimumSize = new System.Drawing.Size(1300, 700);
            this.mm.Name = "mm";
            this.mm.ShadowDecoration.Color = System.Drawing.Color.DarkTurquoise;
            this.mm.ShadowDecoration.Depth = 255;
            this.mm.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.mm.Size = new System.Drawing.Size(1301, 702);
            this.mm.TabIndex = 0;
            this.mm.SizeChanged += new System.EventHandler(this.mm_SizeChanged);
            // 
            // bb
            // 
            this.bb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bb.BackColor = System.Drawing.Color.Transparent;
            this.bb.Controls.Add(this.guna2Panel2);
            this.bb.Location = new System.Drawing.Point(12, 12);
            this.bb.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.bb.MinimumSize = new System.Drawing.Size(1276, 676);
            this.bb.Name = "bb";
            this.bb.Size = new System.Drawing.Size(1276, 676);
            this.bb.TabIndex = 2;
            this.bb.UseTransparentBackground = true;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Panel2.BackColor = System.Drawing.Color.White;
            this.guna2Panel2.Controls.Add(this.guna2Panel1);
            this.guna2Panel2.Font = new System.Drawing.Font("Segoe UI Black", 27.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Panel2.Location = new System.Drawing.Point(-12, -13);
            this.guna2Panel2.MaximumSize = new System.Drawing.Size(1920, 2000);
            this.guna2Panel2.MinimumSize = new System.Drawing.Size(1300, 700);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.ShadowDecoration.Color = System.Drawing.Color.DarkTurquoise;
            this.guna2Panel2.ShadowDecoration.Depth = 255;
            this.guna2Panel2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2Panel2.Size = new System.Drawing.Size(1301, 702);
            this.guna2Panel2.TabIndex = 1;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Controls.Add(this.Typing);
            this.guna2Panel1.Controls.Add(this.lblTyping);
            this.guna2Panel1.Controls.Add(this.pictureBox1);
            this.guna2Panel1.Controls.Add(this.b2);
            this.guna2Panel1.Controls.Add(this.b1);
            this.guna2Panel1.Font = new System.Drawing.Font("Sitka Small", 28.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Panel1.Location = new System.Drawing.Point(12, 12);
            this.guna2Panel1.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.guna2Panel1.MinimumSize = new System.Drawing.Size(1276, 676);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1276, 676);
            this.guna2Panel1.TabIndex = 2;
            this.guna2Panel1.UseTransparentBackground = true;
            // 
            // Typing
            // 
            this.Typing.Font = new System.Drawing.Font("Sitka Small", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Typing.Location = new System.Drawing.Point(0, 315);
            this.Typing.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Typing.Name = "Typing";
            this.Typing.Size = new System.Drawing.Size(1274, 28);
            this.Typing.TabIndex = 4;
            this.Typing.Text = "-";
            this.Typing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTyping
            // 
            this.lblTyping.AutoSize = true;
            this.lblTyping.Font = new System.Drawing.Font("Sitka Small", 15.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblTyping.Location = new System.Drawing.Point(284, 403);
            this.lblTyping.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTyping.Name = "lblTyping";
            this.lblTyping.Size = new System.Drawing.Size(0, 31);
            this.lblTyping.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Image = global::OrderingSystem.Properties.Resources.finaldashicon;
            this.pictureBox1.Location = new System.Drawing.Point(406, -37);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(484, 414);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
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
            this.b2.ImageOffset = new System.Drawing.Point(30, -20);
            this.b2.ImageSize = new System.Drawing.Size(130, 130);
            this.b2.Location = new System.Drawing.Point(672, 360);
            this.b2.Name = "b2";
            this.b2.ShadowDecoration.BorderRadius = 20;
            this.b2.ShadowDecoration.Color = System.Drawing.Color.Silver;
            this.b2.ShadowDecoration.Depth = 100;
            this.b2.ShadowDecoration.Enabled = true;
            this.b2.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(5, 5, 10, 10);
            this.b2.Size = new System.Drawing.Size(368, 313);
            this.b2.TabIndex = 0;
            this.b2.Text = "Dine In";
            this.b2.TextOffset = new System.Drawing.Point(-30, 80);
            this.b2.Click += new System.EventHandler(this.dinein);
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
            this.b1.ImageOffset = new System.Drawing.Point(39, -20);
            this.b1.ImageSize = new System.Drawing.Size(130, 130);
            this.b1.Location = new System.Drawing.Point(260, 360);
            this.b1.Name = "b1";
            this.b1.ShadowDecoration.BorderRadius = 20;
            this.b1.ShadowDecoration.Color = System.Drawing.Color.Silver;
            this.b1.ShadowDecoration.Depth = 100;
            this.b1.ShadowDecoration.Enabled = true;
            this.b1.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(5, 5, 10, 10);
            this.b1.Size = new System.Drawing.Size(368, 313);
            this.b1.TabIndex = 1;
            this.b1.Text = "Take Out";
            this.b1.TextOffset = new System.Drawing.Point(-35, 80);
            this.b1.Click += new System.EventHandler(this.takeout);
            // 
            // typingTimer
            // 
            this.typingTimer.Enabled = true;
            this.typingTimer.Interval = 30;
            this.typingTimer.Tick += new System.EventHandler(this.typingTimer_Tick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 134);
            this.label1.TabIndex = 5;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.mm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1920, 2000);
            this.MinimumSize = new System.Drawing.Size(1300, 700);
            this.Name = "Dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mm.ResumeLayout(false);
            this.bb.ResumeLayout(false);
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel mm;
        private Guna.UI2.WinForms.Guna2Panel bb;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label Typing;
        private System.Windows.Forms.Label lblTyping;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Button b2;
        private Guna.UI2.WinForms.Guna2Button b1;
        private System.Windows.Forms.Timer typingTimer;
        private System.Windows.Forms.Label label1;
    }
}