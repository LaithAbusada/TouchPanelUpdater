using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;

namespace Innovo_TP4_Updater
{
    public partial class FactoryDefaultForm : Form
    {
        private readonly Form1 parentForm;
        private readonly SettingsForm settingsForm;

        public FactoryDefaultForm(Form1 parent, SettingsForm settingsForm)
        {
            InitializeComponent();
            parentForm = parent;
            this.settingsForm = settingsForm;

            // Subscribe to the Load event
            this.Load += new EventHandler(ResetForm_Load);
        }

        private async void ResetForm_Load(object sender, EventArgs e)
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();
            await settingsForm.TriggerConnectionStatusUpdate(isConnected);

            if (!isConnected)
            {
                MessageBox.Show("No device is currently connected. Please connect a device before using this form.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                parentForm.clearMainPanel();
                return;
            }
        }

        private async void btnApp1_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Nice", button);
        }

        private async void btnApp2_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Control4", button);
        }

        private async void btnApp3_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Rako", button);
        }

        private async void btnApp4_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Lutron", button);
        }

        private async Task ResetApplication(string appName, Guna.UI2.WinForms.Guna2Button clickedButton)
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

            if (!isConnected)
            {
                MessageBox.Show("No device is currently connected. Please connect a device before attempting to reset.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the package name using the app name
            string packageName = GetPackageName(appName);
            if (string.IsNullOrEmpty(packageName))
            {
                MessageBox.Show($"No package found for the app: {appName}", "Package Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Show confirmation prompt
            var result = MessageBox.Show(
                $"Are you sure you want to reset the device to factory defaults for {appName}? This action cannot be undone.",
                "Confirm Factory Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                LoadingForm loadingForm = null;

                try
                {
                    // Disable and hide other buttons
                    DisableAndHideButtons(clickedButton);

                    // Disable all buttons in the SettingsForm
                    settingsForm.DisableAllButtons();

                    // Show loading form for factory reset
                    loadingForm = new LoadingForm("Performing factory reset... Please wait.");
                    loadingForm.Show();

                    string deviceModel = await parentForm.ExecuteAdbCommand("adb shell getprop ro.product.model");

                    string androidversion = await parentForm.ExecuteAdbCommand("adb shell getprop ro.build.version.release");

                    Version androidVersionOutput = new Version(NormalizeVersion(androidversion));

                    // Set display to always active
                    await parentForm.ExecuteAdbCommand("adb shell settings put system screen_off_timeout 2147483647");


                    await parentForm.ExecuteAdbCommand("adb shell settings put secure sleep_timeout -1");

                    // Disable screensaver (Daydream)
                    await parentForm.ExecuteAdbCommand("adb shell settings put secure screensaver_enabled 0");

                    // Set display to adaptive (enable adaptive brightness)
                    await parentForm.ExecuteAdbCommand("adb shell settings put system screen_brightness_mode 1");

                    if (deviceModel.ToLower().Contains("p5"))
                    {
                        await parentForm.ExecuteAdbCommand("adb shell settings put system screen_backlight 2147483647");
                    }


                    Version version13 = new Version(NormalizeVersion("13"));

                    if (androidVersionOutput >= version13)
                    {
                        // Disable auto-rotate and set user rotation to the selected mode
                        await parentForm.ExecuteAdbCommand("adb shell settings put system accelerometer_rotation 0");

                        // Set HDMI orientation and user rotation based on mode
                        await parentForm.ExecuteAdbCommand($"adb shell settings put system hdmi_orientation 0");
                        await parentForm.ExecuteAdbCommand($"adb shell settings put system user_rotation 0");
                    }

                    // Set sound to maximum (7 for system, 15 for media)
                    await parentForm.ExecuteAdbCommand("adb shell media volume --stream 3 --set 15");
                    await parentForm.ExecuteAdbCommand("adb shell media volume --stream 1 --set 7");

                    // Clear cache and data
                    await parentForm.ExecuteAdbCommand($"adb shell pm clear {packageName}");

                    // Uninstall app if installed
                    await parentForm.ExecuteAdbCommand($"adb shell pm uninstall {packageName}");

                    // Update the app using the logic from UpdateAppForm

                    string screenSize = await GetScreenSize();

                 
                    await UpdateApp(appName,loadingForm,screenSize);


                    // Close the first loading form
                    loadingForm.Close();

                    // Reboot the device
                    await RebootDevice();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during factory reset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Ensure the loading form is closed in all cases
                    loadingForm?.Close();

                    // Re-enable all buttons in the SettingsForm and FactoryDefaultForm
                    settingsForm.EnableAllButtons();
                    ShowAndEnableButtons();
                }
            }

        }

        private async Task<string> GetCurrentVersion(string appName)
        {
            string packageName = GetPackageName(appName);
            if (string.IsNullOrEmpty(packageName))
            {
                return null;
            }

            string command = $"adb shell dumpsys package {packageName} | findstr versionName";
            string output = await parentForm.ExecuteAdbCommand(command);

            if (!string.IsNullOrEmpty(output))
            {
                string versionLine = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
                string versionName = versionLine.Split('=')[1].Trim();
                return versionName;
            }

            return null;
        }

        private async Task RebootDevice()
        {
            LoadingForm loadingForm = null;

            try
            {
                // Show a new loading form for rebooting
                loadingForm = new LoadingForm("Factory Reset Successful,Rebooting device... Please wait.");
                loadingForm.Show();

                // Execute the adb command to reboot the device
                await parentForm.ExecuteAdbCommand("adb reboot");

                // Delay for 45 seconds to allow the device to turn back on
                await Task.Delay(45000);

                MessageBox.Show("The device has rebooted and should be back online now.", "Reboot Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                // Ensure the loading form is closed
                loadingForm?.Close();
            }
        }

        private string GetPackageName(string appName)
        {
            switch (appName)
            {
                case "Nice":
                    return "com.homelogic";
                case "Lutron":
                    return "com.lutron.mmw";
                case "Control4":
                    return "com.control4.phoenix";
                case "Rako":
                    return "com.rakocontrols.android";
                default:
                    return null;
            }
        }

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

        private async Task UpdateApp(string appName , LoadingForm loadingForm,string screenSize)
        {
            string downloadDirectory = string.Empty;
            try
            {
                // Check for connected devices
                string connectedDevices = await parentForm.ExecuteAdbCommand("adb devices -l");

                if (!connectedDevices.Contains("device"))
                {
                    MessageBox.Show("No connected device detected. Please connect a device and try again.", "No Connected Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // Retrieve the device model
                string deviceModel = await GetDeviceModel();
                string lowerCaseModel = deviceModel.ToLower();

                if (!lowerCaseModel.Contains("p4") && !lowerCaseModel.Contains("p5"))
                {
                    MessageBox.Show($"Connected device is {deviceModel}, but only P4 or P5 devices are supported for updates.\n");
                    return;
                }



                string jsonUrl = "https://innovo.net/repo/TP4/files.json";
                string jsonString;

                using (HttpClient client = new HttpClient())
                {
                    jsonString = await client.GetStringAsync(jsonUrl);
                }

                JObject jsonData = JObject.Parse(jsonString);
                string currentVersion = await GetCurrentVersion(appName);
                string latestVersion = jsonData[appName]["version"].ToString();

                // Parse versions to enable comparison
                Version localVersion = new Version(currentVersion);
                Version jsonVersion = new Version(latestVersion);

                if (localVersion >= jsonVersion)
                {
                    return;
                }

                // Step 3: Set resolution for update
                if (appName == "Control4" && lowerCaseModel.Contains("p4"))
                {
                    await parentForm.ExecuteAdbCommand("adb shell wm size 720x720");
                }
                else if (lowerCaseModel.Contains("p5")) { }
                else if (screenSize == "480x480")
                {
                    await parentForm.ExecuteAdbCommand("adb shell wm size 479x480");
                }

                string baseDirectory = Path.Combine(Application.StartupPath, "Downloads");  // Using relative path
                downloadDirectory = Path.Combine(baseDirectory, jsonData[appName]["filename"].ToString());

                string downloadFolder = Path.GetDirectoryName(downloadDirectory);
                if (!Directory.Exists(downloadFolder))
                {
                    Directory.CreateDirectory(downloadFolder);
                }

                if (File.Exists(downloadDirectory))
                {
                    File.Delete(downloadDirectory);
                }

                string extractPath = Path.Combine(Path.GetDirectoryName(downloadDirectory), Path.GetFileNameWithoutExtension(downloadDirectory));
                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }

                loadingForm.UpdateMessage("Installing Update for " + appName + " this can take up to 2 minutes" + Environment.NewLine);

                loadingForm.UpdateMessage("Downloading the update...");
                string downloadUrl = $"https://innovo.net/repo/TP4/{jsonData[appName]["filename"]}";

                using (WebClient client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(new Uri(downloadUrl), downloadDirectory);
                }

                // Check if the downloaded file exists before proceeding
                if (!File.Exists(downloadDirectory))
                {
                    return;
                }

                loadingForm.UpdateMessage("Installing the update...");
                string fileType = jsonData[appName]["type"].ToString();
                if (fileType == "file")
                {
                    await InstallApk(downloadDirectory);

                    if (File.Exists(downloadDirectory))
                    {
                        File.Delete(downloadDirectory);
                    }
                }
                else if (fileType == "zip")
                {
                    await UnzipAndInstall(downloadDirectory);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during update: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                // Clean up downloaded files
                if (!string.IsNullOrEmpty(downloadDirectory) && File.Exists(downloadDirectory))
                {
                    try
                    {
                        File.Delete(downloadDirectory);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting downloaded file: {ex.Message}", "Cleanup Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private async Task<string> GetScreenSize()
        {
            string screenSizeCommand = "adb shell wm size";
            string screenSizeOutput = await parentForm.ExecuteAdbCommand(screenSizeCommand);
            return screenSizeOutput.Split(':')[1].Trim();
        }

        // Helper method to get the device model
        private async Task<string> GetDeviceModel()
        {
            string modelCommand = "adb shell getprop ro.product.model";
            string modelOutput = await parentForm.ExecuteAdbCommand(modelCommand);
            return modelOutput.Trim();
        }
        private async Task InstallApk(string filePath)
        {

            string installCommand = $"adb install -r \"{filePath}\"";
            await parentForm.ExecuteAdbCommand(installCommand);
        }


        private async Task UnzipAndInstall(string zipFilePath)
        {
            string extractPath = Path.Combine(Path.GetDirectoryName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            string tempDirectory = Path.Combine(Application.StartupPath, "APKFiles");  // Using relative path

            try
            {
                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }

                System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath);

                string[] apkFiles = Directory.GetFiles(extractPath, "*.apk", SearchOption.AllDirectories);

                if (apkFiles.Length > 0)
                {
                    // Ensure temp directory exists
                    if (!Directory.Exists(tempDirectory))
                    {
                        Directory.CreateDirectory(tempDirectory);
                    }

                    // Properly quote each file path
                    var quotedFilePaths = apkFiles.Select(f => $"\"{f}\"");
                    string installCommand = "adb install-multiple -r -d --user 0 " + string.Join(" ", quotedFilePaths);

                    string result = await parentForm.ExecuteAdbCommand(installCommand);
                    

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        // Show the result in a message box

                        // Check if the installation was successful

                    }
                    else
                    {
                       MessageBox.Show("No result from the adb command.\n");
                    }
                }
                else
                {
                   MessageBox.Show("No APK files found after unzipping.\n");
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show($"Error during unzip and install: {ex.Message}\n");
            }
            finally
            {
                CleanUp(extractPath);
                CleanUp(tempDirectory);
            }
        }

        private void CleanUp(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting file: {ex.Message}\n");
                }
            }
        }

            private void DisableAndHideButtons(Guna.UI2.WinForms.Guna2Button clickedButton)
        {
            // Disable and keep the clicked button visible
            clickedButton.Enabled = false;

            // Hide the other buttons
            if (clickedButton != btnApp1)
            {
                btnApp1.Visible = false;
            }
            if (clickedButton != btnApp2)
            {
                btnApp2.Visible = false;
            }
            if (clickedButton != btnApp3)
            {
                btnApp3.Visible = false;
            }
            if (clickedButton != btnApp4)
            {
                btnApp4.Visible = false;
            }
        }

        private void ShowAndEnableButtons()
        {
            // Show and enable all buttons
            btnApp1.Visible = true;
            btnApp2.Visible = true;
            btnApp3.Visible = true;
            btnApp4.Visible = true;

            btnApp1.Enabled = true;
            btnApp2.Enabled = true;
            btnApp3.Enabled = true;
            btnApp4.Enabled = true;
        }
    }
}
