namespace TouchPanelUpdater
{
    partial class Form1
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
            this.IP = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.materialMultiLineTextBox1 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.materialMultiLineTextBox2 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.materialMultiLineTextBox3 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // IP
            // 
            this.IP.AutoSize = true;
            this.IP.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.IP.ForeColor = System.Drawing.Color.Snow;
            this.IP.Location = new System.Drawing.Point(123, 86);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(19, 16);
            this.IP.TabIndex = 0;
            this.IP.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(350, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(162, 83);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(149, 22);
            this.textBoxIP.TabIndex = 2;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(403, 80);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(70, 22);
            this.textBoxPort.TabIndex = 3;
            // 
            // materialMultiLineTextBox1
            // 
            this.materialMultiLineTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialMultiLineTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialMultiLineTextBox1.Depth = 0;
            this.materialMultiLineTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialMultiLineTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialMultiLineTextBox1.Hint = "";
            this.materialMultiLineTextBox1.Location = new System.Drawing.Point(12, 235);
            this.materialMultiLineTextBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialMultiLineTextBox1.Name = "materialMultiLineTextBox1";
            this.materialMultiLineTextBox1.ReadOnly = true;
            this.materialMultiLineTextBox1.Size = new System.Drawing.Size(276, 203);
            this.materialMultiLineTextBox1.TabIndex = 4;
            this.materialMultiLineTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(160, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(318, 40);
            this.button1.TabIndex = 5;
            this.button1.Text = "Upgrade Nice Apk";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1Async);
            // 
            // materialMultiLineTextBox2
            // 
            this.materialMultiLineTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialMultiLineTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialMultiLineTextBox2.Depth = 0;
            this.materialMultiLineTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialMultiLineTextBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialMultiLineTextBox2.Hint = "";
            this.materialMultiLineTextBox2.Location = new System.Drawing.Point(601, 115);
            this.materialMultiLineTextBox2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialMultiLineTextBox2.Name = "materialMultiLineTextBox2";
            this.materialMultiLineTextBox2.ReadOnly = true;
            this.materialMultiLineTextBox2.Size = new System.Drawing.Size(284, 169);
            this.materialMultiLineTextBox2.TabIndex = 6;
            this.materialMultiLineTextBox2.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(598, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Connected Devices";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TouchPanelUpdater.Properties.Resources.InnovoLogoWhiteOnBlack4801;
            this.pictureBox1.Location = new System.Drawing.Point(-1, 76);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(104, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Console";
            // 
            // materialMultiLineTextBox3
            // 
            this.materialMultiLineTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialMultiLineTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialMultiLineTextBox3.Depth = 0;
            this.materialMultiLineTextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialMultiLineTextBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialMultiLineTextBox3.Hint = "";
            this.materialMultiLineTextBox3.Location = new System.Drawing.Point(305, 235);
            this.materialMultiLineTextBox3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialMultiLineTextBox3.Name = "materialMultiLineTextBox3";
            this.materialMultiLineTextBox3.ReadOnly = true;
            this.materialMultiLineTextBox3.Size = new System.Drawing.Size(276, 203);
            this.materialMultiLineTextBox3.TabIndex = 10;
            this.materialMultiLineTextBox3.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(302, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Messages";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(906, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.materialMultiLineTextBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.materialMultiLineTextBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.materialMultiLineTextBox1);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IP);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "Form1";
            this.Text = "Touch Panel Updater";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox1;
        private System.Windows.Forms.Button button1;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox3;
        private System.Windows.Forms.Label label4;
    }
}

