using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Innovo_TP4_Updater
{
    public partial class SoundSettingsForm : Form
    {
        private Form1 parentForm;

        public SoundSettingsForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private async void SoundSettingsForm_Load_1(object sender, EventArgs e)
        {
            CenterControls();

            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearControls();
                return;
            }

            try
            {
                // Fetching the volume levels directly using ExecuteAdbCommand
                string mediaVolumeOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system volume_music");
                string notificationsVolumeOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system volume_notification");
                string alarmVolumeOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system volume_alarm");
                string bluetoothVolumeOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system volume_bluetooth_sco");

                // Parsing the output and setting the trackbars
                soundTrackBar.Value = int.TryParse(mediaVolumeOutput.Trim(), out int mediaVolume) ? mediaVolume : 0;
                lblMediaVolume.Text = $"Media Volume: {soundTrackBar.Value}";
                soundTrackBar.Maximum = 15;
                soundTrackBar.Minimum = 0;

                notificationsTrackBar.Value = int.TryParse(notificationsVolumeOutput.Trim(), out int notificationsVolume) ? notificationsVolume : 0;
                lblNotificationsVolume.Text = $"Notifications Volume: {notificationsTrackBar.Value}";
                notificationsTrackBar.Maximum = 15;
                notificationsTrackBar.Minimum = 0;

                alarmTrackBar.Value = int.TryParse(alarmVolumeOutput.Trim(), out int alarmVolume) ? alarmVolume : 0;
                lblAlarmVolume.Text = $"Alarm Volume: {alarmTrackBar.Value}";
                alarmTrackBar.Maximum = 15;
                alarmTrackBar.Minimum = 0;

                bluetoothTrackBar.Value = int.TryParse(bluetoothVolumeOutput.Trim(), out int bluetoothVolume) ? bluetoothVolume : 0;
                lblBluetoothVolume.Text = $"Bluetooth Volume: {bluetoothTrackBar.Value}";
                bluetoothTrackBar.Maximum = 15;
                bluetoothTrackBar.Minimum = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving volume levels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearControls();
            }
        }


        private void ClearControls()
        {
            soundTrackBar.Value = 0;
            lblMediaVolume.Text = "Media Volume: N/A";

            notificationsTrackBar.Value = 0;
            lblNotificationsVolume.Text = "Notifications Volume: N/A";

            alarmTrackBar.Value = 0;
            lblAlarmVolume.Text = "Alarm Volume: N/A";

            bluetoothTrackBar.Value = 0;
            lblBluetoothVolume.Text = "Bluetooth Volume: N/A";
        }

        private async void soundTrackBar_Scroll(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lblMediaVolume.Text = $"Media Volume: {soundTrackBar.Value}";
                await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 3 --set {soundTrackBar.Value}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting media volume: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void notificationsTrackBar_Scroll(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lblNotificationsVolume.Text = $"Notifications Volume: {notificationsTrackBar.Value}";
                await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 5 --set {notificationsTrackBar.Value}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting notifications volume: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void alarmTrackBar_Scroll(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lblAlarmVolume.Text = $"Alarm Volume: {alarmTrackBar.Value}";
                await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 4 --set {alarmTrackBar.Value}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting alarm volume: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void bluetoothTrackBar_Scroll(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lblBluetoothVolume.Text = $"Bluetooth Volume: {bluetoothTrackBar.Value}";
                await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 6 --set {bluetoothTrackBar.Value}");  // Use stream 6 for Bluetooth SCO
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting Bluetooth volume: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void lblMediaVolume_Click(object sender, EventArgs e)
        {
            // Optional: handle label click event if necessary
        }

        private void CenterControls()
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            lblMediaVolume.Location = new Point((formWidth - lblMediaVolume.Width) / 2, 20);
            soundTrackBar.Location = new Point((formWidth - soundTrackBar.Width) / 2, lblMediaVolume.Bottom + 30);

            lblNotificationsVolume.Location = new Point((formWidth - lblNotificationsVolume.Width) / 2, soundTrackBar.Bottom + 40);
            notificationsTrackBar.Location = new Point((formWidth - notificationsTrackBar.Width) / 2, lblNotificationsVolume.Bottom + 30);

            lblAlarmVolume.Location = new Point((formWidth - lblAlarmVolume.Width) / 2, notificationsTrackBar.Bottom + 40);
            alarmTrackBar.Location = new Point((formWidth - alarmTrackBar.Width) / 2, lblAlarmVolume.Bottom + 30);

            lblBluetoothVolume.Location = new Point((formWidth - lblBluetoothVolume.Width) / 2, alarmTrackBar.Bottom + 40);
            bluetoothTrackBar.Location = new Point((formWidth - bluetoothTrackBar.Width) / 2, lblBluetoothVolume.Bottom + 30);
        }
    }
}
