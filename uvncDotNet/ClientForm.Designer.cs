namespace uvncDotNet
{
    partial class ClientForm
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
            this.remoteDesktop1 = new VncSharp.RemoteDesktop();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctrlAltDelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.altF4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteDesktop1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // remoteDesktop1
            // 
            this.remoteDesktop1.AutoScroll = true;
            this.remoteDesktop1.AutoScrollMinSize = new System.Drawing.Size(600, 400);
            this.remoteDesktop1.Controls.Add(this.menuStrip1);
            this.remoteDesktop1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remoteDesktop1.Location = new System.Drawing.Point(0, 0);
            this.remoteDesktop1.Name = "remoteDesktop1";
            this.remoteDesktop1.Size = new System.Drawing.Size(292, 273);
            this.remoteDesktop1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sendToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(600, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // sendToolStripMenuItem
            // 
            this.sendToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctrlAltDelToolStripMenuItem,
            this.altF4ToolStripMenuItem});
            this.sendToolStripMenuItem.Name = "sendToolStripMenuItem";
            this.sendToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.sendToolStripMenuItem.Text = "&Send";
            // 
            // ctrlAltDelToolStripMenuItem
            // 
            this.ctrlAltDelToolStripMenuItem.Name = "ctrlAltDelToolStripMenuItem";
            this.ctrlAltDelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ctrlAltDelToolStripMenuItem.Text = "Ctrl+Alt+Del";
            this.ctrlAltDelToolStripMenuItem.Click += new System.EventHandler(this.ctrlAltDelToolStripMenuItem_Click);
            // 
            // altF4ToolStripMenuItem
            // 
            this.altF4ToolStripMenuItem.Name = "altF4ToolStripMenuItem";
            this.altF4ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.altF4ToolStripMenuItem.Text = "Alt+F4";
            this.altF4ToolStripMenuItem.Click += new System.EventHandler(this.altF4ToolStripMenuItem_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.remoteDesktop1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.remoteDesktop1.ResumeLayout(false);
            this.remoteDesktop1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VncSharp.RemoteDesktop remoteDesktop1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ctrlAltDelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem altF4ToolStripMenuItem;
    }
}