namespace Innovo_TP4_Updater
{
    partial class UpdateAppForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button buttonUpdate;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox1;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox3;
        private System.Windows.Forms.Label labelStatus;

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

        private void InitializeComponent()
        {
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.materialMultiLineTextBox1 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.materialMultiLineTextBox3 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelIP.ForeColor = System.Drawing.Color.Blue;
            this.labelIP.Location = new System.Drawing.Point(34, 32);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(78, 20);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "IP Address";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxIP.Location = new System.Drawing.Point(137, 32);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(171, 27);
            this.textBoxIP.TabIndex = 1;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelPort.ForeColor = System.Drawing.Color.Blue;
            this.labelPort.Location = new System.Drawing.Point(34, 75);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(35, 20);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Port";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxPort.Location = new System.Drawing.Point(137, 75);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(171, 27);
            this.textBoxPort.TabIndex = 3;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonUpdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonUpdate.ForeColor = System.Drawing.Color.White;
            this.buttonUpdate.Location = new System.Drawing.Point(34, 117);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(274, 32);
            this.buttonUpdate.TabIndex = 4;
            this.buttonUpdate.Text = "Update App";
            this.buttonUpdate.UseVisualStyleBackColor = false;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // materialMultiLineTextBox1
            // 
            this.materialMultiLineTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialMultiLineTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialMultiLineTextBox1.Depth = 0;
            this.materialMultiLineTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.materialMultiLineTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialMultiLineTextBox1.Hint = "";
            this.materialMultiLineTextBox1.Location = new System.Drawing.Point(34, 171);
            this.materialMultiLineTextBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialMultiLineTextBox1.Name = "materialMultiLineTextBox1";
            this.materialMultiLineTextBox1.Size = new System.Drawing.Size(846, 192);
            this.materialMultiLineTextBox1.TabIndex = 5;
            this.materialMultiLineTextBox1.Text = "";
            // 
            // materialMultiLineTextBox3
            // 
            this.materialMultiLineTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialMultiLineTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialMultiLineTextBox3.Depth = 0;
            this.materialMultiLineTextBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.materialMultiLineTextBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialMultiLineTextBox3.Hint = "";
            this.materialMultiLineTextBox3.Location = new System.Drawing.Point(34, 384);
            this.materialMultiLineTextBox3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialMultiLineTextBox3.Name = "materialMultiLineTextBox3";
            this.materialMultiLineTextBox3.Size = new System.Drawing.Size(846, 192);
            this.materialMultiLineTextBox3.TabIndex = 6;
            this.materialMultiLineTextBox3.Text = "";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(34, 587);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(49, 20);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "Status";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(771, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "label5";
            // 
            // UpdateAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 640);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.materialMultiLineTextBox3);
            this.Controls.Add(this.materialMultiLineTextBox1);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.labelIP);
            this.Name = "UpdateAppForm";
            this.Text = "Update App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
    }
}
