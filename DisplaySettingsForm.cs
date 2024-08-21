using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class DisplaySettingsForm : Form
    {
        private Form1 parentForm;
        private bool isCheckingConnection;
        private SettingsForm settingsForm;
        public DisplaySettingsForm(Form1 parent, SettingsForm settingsForm)
        {
            InitializeComponent();
            parentForm = parent;
            isCheckingConnection = false;
            this.settingsForm = settingsForm;
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
                bool isConnected = await parentForm.IsConnected();
                await settingsForm.TriggerConnectionStatusUpdate(isConnected);
                if (!isConnected)
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
                    // Disable all buttons in both forms
                    settingsForm.DisableAllButtons();
                    DisableAllButtons();

                    // Apply the appropriate settings based on the timeoutValue
                    if (timeoutValue == "0") // "Always On" mode
                    {
                        await parentForm.ExecuteAdbCommand("adb shell settings put secure sleep_timeout -1");
                        await parentForm.ExecuteAdbCommand("adb shell settings put system screen_off_timeout 2147483647");

                        // Update the label with the new sleep mode
                        lblSleepMode.Text = $"Sleep Mode: {modeName}";

                        // Show a message indicating the mode is now on
                        MessageBox.Show($"Updated sleep mode to {modeName}. The device will now reboot.", "Mode Change", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Show the loading form during the reboot process
                        using (var loadingForm = new LoadingForm("Rebooting, please wait..."))
                        {
                            loadingForm.Show();

                            // Reboot the device
                            await parentForm.ExecuteAdbCommand("adb reboot");

                            // Wait 30 seconds to ensure the reboot process completes
                            await Task.Delay(30000);

                            // Close the loading form after the wait
                            loadingForm.Close();
                        }
                    }
                    else
                    {
                        await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_off_timeout {timeoutValue}");
                        string sleepTimeoutValue = "1";
                        await parentForm.ExecuteAdbCommand($"adb shell settings put secure sleep_timeout {sleepTimeoutValue}");

                        // Update the label with the new sleep mode
                        lblSleepMode.Text = $"Sleep Mode: {modeName}";

                        // Show a message indicating the mode is now on
                        MessageBox.Show($"Updated sleep mode to {modeName}.", "Mode Change", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Wait a brief moment to ensure settings are applied (optional)
                        await Task.Delay(1000);
                    }

                    // Re-enable all buttons after the settings are applied
                    settingsForm.EnableAllButtons();
                    EnableAllButtons();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting sleep mode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    isCheckingConnection = false;
                }
            });
        }
        private void DisableAllButtons()
        {
            btnAlwaysOn.Enabled = false;
            btn1Min.Enabled = false;
            btn5Min.Enabled = false;
            btn10Min.Enabled = false;
            btn30Min.Enabled = false;
            brightnessTrackBar.Enabled = false;
            adaptiveBrightnessSwitch.Enabled = false;
        }

        private void EnableAllButtons()
        {
            btnAlwaysOn.Enabled = true;
            btn1Min.Enabled = true;
            btn5Min.Enabled = true;
            btn10Min.Enabled = true;
            btn30Min.Enabled = true;
            brightnessTrackBar.Enabled = true;
            adaptiveBrightnessSwitch.Enabled = true;
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
                        case 2147483647:
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
