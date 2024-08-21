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
        private SettingsForm settingsForm;
        public SoundSettingsForm(Form1 parent, SettingsForm settingsForm)
        {
            InitializeComponent();
            parentForm = parent;
            this.settingsForm = settingsForm;
        }

        private async void SoundSettingsForm_Load_1(object sender, EventArgs e)
        {
            CenterControls();
            bool isConnected = await parentForm.IsConnected();
            await settingsForm.TriggerConnectionStatusUpdate(isConnected);

            if (!isConnected)
            {
                parentForm.clearMainPanel();
                MessageBox.Show("No device is currently connected. Please connect a device before proceeding.",
                               "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadingForm loadingForm = new LoadingForm("Please wait getting sound settings");

            try
            {

                loadingForm.Show();
                // Set the range for Main Volume and Notifications Volume
                mainTrackBar.Minimum = 0;
                mainTrackBar.Maximum = 15;

                notificationsTrackBar.Minimum = 0;
                notificationsTrackBar.Maximum = 7;
                // Correct ADB command for retrieving volume levels
                string mainVolumeOutput = await parentForm.ExecuteAdbCommand("adb shell media volume --get --stream 3");
                string notificationsVolumeOutput = await parentForm.ExecuteAdbCommand("adb shell media volume --get --stream 5");

                // Parsing the output, extracting the volume
                int mainVolume = ParseVolume(mainVolumeOutput, 15); // Main volume range 0-15
                int notificationsVolume = ParseVolume(notificationsVolumeOutput, 7); // Notifications volume range 0-7

                // Set trackbar values
                mainTrackBar.Value = mainVolume;
                lblMainVolume.Text = $"Main Volume: {mainTrackBar.Value}";

                notificationsTrackBar.Value = notificationsVolume;
                lblNotificationsVolume.Text = $"Notifications Volume: {notificationsTrackBar.Value}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving volume levels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                parentForm.clearMainPanel(); ;
            }
            finally
            {
                loadingForm.Close();
            }
        }

        private int ParseVolume(string volumeOutput, int maxVolume)
        {
            // Example parsing logic for the output format "volume is x in range [0..y]"
            const string volumePrefix = "volume is ";
            int volumeIndex = volumeOutput.IndexOf(volumePrefix);
            if (volumeIndex >= 0)
            {
                string volumeString = volumeOutput.Substring(volumeIndex + volumePrefix.Length).Split(' ')[0];
                if (int.TryParse(volumeString, out int volume))
                {
                    return Math.Min(volume, maxVolume); // Ensure volume is within the expected range
                }
            }
            return 0; // Default to 0 if parsing fails
        }

   

        private async void mainTrackBar_Scroll(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lblMainVolume.Text = $"Main Volume: {mainTrackBar.Value}";
                await parentForm.ExecuteAdbCommand($"adb shell media volume --show --stream 3 --set {mainTrackBar.Value}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting main volume: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void CenterControls()
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            lblMainVolume.Location = new Point((formWidth - lblMainVolume.Width) / 2, 20);
            mainTrackBar.Location = new Point((formWidth - mainTrackBar.Width) / 2, lblMainVolume.Bottom + 30);

            lblNotificationsVolume.Location = new Point((formWidth - lblNotificationsVolume.Width) / 2, mainTrackBar.Bottom + 40);
            notificationsTrackBar.Location = new Point((formWidth - notificationsTrackBar.Width) / 2, lblNotificationsVolume.Bottom + 30);
        }
    }
}
