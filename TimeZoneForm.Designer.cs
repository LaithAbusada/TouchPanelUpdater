namespace Innovo_TP4_Updater
{
    partial class TimeZoneForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.timeZoneComboBox = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblChooseTimeZone = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblSuccessMessage = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.SuspendLayout();
            // 
            // timeZoneComboBox
            // 
            this.timeZoneComboBox.BackColor = System.Drawing.Color.Transparent;
            this.timeZoneComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.timeZoneComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timeZoneComboBox.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.timeZoneComboBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.timeZoneComboBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.timeZoneComboBox.ForeColor = System.Drawing.Color.Black;
            this.timeZoneComboBox.ItemHeight = 30;
            this.timeZoneComboBox.Location = new System.Drawing.Point(50, 80);
            this.timeZoneComboBox.Name = "timeZoneComboBox";
            this.timeZoneComboBox.Size = new System.Drawing.Size(300, 36);
            this.timeZoneComboBox.TabIndex = 0;
            this.timeZoneComboBox.SelectedIndexChanged += new System.EventHandler(this.timeZoneComboBox_SelectedIndexChanged);
            // 
            // lblChooseTimeZone
            // 
            this.lblChooseTimeZone.BackColor = System.Drawing.Color.Transparent;
            this.lblChooseTimeZone.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblChooseTimeZone.Location = new System.Drawing.Point(50, 40);
            this.lblChooseTimeZone.Name = "lblChooseTimeZone";
            this.lblChooseTimeZone.Size = new System.Drawing.Size(128, 23);
            this.lblChooseTimeZone.TabIndex = 1;
            this.lblChooseTimeZone.Text = "Choose TimeZone";
            // 
            // lblSuccessMessage
            // 
            this.lblSuccessMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblSuccessMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSuccessMessage.ForeColor = System.Drawing.Color.Green;
            this.lblSuccessMessage.Location = new System.Drawing.Point(50, 130);
            this.lblSuccessMessage.Name = "lblSuccessMessage";
            this.lblSuccessMessage.Size = new System.Drawing.Size(3, 2);
            this.lblSuccessMessage.TabIndex = 2;
            this.lblSuccessMessage.Text = null;
            this.lblSuccessMessage.Visible = false;
            // 
            // TimeZoneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.lblSuccessMessage);
            this.Controls.Add(this.lblChooseTimeZone);
            this.Controls.Add(this.timeZoneComboBox);
            this.Name = "TimeZoneForm";
            this.Text = "Time Zone Settings";
            this.Load += new System.EventHandler(this.TimeZoneForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Guna.UI2.WinForms.Guna2ComboBox timeZoneComboBox;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblChooseTimeZone;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSuccessMessage;
    }
}
