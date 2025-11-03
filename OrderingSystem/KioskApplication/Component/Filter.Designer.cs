namespace OrderingSystem.KioskApplication.Component
{
    partial class Filter
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
            this.tt = new Guna.UI2.WinForms.Guna2TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.max = new System.Windows.Forms.Label();
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.fb = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // tt
            // 
            this.tt.Location = new System.Drawing.Point(26, 22);
            this.tt.Maximum = 1000;
            this.tt.Minimum = 10;
            this.tt.Name = "tt";
            this.tt.Size = new System.Drawing.Size(181, 23);
            this.tt.TabIndex = 0;
            this.tt.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.tt.Value = 500;
            this.tt.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tt_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "0";
            // 
            // max
            // 
            this.max.AutoSize = true;
            this.max.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.max.Location = new System.Drawing.Point(173, 48);
            this.max.Name = "max";
            this.max.Size = new System.Drawing.Size(48, 21);
            this.max.TabIndex = 2;
            this.max.Text = "1000";
            // 
            // flow
            // 
            this.flow.AutoScroll = true;
            this.flow.Location = new System.Drawing.Point(9, 78);
            this.flow.Margin = new System.Windows.Forms.Padding(0);
            this.flow.Name = "flow";
            this.flow.Size = new System.Drawing.Size(213, 307);
            this.flow.TabIndex = 4;
            // 
            // guna2Button1
            // 
            this.guna2Button1.BorderRadius = 5;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(101, 402);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(121, 32);
            this.guna2Button1.TabIndex = 5;
            this.guna2Button1.Text = "Search";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // fb
            // 
            this.fb.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(159)))), ((int)(((byte)(249)))));
            this.fb.BorderRadius = 5;
            this.fb.BorderThickness = 1;
            this.fb.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.fb.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.fb.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.fb.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.fb.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.fb.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.fb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.fb.Location = new System.Drawing.Point(122, 440);
            this.fb.Name = "fb";
            this.fb.Size = new System.Drawing.Size(100, 30);
            this.fb.TabIndex = 16;
            this.fb.Text = "Reset";
            this.fb.Click += new System.EventHandler(this.fb_Click);
            // 
            // Filter
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(231, 483);
            this.Controls.Add(this.fb);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.flow);
            this.Controls.Add(this.max);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tt);
            this.Name = "Filter";
            this.Text = "Filter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label max;
        private System.Windows.Forms.FlowLayoutPanel flow;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        public Guna.UI2.WinForms.Guna2TrackBar tt;
        public Guna.UI2.WinForms.Guna2Button fb;
    }
}