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
            this.pp = new Guna.UI2.WinForms.Guna2Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fb = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.max = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tt = new Guna.UI2.WinForms.Guna2TrackBar();
            this.pp.SuspendLayout();
            this.SuspendLayout();
            // 
            // pp
            // 
            this.pp.BackColor = System.Drawing.Color.White;
            this.pp.Controls.Add(this.label3);
            this.pp.Controls.Add(this.label2);
            this.pp.Controls.Add(this.fb);
            this.pp.Controls.Add(this.guna2Button1);
            this.pp.Controls.Add(this.flow);
            this.pp.Controls.Add(this.max);
            this.pp.Controls.Add(this.label1);
            this.pp.Controls.Add(this.tt);
            this.pp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pp.Location = new System.Drawing.Point(0, 0);
            this.pp.Name = "pp";
            this.pp.Size = new System.Drawing.Size(231, 483);
            this.pp.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 25;
            this.label3.Text = "Categories";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 24;
            this.label2.Text = "Price range:";
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
            this.fb.Location = new System.Drawing.Point(123, 442);
            this.fb.Name = "fb";
            this.fb.Size = new System.Drawing.Size(100, 30);
            this.fb.TabIndex = 23;
            this.fb.Text = "Reset";
            this.fb.Click += new System.EventHandler(this.fb_Click_1);
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
            this.guna2Button1.Location = new System.Drawing.Point(102, 404);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(121, 32);
            this.guna2Button1.TabIndex = 22;
            this.guna2Button1.Text = "Search";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click_1);
            // 
            // flow
            // 
            this.flow.AutoScroll = true;
            this.flow.Location = new System.Drawing.Point(10, 118);
            this.flow.Margin = new System.Windows.Forms.Padding(0);
            this.flow.Name = "flow";
            this.flow.Size = new System.Drawing.Size(213, 277);
            this.flow.TabIndex = 21;
            // 
            // max
            // 
            this.max.AutoSize = true;
            this.max.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.max.Location = new System.Drawing.Point(174, 61);
            this.max.Name = "max";
            this.max.Size = new System.Drawing.Size(48, 21);
            this.max.TabIndex = 20;
            this.max.Text = "1000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 21);
            this.label1.TabIndex = 19;
            this.label1.Text = "0";
            // 
            // tt
            // 
            this.tt.Location = new System.Drawing.Point(27, 35);
            this.tt.Maximum = 1000;
            this.tt.Minimum = 10;
            this.tt.Name = "tt";
            this.tt.Size = new System.Drawing.Size(181, 23);
            this.tt.TabIndex = 18;
            this.tt.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.tt.Value = 500;
            this.tt.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tt_Scroll_1);
            // 
            // Filter
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pp);
            this.Name = "Filter";
            this.Size = new System.Drawing.Size(231, 483);
            this.pp.ResumeLayout(false);
            this.pp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pp;
        private System.Windows.Forms.Label label2;
        public Guna.UI2.WinForms.Guna2Button fb;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private System.Windows.Forms.FlowLayoutPanel flow;
        private System.Windows.Forms.Label max;
        private System.Windows.Forms.Label label1;
        public Guna.UI2.WinForms.Guna2TrackBar tt;
        private System.Windows.Forms.Label label3;
    }
}