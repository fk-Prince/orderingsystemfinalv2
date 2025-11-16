namespace OrderingSystem.CashierApp.Forms.Staffs
{
    partial class StaffFrm
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
            this.b1 = new Guna.UI2.WinForms.Guna2Button();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.search = new Guna.UI2.WinForms.Guna2TextBox();
            this.debouncing = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // b1
            // 
            this.b1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b1.BorderRadius = 5;
            this.b1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.b1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.b1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.b1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.b1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(159)))), ((int)(((byte)(249)))));
            this.b1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.b1.ForeColor = System.Drawing.Color.White;
            this.b1.Location = new System.Drawing.Point(943, 12);
            this.b1.Name = "b1";
            this.b1.Size = new System.Drawing.Size(151, 45);
            this.b1.TabIndex = 0;
            this.b1.Text = "Add Staff";
            this.b1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // flowPanel
            // 
            this.flowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPanel.Location = new System.Drawing.Point(12, 63);
            this.flowPanel.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.flowPanel.MinimumSize = new System.Drawing.Size(1082, 585);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(1082, 585);
            this.flowPanel.TabIndex = 2;
            // 
            // search
            // 
            this.search.AutoRoundedCorners = true;
            this.search.BackColor = System.Drawing.Color.Transparent;
            this.search.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.search.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.search.DefaultText = "";
            this.search.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.search.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.search.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.search.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.search.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.search.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.search.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.search.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.search.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.search.IconLeft = global::OrderingSystem.Properties.Resources.ss;
            this.search.IconLeftSize = new System.Drawing.Size(25, 25);
            this.search.Location = new System.Drawing.Point(12, 13);
            this.search.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.search.MaxLength = 255;
            this.search.Name = "search";
            this.search.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(156)))), ((int)(((byte)(162)))));
            this.search.PlaceholderText = "Search Staff";
            this.search.SelectedText = "";
            this.search.Size = new System.Drawing.Size(469, 44);
            this.search.TabIndex = 16;
            this.search.TextOffset = new System.Drawing.Point(10, 0);
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            // 
            // debouncing
            // 
            this.debouncing.Tick += new System.EventHandler(this.debouncing_Tick);
            // 
            // StaffFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1121, 660);
            this.Controls.Add(this.search);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.b1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(1121, 660);
            this.Name = "StaffFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StaffFrm";
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button b1;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        public Guna.UI2.WinForms.Guna2TextBox search;
        private System.Windows.Forms.Timer debouncing;
    }
}