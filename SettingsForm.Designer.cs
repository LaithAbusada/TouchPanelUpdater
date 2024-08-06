using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel settingsPanel;
        private Guna.UI2.WinForms.Guna2Button btnWiFi;
        private Guna.UI2.WinForms.Guna2Button btnBluetooth;
        private Guna.UI2.WinForms.Guna2Button btnUpdate;
        private Guna.UI2.WinForms.Guna2Button btnDisplay;
        private Guna.UI2.WinForms.Guna2Button btnNotifications;
        private Guna.UI2.WinForms.Guna2Button btnSound;
        private Guna.UI2.WinForms.Guna2Button btnTimeZone;
        private Guna.UI2.WinForms.Guna2Button btnReset;
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
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.btnWiFi = new Guna.UI2.WinForms.Guna2Button();
            this.btnBluetooth = new Guna.UI2.WinForms.Guna2Button();
            this.btnUpdate = new Guna.UI2.WinForms.Guna2Button();
            this.btnDisplay = new Guna.UI2.WinForms.Guna2Button();
            this.btnNotifications = new Guna.UI2.WinForms.Guna2Button();
            this.btnSound = new Guna.UI2.WinForms.Guna2Button();
            this.btnTimeZone = new Guna.UI2.WinForms.Guna2Button();
            this.btnReset = new Guna.UI2.WinForms.Guna2Button();
            this.settingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.Color.AliceBlue;
            this.settingsPanel.Controls.Add(this.btnWiFi);
            this.settingsPanel.Controls.Add(this.btnBluetooth);
            this.settingsPanel.Controls.Add(this.btnUpdate);
            this.settingsPanel.Controls.Add(this.btnDisplay);
            this.settingsPanel.Controls.Add(this.btnNotifications);
            this.settingsPanel.Controls.Add(this.btnSound);
            this.settingsPanel.Controls.Add(this.btnTimeZone);
            this.settingsPanel.Controls.Add(this.btnReset);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(800, 600);
            this.settingsPanel.TabIndex = 0;
            // 
            // btnWiFi
            // 
            this.btnWiFi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnWiFi.ForeColor = System.Drawing.Color.White;
            this.btnWiFi.Image = global::Innovo_TP4_Updater.Properties.Resources.wifi;
            this.btnWiFi.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWiFi.ImageSize = new System.Drawing.Size(40, 40);
            this.btnWiFi.Location = new System.Drawing.Point(275, 50);
            this.btnWiFi.Name = "btnWiFi";
            this.btnWiFi.Size = new System.Drawing.Size(250, 60);
            this.btnWiFi.TabIndex = 0;
            this.btnWiFi.Text = "Wi-Fi\n";
            this.btnWiFi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnWiFi.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnBluetooth
            // 
            this.btnBluetooth.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBluetooth.ForeColor = System.Drawing.Color.White;
            this.btnBluetooth.Image = global::Innovo_TP4_Updater.Properties.Resources.bluetooth;
            this.btnBluetooth.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnBluetooth.ImageSize = new System.Drawing.Size(40, 40);
            this.btnBluetooth.Location = new System.Drawing.Point(275, 120);
            this.btnBluetooth.Name = "btnBluetooth";
            this.btnBluetooth.Size = new System.Drawing.Size(250, 60);
            this.btnBluetooth.TabIndex = 1;
            this.btnBluetooth.Text = "Bluetooth\n";
            this.btnBluetooth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnBluetooth.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Image = global::Innovo_TP4_Updater.Properties.Resources.update;
            this.btnUpdate.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnUpdate.ImageSize = new System.Drawing.Size(40, 40);
            this.btnUpdate.Location = new System.Drawing.Point(275, 190);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(250, 60);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update\nOTA Update";
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
            this.btnDisplay.Location = new System.Drawing.Point(275, 260);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(250, 60);
            this.btnDisplay.TabIndex = 3;
            this.btnDisplay.Text = "Display";
            this.btnDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnDisplay.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnNotifications
            // 
            this.btnNotifications.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNotifications.ForeColor = System.Drawing.Color.White;
            this.btnNotifications.Image = global::Innovo_TP4_Updater.Properties.Resources.notifications;
            this.btnNotifications.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnNotifications.ImageSize = new System.Drawing.Size(40, 40);
            this.btnNotifications.Location = new System.Drawing.Point(275, 330);
            this.btnNotifications.Name = "btnNotifications";
            this.btnNotifications.Size = new System.Drawing.Size(250, 60);
            this.btnNotifications.TabIndex = 4;
            this.btnNotifications.Text = "Notifications\n";
            this.btnNotifications.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnNotifications.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // btnSound
            // 
            this.btnSound.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSound.ForeColor = System.Drawing.Color.White;
            this.btnSound.Image = global::Innovo_TP4_Updater.Properties.Resources.sound;
            this.btnSound.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSound.ImageSize = new System.Drawing.Size(40, 40);
            this.btnSound.Location = new System.Drawing.Point(275, 400);
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
            this.btnTimeZone.Location = new System.Drawing.Point(275, 470);
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
            this.btnReset.Location = new System.Drawing.Point(275, 540);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(250, 60);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "RESET\nReset to Factory Default";
            this.btnReset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnReset.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // SettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.settingsPanel);
            this.Name = "SettingsForm";
            this.settingsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

      

        #endregion
    }
}