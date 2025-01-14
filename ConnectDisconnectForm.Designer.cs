using System.Drawing;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class ConnectDisconnectForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboProjectName;
        private System.Windows.Forms.ComboBox comboPanelLocation;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblPanelLocation;
        private System.Windows.Forms.Label lblIpAddress;
        private Guna.UI2.WinForms.Guna2Button btnConnectDisconnect;
        private Guna.UI2.WinForms.Guna2Button btnDeleteProject;
        private Guna.UI2.WinForms.Guna2Button btnDeletePanel;
        private Guna.UI2.WinForms.Guna2Button btnEditProject;
        private Guna.UI2.WinForms.Guna2Button btnEditPanel;
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
            this.comboProjectName = new System.Windows.Forms.ComboBox();
            this.comboPanelLocation = new System.Windows.Forms.ComboBox();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblPanelLocation = new System.Windows.Forms.Label();
            this.lblIpAddress = new System.Windows.Forms.Label();
            this.btnConnectDisconnect = new Guna.UI2.WinForms.Guna2Button();
            this.btnDeleteProject = new Guna.UI2.WinForms.Guna2Button();
            this.btnDeletePanel = new Guna.UI2.WinForms.Guna2Button();
            this.btnEditProject = new Guna.UI2.WinForms.Guna2Button();
            this.btnEditPanel = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // comboProjectName
            // 
            this.comboProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProjectName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboProjectName.ForeColor = System.Drawing.Color.Black;
            this.comboProjectName.FormattingEnabled = true;
            this.comboProjectName.Location = new System.Drawing.Point(200, 50);
            this.comboProjectName.Name = "comboProjectName";
            this.comboProjectName.Size = new System.Drawing.Size(300, 25);
            this.comboProjectName.TabIndex = 0;
            this.comboProjectName.SelectedIndexChanged += new System.EventHandler(this.comboProjectName_SelectedIndexChanged_1);
            // 
            // comboPanelLocation
            // 
            this.comboPanelLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPanelLocation.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboPanelLocation.ForeColor = System.Drawing.Color.Black;
            this.comboPanelLocation.FormattingEnabled = true;
            this.comboPanelLocation.Location = new System.Drawing.Point(200, 100);
            this.comboPanelLocation.Name = "comboPanelLocation";
            this.comboPanelLocation.Size = new System.Drawing.Size(300, 25);
            this.comboPanelLocation.TabIndex = 1;
            this.comboPanelLocation.SelectedIndexChanged += new System.EventHandler(this.comboPanelLocation_SelectedIndexChanged_1);
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtIpAddress.ForeColor = System.Drawing.Color.Black;
            this.txtIpAddress.Location = new System.Drawing.Point(200, 150);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(300, 25);
            this.txtIpAddress.TabIndex = 2;
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProjectName.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblProjectName.Location = new System.Drawing.Point(50, 50);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(91, 19);
            this.lblProjectName.TabIndex = 3;
            this.lblProjectName.Text = "Project Name";
            // 
            // lblPanelLocation
            // 
            this.lblPanelLocation.AutoSize = true;
            this.lblPanelLocation.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPanelLocation.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblPanelLocation.Location = new System.Drawing.Point(50, 100);
            this.lblPanelLocation.Name = "lblPanelLocation";
            this.lblPanelLocation.Size = new System.Drawing.Size(82, 19);
            this.lblPanelLocation.TabIndex = 4;
            this.lblPanelLocation.Text = "Panel Name";
            // 
            // lblIpAddress
            // 
            this.lblIpAddress.AutoSize = true;
            this.lblIpAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIpAddress.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblIpAddress.Location = new System.Drawing.Point(50, 150);
            this.lblIpAddress.Name = "lblIpAddress";
            this.lblIpAddress.Size = new System.Drawing.Size(74, 19);
            this.lblIpAddress.TabIndex = 5;
            this.lblIpAddress.Text = "IP Address";
            // 
            // btnConnectDisconnect
            // 
            this.btnConnectDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            this.btnConnectDisconnect.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnConnectDisconnect.ForeColor = System.Drawing.Color.Black;
            this.btnConnectDisconnect.Location = new System.Drawing.Point(150, 220);
            this.btnConnectDisconnect.Name = "btnConnectDisconnect";
            this.btnConnectDisconnect.Size = new System.Drawing.Size(300, 40);
            this.btnConnectDisconnect.TabIndex = 6;
            this.btnConnectDisconnect.Text = "Connect";
            this.btnConnectDisconnect.Click += new System.EventHandler(this.btnConnectDisconnect_Click);
            // 
            // btnDeleteProject
            // 
            this.btnDeleteProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeleteProject.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteProject.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteProject.Location = new System.Drawing.Point(150, 270);
            this.btnDeleteProject.Name = "btnDeleteProject";
            this.btnDeleteProject.Size = new System.Drawing.Size(120, 40);
            this.btnDeleteProject.TabIndex = 8;
            this.btnDeleteProject.Text = "Delete Project";
            this.btnDeleteProject.Click += new System.EventHandler(this.btnDeleteProject_Click);
            // 
            // btnDeletePanel
            // 
            this.btnDeletePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDeletePanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeletePanel.ForeColor = System.Drawing.Color.Black;
            this.btnDeletePanel.Location = new System.Drawing.Point(330, 270);
            this.btnDeletePanel.Name = "btnDeletePanel";
            this.btnDeletePanel.Size = new System.Drawing.Size(120, 40);
            this.btnDeletePanel.TabIndex = 9;
            this.btnDeletePanel.Text = "Delete Panel";
            this.btnDeletePanel.Click += new System.EventHandler(this.btnDeletePanel_Click);
            // 
            // btnEditProject
            // 
            this.btnEditProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            this.btnEditProject.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnEditProject.ForeColor = System.Drawing.Color.Black;
            this.btnEditProject.Location = new System.Drawing.Point(150, 320);
            this.btnEditProject.Name = "btnEditProject";
            this.btnEditProject.Size = new System.Drawing.Size(120, 40);
            this.btnEditProject.TabIndex = 10;
            this.btnEditProject.Text = "Edit Project";
            this.btnEditProject.Click += new System.EventHandler(this.btnEditProject_Click);
            // 
            // btnEditPanel
            // 
            this.btnEditPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(0)))));
            this.btnEditPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnEditPanel.ForeColor = System.Drawing.Color.Black;
            this.btnEditPanel.Location = new System.Drawing.Point(330, 320);
            this.btnEditPanel.Name = "btnEditPanel";
            this.btnEditPanel.Size = new System.Drawing.Size(120, 40);
            this.btnEditPanel.TabIndex = 11;
            this.btnEditPanel.Text = "Edit Panel";
            this.btnEditPanel.Click += new System.EventHandler(this.btnEditPanel_Click);
            // 
            // ConnectDisconnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(626, 383);
            this.Controls.Add(this.btnDeleteProject);
            this.Controls.Add(this.btnDeletePanel);
            this.Controls.Add(this.btnEditProject);
            this.Controls.Add(this.btnEditPanel);
            this.Controls.Add(this.comboProjectName);
            this.Controls.Add(this.comboPanelLocation);
            this.Controls.Add(this.txtIpAddress);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.lblPanelLocation);
            this.Controls.Add(this.lblIpAddress);
            this.Controls.Add(this.btnConnectDisconnect);
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
