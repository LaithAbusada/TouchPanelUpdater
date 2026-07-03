using System.Drawing;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class ConnectDisconnectForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label lblIpAddress;
        private Guna.UI2.WinForms.Guna2Button btnConnectDisconnect;
		private System.Windows.Forms.LinkLabel linkWiki4Inch;
		private System.Windows.Forms.LinkLabel linkWiki5Inch;
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
			this.lblIpAddress = new System.Windows.Forms.Label();
			this.btnConnectDisconnect = new Guna.UI2.WinForms.Guna2Button();
			this.linkWiki4Inch = new System.Windows.Forms.LinkLabel();
			this.linkWiki5Inch = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			//
			// txtIpAddress
			//
			this.txtIpAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtIpAddress.ForeColor = System.Drawing.Color.Black;
			this.txtIpAddress.Location = new System.Drawing.Point(300, 120);
			this.txtIpAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtIpAddress.Name = "txtIpAddress";
			this.txtIpAddress.Size = new System.Drawing.Size(448, 34);
			this.txtIpAddress.TabIndex = 0;
			//
			// lblIpAddress
			//
			this.lblIpAddress.AutoSize = true;
			this.lblIpAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblIpAddress.ForeColor = System.Drawing.SystemColors.Highlight;
			this.lblIpAddress.Location = new System.Drawing.Point(75, 120);
			this.lblIpAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblIpAddress.Name = "lblIpAddress";
			this.lblIpAddress.Size = new System.Drawing.Size(103, 28);
			this.lblIpAddress.TabIndex = 1;
			this.lblIpAddress.Text = "IP Address";
			//
			// btnConnectDisconnect
			//
			this.btnConnectDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
			this.btnConnectDisconnect.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnConnectDisconnect.ForeColor = System.Drawing.Color.Black;
			this.btnConnectDisconnect.Location = new System.Drawing.Point(225, 227);
			this.btnConnectDisconnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnConnectDisconnect.Name = "btnConnectDisconnect";
			this.btnConnectDisconnect.Size = new System.Drawing.Size(450, 62);
			this.btnConnectDisconnect.TabIndex = 2;
			this.btnConnectDisconnect.Text = "Connect";
			this.btnConnectDisconnect.Click += new System.EventHandler(this.btnConnectDisconnect_Click);
			//
			// linkWiki4Inch
			//
			this.linkWiki4Inch.AutoSize = true;
			this.linkWiki4Inch.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.linkWiki4Inch.Location = new System.Drawing.Point(225, 340);
			this.linkWiki4Inch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.linkWiki4Inch.Name = "linkWiki4Inch";
			this.linkWiki4Inch.Size = new System.Drawing.Size(182, 28);
			this.linkWiki4Inch.TabIndex = 3;
			this.linkWiki4Inch.TabStop = true;
			this.linkWiki4Inch.Text = "4\" Touch Panel Wiki";
			this.linkWiki4Inch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWiki4Inch_LinkClicked);
			//
			// linkWiki5Inch
			//
			this.linkWiki5Inch.AutoSize = true;
			this.linkWiki5Inch.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.linkWiki5Inch.Location = new System.Drawing.Point(495, 340);
			this.linkWiki5Inch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.linkWiki5Inch.Name = "linkWiki5Inch";
			this.linkWiki5Inch.Size = new System.Drawing.Size(182, 28);
			this.linkWiki5Inch.TabIndex = 4;
			this.linkWiki5Inch.TabStop = true;
			this.linkWiki5Inch.Text = "5\" Touch Panel Wiki";
			this.linkWiki5Inch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWiki5Inch_LinkClicked);
			//
			// ConnectDisconnectForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(939, 662);
			this.Controls.Add(this.txtIpAddress);
			this.Controls.Add(this.lblIpAddress);
			this.Controls.Add(this.btnConnectDisconnect);
			this.Controls.Add(this.linkWiki4Inch);
			this.Controls.Add(this.linkWiki5Inch);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "ConnectDisconnectForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Connect / Disconnect";
			this.Load += new System.EventHandler(this.ConnectDisconnectForm_Load_1Async);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
    }
}
