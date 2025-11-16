namespace OrderingSystem.CashierApp.Forms.Category
{
    partial class CategoryFrm
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
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.search = new Guna.UI2.WinForms.Guna2TextBox();
            this.title = new System.Windows.Forms.Label();
            this.b1 = new Guna.UI2.WinForms.Guna2Button();
            this.debouncing = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // flow
            // 
            this.flow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flow.Location = new System.Drawing.Point(39, 171);
            this.flow.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.flow.MinimumSize = new System.Drawing.Size(1040, 456);
            this.flow.Name = "flow";
            this.flow.Size = new System.Drawing.Size(1040, 456);
            this.flow.TabIndex = 0;
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
            this.search.Location = new System.Drawing.Point(39, 120);
            this.search.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.search.MaxLength = 255;
            this.search.Name = "search";
            this.search.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(156)))), ((int)(((byte)(162)))));
            this.search.PlaceholderText = "Search Category";
            this.search.SelectedText = "";
            this.search.Size = new System.Drawing.Size(469, 44);
            this.search.TabIndex = 15;
            this.search.TextOffset = new System.Drawing.Point(10, 0);
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            // 
            // title
            // 
            this.title.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.title.Font = new System.Drawing.Font("Segoe UI Black", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(1, 9);
            this.title.MaximumSize = new System.Drawing.Size(1920, 81);
            this.title.MinimumSize = new System.Drawing.Size(1121, 81);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(1121, 81);
            this.title.TabIndex = 14;
            this.title.Text = "Category";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // b1
            // 
            this.b1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b1.BorderRadius = 5;
            this.b1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.b1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.b1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.b1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.b1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.b1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.b1.ForeColor = System.Drawing.Color.White;
            this.b1.Location = new System.Drawing.Point(935, 130);
            this.b1.MaximumSize = new System.Drawing.Size(144, 35);
            this.b1.MinimumSize = new System.Drawing.Size(144, 35);
            this.b1.Name = "b1";
            this.b1.Size = new System.Drawing.Size(144, 35);
            this.b1.TabIndex = 13;
            this.b1.Text = "New Category";
            this.b1.Click += new System.EventHandler(this.addNewCategory);
            // 
            // debouncing
            // 
            this.debouncing.Interval = 500;
            this.debouncing.Tick += new System.EventHandler(this.debouncing_Tick);
            // 
            // CategoryFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1121, 660);
            this.Controls.Add(this.b1);
            this.Controls.Add(this.title);
            this.Controls.Add(this.search);
            this.Controls.Add(this.flow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(1121, 660);
            this.Name = "CategoryFrm";
            this.Text = "CategoryFrm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flow;
        public Guna.UI2.WinForms.Guna2TextBox search;
        public System.Windows.Forms.Label title;
        public Guna.UI2.WinForms.Guna2Button b1;
        private System.Windows.Forms.Timer debouncing;
    }
}