using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class SoundSettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Guna2TrackBar mainTrackBar;
        private Guna2TrackBar notificationsTrackBar;
        private Label lblMainVolume;
        private Label lblNotificationsVolume;

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
            this.mainTrackBar = new Guna.UI2.WinForms.Guna2TrackBar();
            this.notificationsTrackBar = new Guna.UI2.WinForms.Guna2TrackBar();
            this.lblMainVolume = new System.Windows.Forms.Label();
            this.lblNotificationsVolume = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainTrackBar
            // 
            this.mainTrackBar.Location = new System.Drawing.Point(12, 63);
            this.mainTrackBar.Maximum = 15;
            this.mainTrackBar.Name = "mainTrackBar";
            this.mainTrackBar.Size = new System.Drawing.Size(360, 45);
            this.mainTrackBar.TabIndex = 0;
            this.mainTrackBar.ThumbColor = System.Drawing.Color.Blue;
            this.mainTrackBar.Value = 15;
            this.mainTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mainTrackBar_Scroll);
            // 
            // notificationsTrackBar
            // 
            this.notificationsTrackBar.Location = new System.Drawing.Point(12, 205);
            this.notificationsTrackBar.Maximum = 7;
            this.notificationsTrackBar.Name = "notificationsTrackBar";
            this.notificationsTrackBar.Size = new System.Drawing.Size(360, 45);
            this.notificationsTrackBar.TabIndex = 1;
            this.notificationsTrackBar.ThumbColor = System.Drawing.Color.Blue;
            this.notificationsTrackBar.Value = 7;
            this.notificationsTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.notificationsTrackBar_Scroll);
            // 
            // lblMainVolume
            // 
            this.lblMainVolume.AutoSize = true;
            this.lblMainVolume.BackColor = System.Drawing.Color.White;
            this.lblMainVolume.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblMainVolume.Location = new System.Drawing.Point(20, 30);
            this.lblMainVolume.Name = "lblMainVolume";
            this.lblMainVolume.Size = new System.Drawing.Size(107, 16);
            this.lblMainVolume.TabIndex = 4;
            this.lblMainVolume.Text = "Main Volume: 0";
            // 
            // lblNotificationsVolume
            // 
            this.lblNotificationsVolume.AutoSize = true;
            this.lblNotificationsVolume.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblNotificationsVolume.Location = new System.Drawing.Point(9, 169);
            this.lblNotificationsVolume.Name = "lblNotificationsVolume";
            this.lblNotificationsVolume.Size = new System.Drawing.Size(142, 16);
            this.lblNotificationsVolume.TabIndex = 5;
            this.lblNotificationsVolume.Text = "Notifications Volume: 0";
            // 
            // SoundSettingsForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(976, 605);
            this.Controls.Add(this.lblNotificationsVolume);
            this.Controls.Add(this.lblMainVolume);
            this.Controls.Add(this.notificationsTrackBar);
            this.Controls.Add(this.mainTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SoundSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sound Settings";
            this.Load += new System.EventHandler(this.SoundSettingsForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
