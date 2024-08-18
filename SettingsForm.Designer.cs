using System;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel settingsPanel;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2Button btnUpdate;
        private Guna.UI2.WinForms.Guna2Button btnDisplay;
        private Guna.UI2.WinForms.Guna2Button btnConnectDisconnect;
        private Guna.UI2.WinForms.Guna2Button btnSound;
        private Guna.UI2.WinForms.Guna2Button btnTimeZone;
        private Guna.UI2.WinForms.Guna2Button btnReset;
        private Guna.UI2.WinForms.Guna2Button btnReboot;
        private Guna.UI2.WinForms.Guna2Button btnFactory;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblConnectionStatus; // Label for connection status

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private async void InitializeComponent()
        {
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.lblConnectionStatus = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnFactory = new Guna.UI2.WinForms.Guna2Button();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.btnUpdate = new Guna.UI2.WinForms.Guna2Button();
            this.btnDisplay = new Guna.UI2.WinForms.Guna2Button();
            this.btnConnectDisconnect = new Guna.UI2.WinForms.Guna2Button();
            this.btnSound = new Guna.UI2.WinForms.Guna2Button();
            this.btnTimeZone = new Guna.UI2.WinForms.Guna2Button();
            this.btnReset = new Guna.UI2.WinForms.Guna2Button();
            this.btnReboot = new Guna.UI2.WinForms.Guna2Button();
            this.settingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.AutoScroll = true;
            this.settingsPanel.BackColor = System.Drawing.SystemColors.Highlight;
            this.settingsPanel.Controls.Add(this.lblConnectionStatus);
            this.settingsPanel.Controls.Add(this.btnFactory);
            this.settingsPanel.Controls.Add(this.btnBack);
            this.settingsPanel.Controls.Add(this.btnUpdate);
            this.settingsPanel.Controls.Add(this.btnDisplay);
            this.settingsPanel.Controls.Add(this.btnConnectDisconnect);
            this.settingsPanel.Controls.Add(this.btnSound);
            this.settingsPanel.Controls.Add(this.btnTimeZone);
            this.settingsPanel.Controls.Add(this.btnReset);
            this.settingsPanel.Controls.Add(this.btnReboot);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(898, 780);
            this.settingsPanel.TabIndex = 0;
            this.settingsPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.settingsPanel_Paint);
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.White;
            this.lblConnectionStatus.Location = new System.Drawing.Point(12, 10);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(143, 19);
            this.lblConnectionStatus.TabIndex = 0;
            this.lblConnectionStatus.Text = "Checking connection...";
            // 
            // btnFactory
            // 
            this.btnFactory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFactory.ForeColor = System.Drawing.Color.White;
            this.btnFactory.Image = global::Innovo_TP4_Updater.Properties.Resources.reset;
            this.btnFactory.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnFactory.ImageSize = new System.Drawing.Size(40, 40);
            this.btnFactory.Location = new System.Drawing.Point(12, 555);
            this.btnFactory.Name = "btnFactory";
            this.btnFactory.Size = new System.Drawing.Size(250, 60);
            this.btnFactory.TabIndex = 10;
            this.btnFactory.Text = "RESET\nReset to Factory Default";
            this.btnFactory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnFactory.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btnFactory.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnBack.ImageSize = new System.Drawing.Size(25, 25);
            this.btnBack.Location = new System.Drawing.Point(27, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 40);
            this.btnBack.TabIndex = 8;
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Image = global::Innovo_TP4_Updater.Properties.Resources.update;
            this.btnUpdate.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnUpdate.ImageSize = new System.Drawing.Size(40, 40);
            this.btnUpdate.Location = new System.Drawing.Point(12, 156);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(250, 60);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update App";
            this.btnUpdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnUpdate.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDisplay.ForeColor = System.Drawing.Color.White;
            this.btnDisplay.Image = global::Innovo_TP4_Updater.Properties.Resources.display;
            this.btnDisplay.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnDisplay.ImageSize = new System.Drawing.Size(40, 40);
            this.btnDisplay.Location = new System.Drawing.Point(12, 231);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(250, 60);
            this.btnDisplay.TabIndex = 3;
            this.btnDisplay.Text = "Display";
            this.btnDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnDisplay.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnConnectDisconnect
            // 
            this.btnConnectDisconnect.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnConnectDisconnect.ForeColor = System.Drawing.Color.White;
            this.btnConnectDisconnect.Image = global::Innovo_TP4_Updater.Properties.Resources.connection;
            this.btnConnectDisconnect.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnConnectDisconnect.ImageSize = new System.Drawing.Size(40, 40);
            this.btnConnectDisconnect.Location = new System.Drawing.Point(12, 81);
            this.btnConnectDisconnect.Name = "btnConnectDisconnect";
            this.btnConnectDisconnect.Size = new System.Drawing.Size(250, 60);
            this.btnConnectDisconnect.TabIndex = 4;
            this.btnConnectDisconnect.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnConnectDisconnect.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnSound
            // 
            this.btnSound.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSound.ForeColor = System.Drawing.Color.White;
            this.btnSound.Image = global::Innovo_TP4_Updater.Properties.Resources.sound;
            this.btnSound.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSound.ImageSize = new System.Drawing.Size(40, 40);
            this.btnSound.Location = new System.Drawing.Point(12, 311);
            this.btnSound.Name = "btnSound";
            this.btnSound.Size = new System.Drawing.Size(250, 60);
            this.btnSound.TabIndex = 5;
            this.btnSound.Text = "Sound\n";
            this.btnSound.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnSound.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnTimeZone
            // 
            this.btnTimeZone.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTimeZone.ForeColor = System.Drawing.Color.White;
            this.btnTimeZone.Image = global::Innovo_TP4_Updater.Properties.Resources.timezone;
            this.btnTimeZone.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnTimeZone.ImageSize = new System.Drawing.Size(40, 40);
            this.btnTimeZone.Location = new System.Drawing.Point(12, 398);
            this.btnTimeZone.Name = "btnTimeZone";
            this.btnTimeZone.Size = new System.Drawing.Size(250, 60);
            this.btnTimeZone.TabIndex = 6;
            this.btnTimeZone.Text = "Time Zone\nTime Zone";
            this.btnTimeZone.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnTimeZone.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Image = global::Innovo_TP4_Updater.Properties.Resources.reset;
            this.btnReset.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReset.ImageSize = new System.Drawing.Size(40, 40);
            this.btnReset.Location = new System.Drawing.Point(12, 476);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(250, 60);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "RESET\nReset Applications";
            this.btnReset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnReset.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnReboot
            // 
            this.btnReboot.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReboot.ForeColor = System.Drawing.Color.White;
            this.btnReboot.Image = global::Innovo_TP4_Updater.Properties.Resources.reboot;
            this.btnReboot.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReboot.ImageSize = new System.Drawing.Size(40, 40);
            this.btnReboot.Location = new System.Drawing.Point(12, 639);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(250, 60);
            this.btnReboot.TabIndex = 9;
            this.btnReboot.Text = "REBOOT\nRestart Device";
            this.btnReboot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnReboot.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // SettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(898, 780);
            this.Controls.Add(this.settingsPanel);
            this.Name = "SettingsForm";
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.Load += new System.EventHandler(this.SettingsForm_Load);

        }
    }
}
