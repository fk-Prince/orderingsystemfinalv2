namespace OrderingSystem.KioskApplication.Layouts
{
    partial class PackageLayout
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
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pp.SuspendLayout();
            this.SuspendLayout();
            // 
            // pp
            // 
            this.pp.BackColor = System.Drawing.Color.Transparent;
            this.pp.BorderColor = System.Drawing.Color.LightGray;
            this.pp.BorderRadius = 8;
            this.pp.BorderThickness = 1;
            this.pp.Controls.Add(this.flowPanel);
            this.pp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pp.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.pp.Location = new System.Drawing.Point(0, 0);
            this.pp.Name = "pp";
            this.pp.Size = new System.Drawing.Size(721, 145);
            this.pp.TabIndex = 0;
            // 
            // flowPanel
            // 
            this.flowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.flowPanel.Location = new System.Drawing.Point(6, 3);
            this.flowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(708, 139);
            this.flowPanel.TabIndex = 1;
            // 
            // PackageLayout
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.Controls.Add(this.pp);
            this.Name = "PackageLayout";
            this.Size = new System.Drawing.Size(721, 145);
            this.pp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pp;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
    }
}