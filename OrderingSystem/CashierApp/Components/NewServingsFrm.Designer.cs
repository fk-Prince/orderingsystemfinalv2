namespace OrderingSystem.CashierApp.Forms.Menu
{
    partial class NewServingsFrm
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
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.mm = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.dtServed = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.qty = new Guna.UI2.WinForms.Guna2TextBox();
            this.menuPrice = new Guna.UI2.WinForms.Guna2TextBox();
            this.p = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.prep = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 20;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // mm
            // 
            this.mm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mm.Location = new System.Drawing.Point(12, 175);
            this.mm.Margin = new System.Windows.Forms.Padding(0);
            this.mm.MaximumSize = new System.Drawing.Size(613, 468);
            this.mm.MinimumSize = new System.Drawing.Size(613, 468);
            this.mm.Name = "mm";
            this.mm.Size = new System.Drawing.Size(613, 468);
            this.mm.TabIndex = 71;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox1.BorderRadius = 10;
            this.guna2PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.guna2PictureBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox1.Image = global::OrderingSystem.Properties.Resources.exit;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(605, 7);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(25, 25);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox1.TabIndex = 60;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.UseTransparentBackground = true;
            this.guna2PictureBox1.Click += new System.EventHandler(this.exit);
            // 
            // dtServed
            // 
            this.dtServed.BorderRadius = 5;
            this.dtServed.Checked = true;
            this.dtServed.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.dtServed.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtServed.ForeColor = System.Drawing.Color.White;
            this.dtServed.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtServed.Location = new System.Drawing.Point(32, 47);
            this.dtServed.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtServed.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtServed.Name = "dtServed";
            this.dtServed.Size = new System.Drawing.Size(230, 36);
            this.dtServed.TabIndex = 72;
            this.dtServed.Value = new System.DateTime(2025, 11, 22, 14, 57, 29, 300);
            this.dtServed.ValueChanged += new System.EventHandler(this.dtServed_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 73;
            this.label1.Text = "Date Serving";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(376, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 74;
            this.label2.Text = "Quantity";
            // 
            // qty
            // 
            this.qty.BorderRadius = 5;
            this.qty.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.qty.DefaultText = "";
            this.qty.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.qty.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.qty.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.qty.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.qty.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.qty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.qty.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.qty.Location = new System.Drawing.Point(390, 47);
            this.qty.Name = "qty";
            this.qty.PlaceholderText = "";
            this.qty.SelectedText = "";
            this.qty.Size = new System.Drawing.Size(225, 36);
            this.qty.TabIndex = 75;
            this.qty.TextChanged += new System.EventHandler(this.qty_TextChanged);
            // 
            // menuPrice
            // 
            this.menuPrice.BorderRadius = 5;
            this.menuPrice.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.menuPrice.DefaultText = "";
            this.menuPrice.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.menuPrice.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.menuPrice.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.menuPrice.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.menuPrice.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.menuPrice.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuPrice.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.menuPrice.Location = new System.Drawing.Point(32, 116);
            this.menuPrice.Name = "menuPrice";
            this.menuPrice.PlaceholderText = "";
            this.menuPrice.SelectedText = "";
            this.menuPrice.Size = new System.Drawing.Size(225, 36);
            this.menuPrice.TabIndex = 76;
            this.menuPrice.TextChanged += new System.EventHandler(this.menuPrice_TextChanged);
            this.menuPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.menuPrice_KeyPress);
            // 
            // p
            // 
            this.p.AutoSize = true;
            this.p.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p.Location = new System.Drawing.Point(13, 96);
            this.p.Name = "p";
            this.p.Size = new System.Drawing.Size(113, 17);
            this.p.TabIndex = 77;
            this.p.Text = "Price per Servings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(376, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 17);
            this.label3.TabIndex = 79;
            this.label3.Text = "Preperation Per Serve";
            // 
            // prep
            // 
            this.prep.BorderRadius = 5;
            this.prep.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.prep.DefaultText = "";
            this.prep.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.prep.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.prep.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.prep.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.prep.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.prep.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.prep.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.prep.Location = new System.Drawing.Point(390, 116);
            this.prep.Name = "prep";
            this.prep.PlaceholderText = "";
            this.prep.SelectedText = "";
            this.prep.Size = new System.Drawing.Size(225, 36);
            this.prep.TabIndex = 80;
            // 
            // NewServingsFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(638, 652);
            this.Controls.Add(this.prep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.p);
            this.Controls.Add(this.menuPrice);
            this.Controls.Add(this.qty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtServed);
            this.Controls.Add(this.mm);
            this.Controls.Add(this.guna2PictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NewServingsFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IngredientListPopup";
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2Panel mm;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtServed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox qty;
        private System.Windows.Forms.Label p;
        private Guna.UI2.WinForms.Guna2TextBox menuPrice;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox prep;
    }
}