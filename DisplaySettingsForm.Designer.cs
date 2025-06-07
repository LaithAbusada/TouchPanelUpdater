using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class DisplaySettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2TrackBar brightnessTrackBar;
        private Guna.UI2.WinForms.Guna2ToggleSwitch adaptiveBrightnessSwitch;
        private Label lblBrightness;
        private Label lblAdaptiveBrightness;
        private Label lblSleepMode;
        private Panel adaptiveBrightnessPanel;
        private Guna.UI2.WinForms.Guna2Button btnAlwaysOn;
        private Guna.UI2.WinForms.Guna2Button btn1Min;
        private Guna.UI2.WinForms.Guna2Button btn5Min;
        private Guna.UI2.WinForms.Guna2Button btn10Min;
        private Guna.UI2.WinForms.Guna2Button btn30Min;
        private Guna.UI2.WinForms.Guna2Button btnPortrait;
        private Guna.UI2.WinForms.Guna2Button btnLandscape;

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
            this.lblBrightness = new System.Windows.Forms.Label();
            this.lblAdaptiveBrightness = new System.Windows.Forms.Label();
            this.lblSleepMode = new System.Windows.Forms.Label();
            this.adaptiveBrightnessPanel = new System.Windows.Forms.Panel();
            this.btnAlwaysOn = new Guna.UI2.WinForms.Guna2Button();
            this.btn1Min = new Guna.UI2.WinForms.Guna2Button();
            this.btn5Min = new Guna.UI2.WinForms.Guna2Button();
            this.btn10Min = new Guna.UI2.WinForms.Guna2Button();
            this.btn30Min = new Guna.UI2.WinForms.Guna2Button();
            this.btnPortrait = new Guna.UI2.WinForms.Guna2Button();
            this.btnLandscape = new Guna.UI2.WinForms.Guna2Button();
            this.displayLabel = new System.Windows.Forms.Label();
            this.btnLandscapeRight = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.adaptiveBrightnessPanel.SuspendLayout();
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
            // lblBrightness
            // 
            this.lblBrightness.AutoSize = true;
            this.lblBrightness.BackColor = System.Drawing.Color.Transparent;
            this.lblBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.lblBrightness.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblBrightness.Location = new System.Drawing.Point(394, 34);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(139, 26);
            this.lblBrightness.TabIndex = 4;
            this.lblBrightness.Text = "Brightness: 0";
            // 
            // lblAdaptiveBrightness
            // 
            this.lblAdaptiveBrightness.AutoSize = true;
            this.lblAdaptiveBrightness.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblAdaptiveBrightness.Location = new System.Drawing.Point(10, 15);
            this.lblAdaptiveBrightness.Name = "lblAdaptiveBrightness";
            this.lblAdaptiveBrightness.Size = new System.Drawing.Size(127, 16);
            this.lblAdaptiveBrightness.TabIndex = 5;
            this.lblAdaptiveBrightness.Text = "Adaptive Brightness";
            // 
            // lblSleepMode
            // 
            this.lblSleepMode.AutoSize = true;
            this.lblSleepMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.lblSleepMode.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblSleepMode.Location = new System.Drawing.Point(295, 261);
            this.lblSleepMode.Name = "lblSleepMode";
            this.lblSleepMode.Size = new System.Drawing.Size(128, 26);
            this.lblSleepMode.TabIndex = 16;
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
            // btnAlwaysOn
            // 
            this.btnAlwaysOn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAlwaysOn.ForeColor = System.Drawing.Color.White;
            this.btnAlwaysOn.Location = new System.Drawing.Point(300, 300);
            this.btnAlwaysOn.Name = "btnAlwaysOn";
            this.btnAlwaysOn.Size = new System.Drawing.Size(100, 30);
            this.btnAlwaysOn.TabIndex = 11;
            this.btnAlwaysOn.Text = "Always On";
            this.btnAlwaysOn.Click += new System.EventHandler(this.btnAlwaysOn_Click);
            // 
            // btn1Min
            // 
            this.btn1Min.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn1Min.ForeColor = System.Drawing.Color.White;
            this.btn1Min.Location = new System.Drawing.Point(420, 300);
            this.btn1Min.Name = "btn1Min";
            this.btn1Min.Size = new System.Drawing.Size(100, 30);
            this.btn1Min.TabIndex = 12;
            this.btn1Min.Text = "1 Minute";
            this.btn1Min.Click += new System.EventHandler(this.btn1Min_Click);
            // 
            // btn5Min
            // 
            this.btn5Min.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn5Min.ForeColor = System.Drawing.Color.White;
            this.btn5Min.Location = new System.Drawing.Point(540, 300);
            this.btn5Min.Name = "btn5Min";
            this.btn5Min.Size = new System.Drawing.Size(100, 30);
            this.btn5Min.TabIndex = 13;
            this.btn5Min.Text = "5 Minutes";
            this.btn5Min.Click += new System.EventHandler(this.btn5Min_Click);
            // 
            // btn10Min
            // 
            this.btn10Min.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn10Min.ForeColor = System.Drawing.Color.White;
            this.btn10Min.Location = new System.Drawing.Point(300, 350);
            this.btn10Min.Name = "btn10Min";
            this.btn10Min.Size = new System.Drawing.Size(100, 30);
            this.btn10Min.TabIndex = 14;
            this.btn10Min.Text = "10 Minutes";
            this.btn10Min.Click += new System.EventHandler(this.btn10Min_Click);
            // 
            // btn30Min
            // 
            this.btn30Min.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn30Min.ForeColor = System.Drawing.Color.White;
            this.btn30Min.Location = new System.Drawing.Point(420, 350);
            this.btn30Min.Name = "btn30Min";
            this.btn30Min.Size = new System.Drawing.Size(100, 30);
            this.btn30Min.TabIndex = 15;
            this.btn30Min.Text = "30 Minutes";
            this.btn30Min.Click += new System.EventHandler(this.btn30Min_Click);
            // 
            // btnPortrait
            // 
            this.btnPortrait.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnPortrait.ForeColor = System.Drawing.Color.White;
            this.btnPortrait.Location = new System.Drawing.Point(301, 433);
            this.btnPortrait.Name = "btnPortrait";
            this.btnPortrait.Size = new System.Drawing.Size(100, 39);
            this.btnPortrait.TabIndex = 17;
            this.btnPortrait.Text = "Portrait  Mode";
            this.btnPortrait.Click += new System.EventHandler(this.btnPortrait_Click);
            // 
            // btnLandscape
            // 
            this.btnLandscape.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLandscape.ForeColor = System.Drawing.Color.White;
            this.btnLandscape.Location = new System.Drawing.Point(436, 433);
            this.btnLandscape.Name = "btnLandscape";
            this.btnLandscape.Size = new System.Drawing.Size(100, 39);
            this.btnLandscape.TabIndex = 18;
            this.btnLandscape.Text = " Defaut Landscape";
            this.btnLandscape.Click += new System.EventHandler(this.btnLandscape_Click);
            // 
            // displayLabel
            // 
            this.displayLabel.AutoSize = true;
            this.displayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.displayLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.displayLabel.Location = new System.Drawing.Point(297, 399);
            this.displayLabel.Name = "displayLabel";
            this.displayLabel.Size = new System.Drawing.Size(137, 26);
            this.displayLabel.TabIndex = 19;
            this.displayLabel.Text = "Display Type";
            // 
            // btnLandscapeRight
            // 
            this.btnLandscapeRight.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLandscapeRight.ForeColor = System.Drawing.Color.White;
            this.btnLandscapeRight.Location = new System.Drawing.Point(560, 433);
            this.btnLandscapeRight.Name = "btnLandscapeRight";
            this.btnLandscapeRight.Size = new System.Drawing.Size(100, 39);
            this.btnLandscapeRight.TabIndex = 20;
            this.btnLandscapeRight.Text = "Landscape Right";
            this.btnLandscapeRight.Click += new System.EventHandler(this.btnLandscapeRight_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(297, 475);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 26);
            this.label1.TabIndex = 21;
            this.label1.Text = "Icon Size";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // guna2Button1
            // 
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(299, 516);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(100, 39);
            this.guna2Button1.TabIndex = 22;
            this.guna2Button1.Text = "Large Icons";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2Button2
            // 
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Location = new System.Drawing.Point(436, 516);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(100, 39);
            this.guna2Button2.TabIndex = 23;
            this.guna2Button2.Text = "Small Icons";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // DisplaySettingsForm
            // 
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(976, 605);
            this.Controls.Add(this.guna2Button2);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLandscapeRight);
            this.Controls.Add(this.displayLabel);
            this.Controls.Add(this.btnPortrait);
            this.Controls.Add(this.btnLandscape);
            this.Controls.Add(this.btn30Min);
            this.Controls.Add(this.btn10Min);
            this.Controls.Add(this.btn5Min);
            this.Controls.Add(this.btn1Min);
            this.Controls.Add(this.btnAlwaysOn);
            this.Controls.Add(this.lblSleepMode);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label displayLabel;
        private Guna.UI2.WinForms.Guna2Button btnLandscapeRight;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
    }
}