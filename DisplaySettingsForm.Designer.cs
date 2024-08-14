using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class DisplaySettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2TrackBar brightnessTrackBar;
        private Guna.UI2.WinForms.Guna2ToggleSwitch adaptiveBrightnessSwitch;
        private Guna.UI2.WinForms.Guna2ToggleSwitch screenSaverSwitch;
        private Guna.UI2.WinForms.Guna2ToggleSwitch sleepModeSwitch;
        private Label lblBrightness;
        private Label lblAdaptiveBrightness;
        private Label lblScreenSaver;
        private Label lblSleepMode;
        private Panel adaptiveBrightnessPanel;
        private Panel screenSaverPanel;
        private Panel sleepModePanel;
        private Guna.UI2.WinForms.Guna2Button btnBalanced;
        private Guna.UI2.WinForms.Guna2Button btnAlwaysReady;
        private Guna.UI2.WinForms.Guna2Button btnRestMode;

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
            this.brightnessTrackBar = new Guna.UI2.WinForms.Guna2TrackBar();
            this.adaptiveBrightnessSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.screenSaverSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.sleepModeSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.lblBrightness = new System.Windows.Forms.Label();
            this.lblAdaptiveBrightness = new System.Windows.Forms.Label();
            this.lblScreenSaver = new System.Windows.Forms.Label();
            this.lblSleepMode = new System.Windows.Forms.Label();
            this.adaptiveBrightnessPanel = new System.Windows.Forms.Panel();
            this.screenSaverPanel = new System.Windows.Forms.Panel();
            this.sleepModePanel = new System.Windows.Forms.Panel();
            this.btnBalanced = new Guna.UI2.WinForms.Guna2Button();
            this.btnAlwaysReady = new Guna.UI2.WinForms.Guna2Button();
            this.btnRestMode = new Guna.UI2.WinForms.Guna2Button();
            this.adaptiveBrightnessPanel.SuspendLayout();
            this.screenSaverPanel.SuspendLayout();
            this.sleepModePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // brightnessTrackBar
            // 
            this.brightnessTrackBar.Location = new System.Drawing.Point(300, 77);
            this.brightnessTrackBar.Maximum = 255;
            this.brightnessTrackBar.Name = "brightnessTrackBar";
            this.brightnessTrackBar.Size = new System.Drawing.Size(360, 45);
            this.brightnessTrackBar.TabIndex = 0;
            this.brightnessTrackBar.ThumbColor = System.Drawing.Color.Blue;
            this.brightnessTrackBar.Value = 255;
            this.brightnessTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.brightnessTrackBar_Scroll);
            // 
            // adaptiveBrightnessSwitch
            // 
            this.adaptiveBrightnessSwitch.Location = new System.Drawing.Point(237, 10);
            this.adaptiveBrightnessSwitch.Name = "adaptiveBrightnessSwitch";
            this.adaptiveBrightnessSwitch.Size = new System.Drawing.Size(60, 30);
            this.adaptiveBrightnessSwitch.TabIndex = 1;
            this.adaptiveBrightnessSwitch.CheckedChanged += new System.EventHandler(this.adaptiveBrightnessSwitch_CheckedChanged);
            // 
            // screenSaverSwitch
            // 
            this.screenSaverSwitch.Location = new System.Drawing.Point(237, 10);
            this.screenSaverSwitch.Name = "screenSaverSwitch";
            this.screenSaverSwitch.Size = new System.Drawing.Size(60, 30);
            this.screenSaverSwitch.TabIndex = 2;
            this.screenSaverSwitch.CheckedChanged += new System.EventHandler(this.screenSaverSwitch_CheckedChanged);
            // 
            // sleepModeSwitch
            // 
            this.sleepModeSwitch.Location = new System.Drawing.Point(237, 10);
            this.sleepModeSwitch.Name = "sleepModeSwitch";
            this.sleepModeSwitch.Size = new System.Drawing.Size(60, 30);
            this.sleepModeSwitch.TabIndex = 3;
            this.sleepModeSwitch.CheckedChanged += new System.EventHandler(this.sleepModeSwitch_CheckedChanged);
            // 
            // lblBrightness
            // 
            this.lblBrightness.AutoSize = true;
            this.lblBrightness.BackColor = System.Drawing.Color.Transparent;
            this.lblBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.lblBrightness.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblBrightness.Location = new System.Drawing.Point(394, 34);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(115, 22);
            this.lblBrightness.TabIndex = 4;
            this.lblBrightness.Text = "Brightness: 0";
            // 
            // lblAdaptiveBrightness
            // 
            this.lblAdaptiveBrightness.AutoSize = true;
            this.lblAdaptiveBrightness.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblAdaptiveBrightness.Location = new System.Drawing.Point(10, 15);
            this.lblAdaptiveBrightness.Name = "lblAdaptiveBrightness";
            this.lblAdaptiveBrightness.Size = new System.Drawing.Size(101, 13);
            this.lblAdaptiveBrightness.TabIndex = 5;
            this.lblAdaptiveBrightness.Text = "Adaptive Brightness";
            // 
            // lblScreenSaver
            // 
            this.lblScreenSaver.AutoSize = true;
            this.lblScreenSaver.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblScreenSaver.Location = new System.Drawing.Point(10, 15);
            this.lblScreenSaver.Name = "lblScreenSaver";
            this.lblScreenSaver.Size = new System.Drawing.Size(72, 13);
            this.lblScreenSaver.TabIndex = 6;
            this.lblScreenSaver.Text = "Screen Saver";
            // 
            // lblSleepMode
            // 
            this.lblSleepMode.AutoSize = true;
            this.lblSleepMode.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblSleepMode.Location = new System.Drawing.Point(10, 15);
            this.lblSleepMode.Name = "lblSleepMode";
            this.lblSleepMode.Size = new System.Drawing.Size(64, 13);
            this.lblSleepMode.TabIndex = 7;
            this.lblSleepMode.Text = "Sleep Mode";
            // 
            // adaptiveBrightnessPanel
            // 
            this.adaptiveBrightnessPanel.BackColor = System.Drawing.Color.White;
            this.adaptiveBrightnessPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.adaptiveBrightnessPanel.Controls.Add(this.adaptiveBrightnessSwitch);
            this.adaptiveBrightnessPanel.Controls.Add(this.lblAdaptiveBrightness);
            this.adaptiveBrightnessPanel.Location = new System.Drawing.Point(300, 195);
            this.adaptiveBrightnessPanel.Name = "adaptiveBrightnessPanel";
            this.adaptiveBrightnessPanel.Size = new System.Drawing.Size(360, 48);
            this.adaptiveBrightnessPanel.TabIndex = 8;
            // 
            // screenSaverPanel
            // 
            this.screenSaverPanel.BackColor = System.Drawing.Color.White;
            this.screenSaverPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenSaverPanel.Controls.Add(this.screenSaverSwitch);
            this.screenSaverPanel.Controls.Add(this.lblScreenSaver);
            this.screenSaverPanel.Enabled = false;
            this.screenSaverPanel.Location = new System.Drawing.Point(300, 300);
            this.screenSaverPanel.Name = "screenSaverPanel";
            this.screenSaverPanel.Size = new System.Drawing.Size(360, 50);
            this.screenSaverPanel.TabIndex = 9;
            // 
            // sleepModePanel
            // 
            this.sleepModePanel.BackColor = System.Drawing.Color.White;
            this.sleepModePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sleepModePanel.Controls.Add(this.sleepModeSwitch);
            this.sleepModePanel.Controls.Add(this.lblSleepMode);
            this.sleepModePanel.Enabled = false;
            this.sleepModePanel.Location = new System.Drawing.Point(300, 411);
            this.sleepModePanel.Name = "sleepModePanel";
            this.sleepModePanel.Size = new System.Drawing.Size(360, 50);
            this.sleepModePanel.TabIndex = 10;
            // 
            // btnBalanced
            // 
            this.btnBalanced.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBalanced.ForeColor = System.Drawing.Color.White;
            this.btnBalanced.Location = new System.Drawing.Point(300, 500);
            this.btnBalanced.Name = "btnBalanced";
            this.btnBalanced.Size = new System.Drawing.Size(100, 30);
            this.btnBalanced.TabIndex = 11;
            this.btnBalanced.Text = "Balanced";
            this.btnBalanced.Click += new System.EventHandler(this.btnBalanced_Click);
            // 
            // btnAlwaysReady
            // 
            this.btnAlwaysReady.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAlwaysReady.ForeColor = System.Drawing.Color.White;
            this.btnAlwaysReady.Location = new System.Drawing.Point(420, 500);
            this.btnAlwaysReady.Name = "btnAlwaysReady";
            this.btnAlwaysReady.Size = new System.Drawing.Size(120, 30);
            this.btnAlwaysReady.TabIndex = 12;
            this.btnAlwaysReady.Text = "Always Ready";
            this.btnAlwaysReady.Click += new System.EventHandler(this.btnAlwaysReady_Click);
            // 
            // btnRestMode
            // 
            this.btnRestMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRestMode.ForeColor = System.Drawing.Color.White;
            this.btnRestMode.Location = new System.Drawing.Point(560, 500);
            this.btnRestMode.Name = "btnRestMode";
            this.btnRestMode.Size = new System.Drawing.Size(100, 30);
            this.btnRestMode.TabIndex = 13;
            this.btnRestMode.Text = "Rest Mode";
            this.btnRestMode.Click += new System.EventHandler(this.btnRestMode_Click);
            // 
            // DisplaySettingsForm
            // 
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(976, 605);
            this.Controls.Add(this.btnRestMode);
            this.Controls.Add(this.btnAlwaysReady);
            this.Controls.Add(this.btnBalanced);
            this.Controls.Add(this.sleepModePanel);
            this.Controls.Add(this.screenSaverPanel);
            this.Controls.Add(this.adaptiveBrightnessPanel);
            this.Controls.Add(this.lblBrightness);
            this.Controls.Add(this.brightnessTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DisplaySettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Display Settings";
            this.Load += new System.EventHandler(this.DisplaySettingsForm_Load);
            this.adaptiveBrightnessPanel.ResumeLayout(false);
            this.adaptiveBrightnessPanel.PerformLayout();
            this.screenSaverPanel.ResumeLayout(false);
            this.screenSaverPanel.PerformLayout();
            this.sleepModePanel.ResumeLayout(false);
            this.sleepModePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}