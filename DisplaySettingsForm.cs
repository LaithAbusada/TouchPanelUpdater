using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class DisplaySettingsForm : Form
    {
        private Form1 parentForm;

        public DisplaySettingsForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private async void DisplaySettingsForm_Load(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           
            await LoadDisplaySettings();
        }

        private async Task LoadDisplaySettings()
        {
            // Fetch current brightness and adaptive brightness state
            string brightnessOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_brightness");
            string adaptiveBrightnessOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_brightness_mode");
            string screenSaverOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_off_timeout");
            string sleepModeOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system sleep_timeout");

            if (int.TryParse(brightnessOutput.Trim(), out int brightness))
            {
                brightnessTrackBar.Value = brightness;
                lblBrightness.Text = $"Brightness: {brightness}";
            }

            adaptiveBrightnessSwitch.Checked = adaptiveBrightnessOutput.Trim() == "1";
            screenSaverSwitch.Checked = screenSaverOutput.Trim() == "60000"; // Assuming 1 minute as on
            sleepModeSwitch.Checked = sleepModeOutput.Trim() == "300000"; // Assuming 5 minutes as on
        }

        private async void adaptiveBrightnessSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string newMode = adaptiveBrightnessSwitch.Checked ? "1" : "0";
            await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_brightness_mode {newMode}");
        }

        private async void brightnessTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Disable adaptive brightness before setting the brightness
            int brightness = brightnessTrackBar.Value;

            lblBrightness.Text = $"Brightness: {brightness}";

            await parentForm.ExecuteAdbCommand("adb shell settings put system screen_brightness_mode 0");
            adaptiveBrightnessSwitch.Checked = false;

            await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_brightness {brightness}");
        }

        private async void screenSaverSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string newSetting = screenSaverSwitch.Checked ? "60000" : "0"; // Toggle between 1 minute and off
            await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_off_timeout {newSetting}");
        }

        private async void sleepModeSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string newSetting = sleepModeSwitch.Checked ? "300000" : "0"; // Toggle between 5 minutes and off
            await parentForm.ExecuteAdbCommand($"adb shell settings put system sleep_timeout {newSetting}");
        }

       
    }
}
