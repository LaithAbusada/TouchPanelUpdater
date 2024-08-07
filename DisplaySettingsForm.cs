using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class DisplaySettingsForm : Form
    {
        private Form1 parentForm;
        private bool isCheckingConnection;

        public DisplaySettingsForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
            isCheckingConnection = false;
        }

        private async void DisplaySettingsForm_Load(object sender, EventArgs e)
        {
            await CheckAndLoadSettings();
        }

        private async Task CheckAndLoadSettings()
        {
            if (isCheckingConnection) return;

            isCheckingConnection = true;

            try
            {
                if (!await parentForm.IsConnected())
                {
                    MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearControls();
                    return;
                }

                await LoadDisplaySettings();
            }
            finally
            {
                isCheckingConnection = false;
            }
        }

        private async Task LoadDisplaySettings()
        {
            // Fetch current brightness and adaptive brightness state
            string brightnessOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_brightness");
            string adaptiveBrightnessOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_brightness_mode");
            string screenSaverOutput = await parentForm.ExecuteAdbCommand("adb shell settings get secure screensaver_enabled");
            string screenOffTimeoutOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_off_timeout");

            if (int.TryParse(brightnessOutput.Trim(), out int brightness))
            {
                brightnessTrackBar.Value = brightness;
                lblBrightness.Text = $"Brightness: {brightness}";
            }
            else
            {
                brightnessTrackBar.Value = 0;
                lblBrightness.Text = "Brightness: N/A";
            }

            adaptiveBrightnessSwitch.Checked = adaptiveBrightnessOutput.Trim() == "1";
            screenSaverSwitch.Checked = screenSaverOutput.Trim() == "1"; // Assuming 1 means enabled
            sleepModeSwitch.Checked = int.Parse(screenOffTimeoutOutput.Trim())  > 0; // Assuming 5 minutes as on for sleep mode
        }

        private void ClearControls()
        {
            brightnessTrackBar.Value = 0;
            lblBrightness.Text = "Brightness: N/A";

            adaptiveBrightnessSwitch.Checked = false;
            screenSaverSwitch.Checked = false;
            sleepModeSwitch.Checked = false;
        }

        private async void adaptiveBrightnessSwitch_CheckedChanged(object sender, EventArgs e)
        {
            await CheckAndExecuteCommand(async () =>
            {
                string newMode = adaptiveBrightnessSwitch.Checked ? "1" : "0";
                await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_brightness_mode {newMode}");
            });
        }

        private async void brightnessTrackBar_Scroll(object sender, EventArgs e)
        {
            await CheckAndExecuteCommand(async () =>
            {
                int brightness = brightnessTrackBar.Value;

                lblBrightness.Text = $"Brightness: {brightness}";

                await parentForm.ExecuteAdbCommand("adb shell settings put system screen_brightness_mode 0");
                adaptiveBrightnessSwitch.Checked = false;

                await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_brightness {brightness}");
            });
        }

        private async void screenSaverSwitch_CheckedChanged(object sender, EventArgs e)
        {
            await CheckAndExecuteCommand(async () =>
            {
                string newSetting = screenSaverSwitch.Checked ? "1" : "0"; // Toggle between enabled and disabled
                await parentForm.ExecuteAdbCommand($"adb shell settings put secure screensaver_enabled {newSetting}");
            });
        }

        private async void sleepModeSwitch_CheckedChanged(object sender, EventArgs e)
        {
            await CheckAndExecuteCommand(async () =>
            {
                string newSetting = sleepModeSwitch.Checked ? "300000" : "0"; // Toggle between 5 minutes and off
                await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_off_timeout {newSetting}");
            });
        }

        private async Task CheckAndExecuteCommand(Func<Task> command)
        {
            if (isCheckingConnection) return;

            isCheckingConnection = true;

            try
            {
                if (!await parentForm.IsConnected())
                {
                    MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearControls();
                    return;
                }

                await command();
            }
            finally
            {
                isCheckingConnection = false;
            }
        }
    }
}
