namespace Innovo_TP4_Updater
{
    partial class UpdateAppForm
    {
        private System.ComponentModel.IContainer components = null;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox3;
        private System.Windows.Forms.Label labelStatus;

        // New labels to display status
        private System.Windows.Forms.Label labelNiceStatus;
        private System.Windows.Forms.Label labelLutronStatus;
        private System.Windows.Forms.Label labelControl4Status;

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
            this.materialMultiLineTextBox3 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelNiceStatus = new System.Windows.Forms.Label();
            this.labelLutronStatus = new System.Windows.Forms.Label();
            this.labelControl4Status = new System.Windows.Forms.Label();
            this.appsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // materialMultiLineTextBox3
            // 
            this.materialMultiLineTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialMultiLineTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialMultiLineTextBox3.Depth = 0;
            this.materialMultiLineTextBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.materialMultiLineTextBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialMultiLineTextBox3.Hint = "";
            this.materialMultiLineTextBox3.Location = new System.Drawing.Point(46, 435);
            this.materialMultiLineTextBox3.Margin = new System.Windows.Forms.Padding(4);
            this.materialMultiLineTextBox3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialMultiLineTextBox3.Name = "materialMultiLineTextBox3";
            this.materialMultiLineTextBox3.Size = new System.Drawing.Size(1142, 192);
            this.materialMultiLineTextBox3.TabIndex = 8;
            this.materialMultiLineTextBox3.Text = "";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(35, 587);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(49, 20);
            this.labelStatus.TabIndex = 9;
            this.labelStatus.Text = "Status";
            // 
            // labelNiceStatus
            // 
            this.labelNiceStatus.AutoSize = true;
            this.labelNiceStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelNiceStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelNiceStatus.Location = new System.Drawing.Point(39, 172);
            this.labelNiceStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNiceStatus.Name = "labelNiceStatus";
            this.labelNiceStatus.Size = new System.Drawing.Size(0, 20);
            this.labelNiceStatus.TabIndex = 11;
            // 
            // labelLutronStatus
            // 
            this.labelLutronStatus.AutoSize = true;
            this.labelLutronStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelLutronStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelLutronStatus.Location = new System.Drawing.Point(39, 350);
            this.labelLutronStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLutronStatus.Name = "labelLutronStatus";
            this.labelLutronStatus.Size = new System.Drawing.Size(0, 20);
            this.labelLutronStatus.TabIndex = 13;
            // 
            // labelControl4Status
            // 
            this.labelControl4Status.AutoSize = true;
            this.labelControl4Status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelControl4Status.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelControl4Status.Location = new System.Drawing.Point(339, 350);
            this.labelControl4Status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelControl4Status.Name = "labelControl4Status";
            this.labelControl4Status.Size = new System.Drawing.Size(0, 20);
            this.labelControl4Status.TabIndex = 14;
            // 
            // appsPanel
            // 
            this.appsPanel.Location = new System.Drawing.Point(46, 12);
            this.appsPanel.Name = "appsPanel";
            this.appsPanel.Size = new System.Drawing.Size(1142, 416);
            this.appsPanel.TabIndex = 15;
            // 
            // UpdateAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1200, 640);
            this.Controls.Add(this.appsPanel);
            this.Controls.Add(this.labelControl4Status);
            this.Controls.Add(this.labelLutronStatus);
            this.Controls.Add(this.labelNiceStatus);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.materialMultiLineTextBox3);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UpdateAppForm";
            this.Text = "Update App";
            this.Load += new System.EventHandler(this.UpdateAppForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel appsPanel;
    }
}
