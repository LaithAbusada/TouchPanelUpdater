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
                    parentForm.clearMainPanel();
                    MessageBox.Show("No device is currently connected. Please connect a device before proceeding.",
                               "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                await LoadDisplaySettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isCheckingConnection = false;
            }
        }

        private async Task LoadDisplaySettings()
        {
            try
            {
                // Fetch current brightness
                string brightnessOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_brightness");

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

                // Fetch adaptive brightness setting
                string adaptiveBrightnessOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_brightness_mode");

                if (int.TryParse(adaptiveBrightnessOutput.Trim(), out int adaptiveBrightnessMode))
                {
                    adaptiveBrightnessSwitch.Checked = adaptiveBrightnessMode == 1;
                }
                else
                {
                    adaptiveBrightnessSwitch.Checked = false;
                }

                // Fetch current sleep mode value
                await UpdateSleepModeLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading display settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void adaptiveBrightnessSwitch_CheckedChanged(object sender, EventArgs e)
        {
            await CheckAndExecuteCommand(async () =>
            {
                try
                {
                    string newMode = adaptiveBrightnessSwitch.Checked ? "1" : "0";
                    await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_brightness_mode {newMode}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating adaptive brightness: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private async void brightnessTrackBar_Scroll(object sender, EventArgs e)
        {
            await CheckAndExecuteCommand(async () =>
            {
                try
                {
                    int brightness = brightnessTrackBar.Value;

                    lblBrightness.Text = $"Brightness: {brightness}";

                    await parentForm.ExecuteAdbCommand("adb shell settings put system screen_brightness_mode 0");
                    adaptiveBrightnessSwitch.Checked = false;

                    await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_brightness {brightness}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating brightness: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private async void btnAlwaysOn_Click(object sender, EventArgs e)
        {
            await SetSleepMode("0", "Always On"); // Disable sleep mode
        }

        private async void btn1Min_Click(object sender, EventArgs e)
        {
            await SetSleepMode("60000", "1 Minute"); // Set sleep mode to 1 minute
        }

        private async void btn5Min_Click(object sender, EventArgs e)
        {
            await SetSleepMode("300000", "5 Minutes"); // Set sleep mode to 5 minutes
        }

        private async void btn10Min_Click(object sender, EventArgs e)
        {
            await SetSleepMode("600000", "10 Minutes"); // Set sleep mode to 10 minutes
        }

        private async void btn30Min_Click(object sender, EventArgs e)
        {
            await SetSleepMode("1800000", "30 Minutes"); // Set sleep mode to 30 minutes
        }

        private async Task SetSleepMode(string timeoutValue, string modeName)
        {
            await CheckAndExecuteCommand(async () =>
            {
                try
                {
                    // Set screen_off_timeout
                    await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_off_timeout {timeoutValue}");

                    // Set sleep_timeout based on whether it's "Always On" or not
                    string sleepTimeoutValue = timeoutValue == "0" ? "-1" : "1";
                    await parentForm.ExecuteAdbCommand($"adb shell settings put secure sleep_timeout {sleepTimeoutValue}");

                    // Update the label with the new sleep mode
                    lblSleepMode.Text = $"Sleep Mode: {modeName}";

                    // Show a message indicating the mode is now on
                    MessageBox.Show($"Updated sleep mode to {modeName}.", "Mode Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting sleep mode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private async Task UpdateSleepModeLabel()
        {
            try
            {
                string screenOffTimeoutOutput = await parentForm.ExecuteAdbCommand("adb shell settings get system screen_off_timeout");
                if (int.TryParse(screenOffTimeoutOutput.Trim(), out int timeoutValue))
                {
                    string modeName;
                    switch (timeoutValue)
                    {
                        case 0:
                            modeName = "Always On";
                            break;
                        case 60000:
                            modeName = "1 Minute";
                            break;
                        case 300000:
                            modeName = "5 Minutes";
                            break;
                        case 600000:
                            modeName = "10 Minutes";
                            break;
                        case 1800000:
                            modeName = "30 Minutes";
                            break;
                        default:
                            modeName = $"{timeoutValue / 60000} Minutes";
                            break;
                    }
                    lblSleepMode.Text = $"Sleep Mode: {modeName}";
                }
                else
                {
                    lblSleepMode.Text = "Sleep Mode: N/A";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving sleep mode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during execution: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isCheckingConnection = false;
            }
        }
    }
}
