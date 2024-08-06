using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class SoundSettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Guna2TrackBar soundTrackBar;
        private Guna2TrackBar notificationsTrackBar;
        private Guna2TrackBar alarmTrackBar;
        private Guna2TrackBar bluetoothTrackBar;
        private Label lblMediaVolume;
        private Label lblNotificationsVolume;
        private Label lblAlarmVolume;
        private Label lblBluetoothVolume;

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
            this.soundTrackBar = new Guna.UI2.WinForms.Guna2TrackBar();
            this.notificationsTrackBar = new Guna.UI2.WinForms.Guna2TrackBar();
            this.alarmTrackBar = new Guna.UI2.WinForms.Guna2TrackBar();
            this.bluetoothTrackBar = new Guna.UI2.WinForms.Guna2TrackBar();
            this.lblMediaVolume = new System.Windows.Forms.Label();
            this.lblNotificationsVolume = new System.Windows.Forms.Label();
            this.lblAlarmVolume = new System.Windows.Forms.Label();
            this.lblBluetoothVolume = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // soundTrackBar
            // 
            this.soundTrackBar.Location = new System.Drawing.Point(12, 63);
            this.soundTrackBar.Maximum = 15;
            this.soundTrackBar.Name = "soundTrackBar";
            this.soundTrackBar.Size = new System.Drawing.Size(360, 45);
            this.soundTrackBar.TabIndex = 0;
            this.soundTrackBar.ThumbColor = System.Drawing.Color.Blue;
            this.soundTrackBar.Value = 15;
            this.soundTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.soundTrackBar_Scroll);
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
            // alarmTrackBar
            // 
            this.alarmTrackBar.Location = new System.Drawing.Point(12, 348);
            this.alarmTrackBar.Maximum = 7;
            this.alarmTrackBar.Name = "alarmTrackBar";
            this.alarmTrackBar.Size = new System.Drawing.Size(360, 45);
            this.alarmTrackBar.TabIndex = 2;
            this.alarmTrackBar.ThumbColor = System.Drawing.Color.Blue;
            this.alarmTrackBar.Value = 7;
            this.alarmTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.alarmTrackBar_Scroll);
            // 
            // bluetoothTrackBar
            // 
            this.bluetoothTrackBar.Location = new System.Drawing.Point(12, 493);
            this.bluetoothTrackBar.Maximum = 15;
            this.bluetoothTrackBar.Name = "bluetoothTrackBar";
            this.bluetoothTrackBar.Size = new System.Drawing.Size(360, 46);
            this.bluetoothTrackBar.TabIndex = 3;
            this.bluetoothTrackBar.ThumbColor = System.Drawing.Color.Blue;
            this.bluetoothTrackBar.Value = 15;
            this.bluetoothTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.bluetoothTrackBar_Scroll);
            // 
            // lblMediaVolume
            // 
            this.lblMediaVolume.AutoSize = true;
            this.lblMediaVolume.BackColor = System.Drawing.Color.White;
            this.lblMediaVolume.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblMediaVolume.Location = new System.Drawing.Point(20, 30);
            this.lblMediaVolume.Name = "lblMediaVolume";
            this.lblMediaVolume.Size = new System.Drawing.Size(107, 16);
            this.lblMediaVolume.TabIndex = 4;
            this.lblMediaVolume.Text = "Media Volume: 0";
            this.lblMediaVolume.Click += new System.EventHandler(this.lblMediaVolume_Click);
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
            // lblAlarmVolume
            // 
            this.lblAlarmVolume.AutoSize = true;
            this.lblAlarmVolume.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblAlarmVolume.Location = new System.Drawing.Point(12, 295);
            this.lblAlarmVolume.Name = "lblAlarmVolume";
            this.lblAlarmVolume.Size = new System.Drawing.Size(104, 16);
            this.lblAlarmVolume.TabIndex = 6;
            this.lblAlarmVolume.Text = "Alarm Volume: 0";
            // 
            // lblBluetoothVolume
            // 
            this.lblBluetoothVolume.AutoSize = true;
            this.lblBluetoothVolume.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblBluetoothVolume.Location = new System.Drawing.Point(9, 453);
            this.lblBluetoothVolume.Name = "lblBluetoothVolume";
            this.lblBluetoothVolume.Size = new System.Drawing.Size(125, 16);
            this.lblBluetoothVolume.TabIndex = 7;
            this.lblBluetoothVolume.Text = "Bluetooth Volume: 0";
            // 
            // SoundSettingsForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(976, 605);
            this.Controls.Add(this.lblBluetoothVolume);
            this.Controls.Add(this.lblAlarmVolume);
            this.Controls.Add(this.lblNotificationsVolume);
            this.Controls.Add(this.lblMediaVolume);
            this.Controls.Add(this.bluetoothTrackBar);
            this.Controls.Add(this.alarmTrackBar);
            this.Controls.Add(this.notificationsTrackBar);
            this.Controls.Add(this.soundTrackBar);
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
