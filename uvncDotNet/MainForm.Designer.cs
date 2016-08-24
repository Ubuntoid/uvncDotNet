namespace uvncDotNet
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            uvncDotNet.Controls.FlatMenuItemList flatMenuItemList1 = new uvncDotNet.Controls.FlatMenuItemList();
            this.partnerIDTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.connectToPartnerButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.flatMenuBar1 = new uvncDotNet.Controls.FlatMenuBar();
            this.SuspendLayout();
            // 
            // partnerIDTextBox
            // 
            this.partnerIDTextBox.Location = new System.Drawing.Point(285, 160);
            this.partnerIDTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.partnerIDTextBox.Name = "partnerIDTextBox";
            this.partnerIDTextBox.Size = new System.Drawing.Size(250, 25);
            this.partnerIDTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label1.Location = new System.Drawing.Point(282, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Partner ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(280, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Control Remote";
            // 
            // connectToPartnerButton
            // 
            this.connectToPartnerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(71)))), ((int)(((byte)(38)))));
            this.connectToPartnerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connectToPartnerButton.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.connectToPartnerButton.ForeColor = System.Drawing.Color.White;
            this.connectToPartnerButton.Location = new System.Drawing.Point(285, 210);
            this.connectToPartnerButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.connectToPartnerButton.Name = "connectToPartnerButton";
            this.connectToPartnerButton.Size = new System.Drawing.Size(175, 30);
            this.connectToPartnerButton.TabIndex = 3;
            this.connectToPartnerButton.Text = "Connect to partner";
            this.connectToPartnerButton.UseVisualStyleBackColor = false;
            this.connectToPartnerButton.Click += new System.EventHandler(this.connectToPartnerButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(71)))), ((int)(((byte)(38)))));
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "Remote Control";
            // 
            // flatMenuBar1
            // 
            this.flatMenuBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(71)))), ((int)(((byte)(38)))));
            this.flatMenuBar1.BorderColor = System.Drawing.Color.Black;
            this.flatMenuBar1.DisabledTextColor = System.Drawing.Color.Gray;
            this.flatMenuBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flatMenuBar1.EnableBorderDrawing = false;
            this.flatMenuBar1.EnableHoverBorderDrawing = false;
            this.flatMenuBar1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.flatMenuBar1.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(110)))), ((int)(((byte)(75)))));
            this.flatMenuBar1.HoverBorderColor = System.Drawing.Color.Black;
            this.flatMenuBar1.HoverFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.flatMenuBar1.HoverTextColor = System.Drawing.Color.White;
            this.flatMenuBar1.Location = new System.Drawing.Point(0, 0);
            this.flatMenuBar1.MenuItems = flatMenuItemList1;
            this.flatMenuBar1.Name = "flatMenuBar1";
            this.flatMenuBar1.ParentMenu = null;
            this.flatMenuBar1.ParentMenuItem = null;
            this.flatMenuBar1.SeparatorColor = System.Drawing.Color.Black;
            this.flatMenuBar1.Size = new System.Drawing.Size(554, 26);
            this.flatMenuBar1.TabIndex = 4;
            this.flatMenuBar1.TabStop = false;
            this.flatMenuBar1.TextColor = System.Drawing.Color.White;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::uvncDotNet.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(554, 352);
            this.Controls.Add(this.flatMenuBar1);
            this.Controls.Add(this.connectToPartnerButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.partnerIDTextBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox partnerIDTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button connectToPartnerButton;
        private System.Windows.Forms.Label label3;
        private Controls.FlatMenuBar flatMenuBar1;
    }
}

