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

            var volumeLevels = await parentForm.GetCurrentVolumeLevels();
            soundTrackBar.Value = volumeLevels["Media"];
            lblMediaVolume.Text = $"Media Volume: {soundTrackBar.Value}";

            notificationsTrackBar.Value = volumeLevels["Notifications"];
            lblNotificationsVolume.Text = $"Notifications Volume: {notificationsTrackBar.Value}";

            alarmTrackBar.Value = volumeLevels["Alarm"];
            lblAlarmVolume.Text = $"Alarm Volume: {alarmTrackBar.Value}";

            bluetoothTrackBar.Value = volumeLevels["Bluetooth"];
            lblBluetoothVolume.Text = $"Bluetooth Volume: {bluetoothTrackBar.Value}";
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

        private async void soundTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblMediaVolume.Text = $"Media Volume: {soundTrackBar.Value}";
            await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 3 --set {soundTrackBar.Value}");
        }

        private async void notificationsTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SettingsForm settingsForm = new SettingsForm(parentForm);
                parentForm.LoadFormIntoPanel(settingsForm);
                ClearControls();
                return;
            }

            lblNotificationsVolume.Text = $"Notifications Volume: {notificationsTrackBar.Value}";
            await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 5 --set {notificationsTrackBar.Value}");
        }

        private async void alarmTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblAlarmVolume.Text = $"Alarm Volume: {alarmTrackBar.Value}";
            await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 4 --set {alarmTrackBar.Value}");
        }

        private async void bluetoothTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblBluetoothVolume.Text = $"Bluetooth Volume: {bluetoothTrackBar.Value}";
            await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 4 --set {bluetoothTrackBar.Value}"); // Use appropriate stream if different for Bluetooth
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
