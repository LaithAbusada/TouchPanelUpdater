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

        string DisplayType;

        private static string NormalizeVersion(string version)
        {
            // Split the version into parts
            var parts = version.Split('.');

            // Add ".0" for missing parts up to 4 components (as `Version` class can handle versions with up to 4 parts)
            while (parts.Length < 4)
            {
                version += ".0";
                parts = version.Split('.');
            }

            return version;
        }

        private async Task LoadDisplaySettings()
        {
            try
            {
                // Fetch the Android version
                string androidVersionOutput = await parentForm.ExecuteAdbCommand("adb shell getprop ro.build.version.release");


                Version deviceVersion = new Version(NormalizeVersion(androidVersionOutput));
                // Check if Android version is 13 or higher

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





                Version version13 = new Version(NormalizeVersion("13"));
       
                if (deviceVersion >= version13)
                {
                    // Fetch display type (user rotation) only for Android 13 or higher
                    DisplayType = await parentForm.ExecuteAdbCommand("adb shell settings get system user_rotation");

                    showDisplayType();

                    // Update the displayLabel based on the user rotation setting
                    UpdateDisplayLabel(DisplayType);
                }
                else
                {
                    HideDisplayType();
                }

                // Fetch current sleep mode valueUODATE
                await UpdateSleepModeLabel();
                }
              
            
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading display settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showDisplayType()
        {
            displayLabel.Visible = true;
            btnPortrait.Visible = true;
            btnLandscape.Visible = true;
            btnLandscapeRight.Visible = true;
        }
    
        private void HideDisplayType()
        {
            displayLabel.Visible = false;
            btnPortrait.Visible = false;
            btnLandscape.Visible = false;
            btnLandscapeRight.Visible = false;
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
                    string deviceModel = await parentForm.ExecuteAdbCommand("adb shell getprop ro.product.model");

                    if (deviceModel.ToLower().Contains("p5"))
                    {
                        var result = MessageBox.Show("The device will reboot after changes. Do you want to proceed?", "Reboot Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            return; // Cancel the operation if the user doesn't confirm
                        }
                    }

                    // Disable all buttons in both forms
                    settingsForm.DisableAllButtons();
                    DisableAllButtons();

                    if (deviceModel.ToLower().Contains("p5"))
                    {
                        await parentForm.ExecuteAdbCommand("adb shell settings put secure sleep_timeout -1");
                        await parentForm.ExecuteAdbCommand("adb shell settings put system screen_off_timeout 2147483647");
                        // If the device model contains "p5", use the screen_backlight command instead
                        if (timeoutValue == "0") // "Always On" mode
                        {
                            await parentForm.ExecuteAdbCommand("adb shell settings put system screen_backlight 2147483647");
                        }
                        else
                        {
                            await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_backlight {timeoutValue}");
                        }

                        // Update the label with the new sleep mode
                        lblSleepMode.Text = $"Sleep Mode: {modeName}";

                        MessageBox.Show($"Updated sleep mode to {modeName}. The device will now reboot.", "Mode Change", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Show the loading form during the reboot process
                        using (var loadingForm = new LoadingForm("Rebooting, please wait..."))
                        {
                            loadingForm.Show();

                            // Reboot the device
                            await parentForm.ExecuteAdbCommand("adb reboot");

                            // Wait 30-45 seconds to ensure the reboot process completes
                            await Task.Delay(45000);

                            // Close the loading form after the wait
                            loadingForm.Close();
                        }
                    }
                    else
                    {
                        // For non-p5 devices, use the existing logic for screen_off_timeout and sleep_timeout
                        if (timeoutValue == "0") // "Always On" mode
                        {
                            await parentForm.ExecuteAdbCommand("adb shell settings put secure sleep_timeout -1");
                            await parentForm.ExecuteAdbCommand("adb shell settings put system screen_off_timeout 2147483647");
                        }
                        else
                        {
                            await parentForm.ExecuteAdbCommand($"adb shell settings put system screen_off_timeout {timeoutValue}");

                            // Check if the device model contains "Innovo"
                            if (deviceModel.Contains("Innovo"))
                            {
                                // Set both screen saver and sleep mode to the same value
                                await parentForm.ExecuteAdbCommand($"adb shell settings put secure sleep_timeout {timeoutValue}");
                            }
                            else
                            {
                                // Only set the sleep mode
                                string sleepTimeoutValue = "1";
                                await parentForm.ExecuteAdbCommand($"adb shell settings put secure sleep_timeout {sleepTimeoutValue}");
                            }
                        }

                        // Update the label with the new sleep mode
                        lblSleepMode.Text = $"Sleep Mode: {modeName}";

                        MessageBox.Show($"Updated sleep mode to {modeName}.", "Mode Change", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Wait briefly to ensure settings are applied
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
            btnPortrait.Enabled = false; // Added this line for new buttons
            btnLandscape.Enabled = false; // Added this line for new buttons
            btnLandscapeRight.Enabled = false; // Added this line for new buttons
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
            btnPortrait.Enabled = true; // Added this line for new buttons
            btnLandscape.Enabled = true; // Added this line for new buttons
            btnLandscapeRight.Enabled = true; // Added this line for new buttons
        }

        private async void btnPortrait_Click(object sender, EventArgs e)
        {
            await SetDisplayMode("Portrait", 0, 0);
        }

        private async void btnLandscape_Click(object sender, EventArgs e)
        {
            await SetDisplayMode("Default Landscape", 1, 90);
        }
        private async Task SetDisplayMode(string mode, int userRotation, int hdmiOrientation)
        {
            await CheckAndExecuteCommand(async () =>
            {
                try
                {
                    // Ask the user if they want to reboot before applying the changes
                    var result = MessageBox.Show($"The device will reboot after applying the changes. Do you want to reboot and set the display to {mode} Mode?",
                                                 "Reboot Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Disable all buttons before proceeding
                        DisableAllButtons();
                        settingsForm.DisableAllButtons();


                        // Disable auto-rotate and set user rotation to the selected mode
                        await parentForm.ExecuteAdbCommand("adb shell settings put system accelerometer_rotation 0");

                        // Set HDMI orientation and user rotation based on mode
                        await parentForm.ExecuteAdbCommand($"adb shell settings put system hdmi_orientation {hdmiOrientation}");
                        await parentForm.ExecuteAdbCommand($"adb shell settings put system user_rotation {userRotation}");

                        // Update the displayLabel
                        displayLabel.Text = $"Display: {mode}";

                        MessageBox.Show($"Display set to {mode} Mode", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Show the loading form during the reboot process
                        using (var loadingForm = new LoadingForm("Rebooting, please wait...(This can take up to 45 seconds)"))
                        {
                            loadingForm.Show();

                            // Reboot the device
                            await parentForm.ExecuteAdbCommand("adb reboot");

                            // Wait 30-45 seconds to ensure the reboot process completes
                            await Task.Delay(45000);

                            // Close the loading form after the wait
                            loadingForm.Close();
                        }

                        // Re-enable all buttons after reboot
                        settingsForm.EnableAllButtons();
                        EnableAllButtons();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while changing to {mode} Mode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void UpdateDisplayLabel(string rotation)
        {
            switch (rotation.Trim())
            {
                case "0":
                    displayLabel.Text = "Display: Portrait";
                    break;
                case "1":
                    displayLabel.Text = "Display:Default Landscape";
                    break;
                case "2":
                    displayLabel.Text = "Display: Reverse Portrait";
                    break;
                case "3":
                    displayLabel.Text = "Display: Landscape Right";
                    break;
                default:
                    displayLabel.Text = "Display: Unknown";
                    break;
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

            private async Task setScreenSize(string size)
        {
            try
            {
                string deviceModel = await parentForm.ExecuteAdbCommand("adb shell getprop ro.product.model");

                if (deviceModel.ToLower().Contains("p4"))
                {
                    await parentForm.ExecuteAdbCommand($"adb shell wm size {size}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while setting screen size: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            }

            private async Task guna2Button1_ClickAsync(object sender, EventArgs e)
            {

        }

        private async void btnLandscapeRight_Click(object sender, EventArgs e)
        {
            await SetDisplayMode("Landscape Right", 3, 270);


        }

        private async void label1_Click(object sender, EventArgs e)
        {

            await setScreenSize("479x480");

        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            await setScreenSize("719x720");
        }
    }
}
