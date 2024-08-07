namespace Innovo_TP4_Updater
{
    partial class ConnectDisconnectForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblIpAddress;
        private System.Windows.Forms.Label lblPort;
        private Guna.UI2.WinForms.Guna2Button btnConnectDisconnect;

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
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblIpAddress = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.btnConnectDisconnect = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(280, 97);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(200, 20);
            this.txtIpAddress.TabIndex = 0;
            this.txtIpAddress.Visible = false;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(280, 186);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(200, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.Visible = false;
            // 
            // lblIpAddress
            // 
            this.lblIpAddress.AutoSize = true;
            this.lblIpAddress.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblIpAddress.Location = new System.Drawing.Point(277, 61);
            this.lblIpAddress.Name = "lblIpAddress";
            this.lblIpAddress.Size = new System.Drawing.Size(58, 13);
            this.lblIpAddress.TabIndex = 3;
            this.lblIpAddress.Text = "IP Address";
            this.lblIpAddress.Visible = false;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblPort.Location = new System.Drawing.Point(277, 150);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port";
            this.lblPort.Visible = false;
            // 
            // btnConnectDisconnect
            // 
            this.btnConnectDisconnect.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnConnectDisconnect.ForeColor = System.Drawing.Color.White;
            this.btnConnectDisconnect.Location = new System.Drawing.Point(280, 252);
            this.btnConnectDisconnect.Name = "btnConnectDisconnect";
            this.btnConnectDisconnect.Size = new System.Drawing.Size(200, 40);
            this.btnConnectDisconnect.TabIndex = 2;
            this.btnConnectDisconnect.Text = "Connect";
            this.btnConnectDisconnect.Click += new System.EventHandler(this.btnConnectDisconnect_Click);
            // 
            // ConnectDisconnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 466);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblIpAddress);
            this.Controls.Add(this.btnConnectDisconnect);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIpAddress);
            this.Name = "ConnectDisconnectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect/Disconnect";
            this.Load += new System.EventHandler(this.ConnectDisconnectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
