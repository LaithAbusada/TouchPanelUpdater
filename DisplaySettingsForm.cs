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
                    parentForm.clearMainPanel();

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
            sleepModeSwitch.Checked = int.Parse(screenOffTimeoutOutput.Trim()) > 0; // Assuming 5 minutes as on for sleep mode
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
                    parentForm.clearMainPanel();
                       
                    return;
                }

                await command();
            }
            finally
            {
                isCheckingConnection = false;
            }
        }

        private async void btnBalanced_Click(object sender, EventArgs e)
        {
            await SetDisplayMode(true, true); // Screen saver on, sleep mode on
        }

        private async void btnAlwaysReady_Click(object sender, EventArgs e)
        {
            await SetDisplayMode(false, false); // Screen saver off, sleep mode off
        }

        private async void btnRestMode_Click(object sender, EventArgs e)
        {
            await SetDisplayMode(false, true); // Screen saver off, sleep mode on
        }

        private async Task SetDisplayMode(bool screenSaver, bool sleepMode)
        {
            await CheckAndExecuteCommand(async () =>
            {
                // Set screen saver
                string screenSaverSetting = screenSaver ? "1" : "0";
                await parentForm.ExecuteAdbCommand($"adb shell settings put secure screensaver_enabled {screenSaverSetting}");
                screenSaverSwitch.Checked = screenSaver;

                // Set sleep mode
                string sleepModeSetting = sleepMode ? "300000" : "0"; // 5 minutes for sleep mode
                await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_off_timeout {sleepModeSetting}");
                sleepModeSwitch.Checked = sleepMode;
            });
        }
    }
}
