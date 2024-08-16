using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Innovo_TP4_Updater
{
    public partial class FactoryDefaultForm : Form
    {
        private readonly Form1 parentForm;

        public FactoryDefaultForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;

            // Subscribe to the Load event
            this.Load += new EventHandler(ResetForm_Load);
        }

        private async void ResetForm_Load(object sender, EventArgs e)
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

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
            await ResetApplication("Nice", button.Text); // Using app name to identify the package
        }

        private async void btnApp2_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Control4", button.Text);
        }

        private async void btnApp3_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Rako", button.Text);
        }

        private async void btnApp4_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Lutron", button.Text);
        }

        private async Task ResetApplication(string appName, string displayName)
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
                    // Show loading form for factory reset
                    loadingForm = new LoadingForm("Performing factory reset... Please wait.");
                    loadingForm.Show();

                    // Set display to always active
                    await parentForm.ExecuteAdbCommand("adb shell settings put system screen_off_timeout 2147483647");

                    // Set display to adaptive
                    await parentForm.ExecuteAdbCommand("adb shell settings put system adaptive_display 1");

                    // Set resolution to 479x480
                    await parentForm.ExecuteAdbCommand("adb shell wm size 479x480");

                    // Set sound to maximum (7 for system, 15 for media)
                    await parentForm.ExecuteAdbCommand("adb shell media volume --stream 3 --set 15");
                    await parentForm.ExecuteAdbCommand("adb shell media volume --stream 1 --set 7");

                    // Clear cache and data
                    await parentForm.ExecuteAdbCommand($"adb shell pm clear {packageName}");

                    // Uninstall app if installed
                    await parentForm.ExecuteAdbCommand($"adb shell pm uninstall {packageName}");

                    // Update the app using the logic from UpdateAppForm
                    await UpdateApp(appName);

                    MessageBox.Show($"{displayName} factory reset and update completed successfully.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Close the first loading form
                    loadingForm.Close();

                    // Show a new loading form for rebooting
                    loadingForm = new LoadingForm("Rebooting the device... Please wait.");
                    loadingForm.Show();

                    // Execute the adb command to reboot the device
                    await parentForm.ExecuteAdbCommand("adb reboot");

                    MessageBox.Show("The device is rebooting.", "Rebooting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during factory reset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Ensure the loading form is closed in all cases
                    loadingForm?.Close();
                }
            }
        }

        private async Task UpdateApp(string appName)
        {
            // Step 1: Check for connected devices
            string connectedDevices = await parentForm.ExecuteAdbCommand("adb devices -l");

            if (!connectedDevices.Contains("device"))
            {
                MessageBox.Show("No connected device detected. Please connect a device and try again.", "No Connected Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Step 2: Retrieve the device model name
            string deviceModel = await GetDeviceModel();

            if (deviceModel != "P4")
            {
                MessageBox.Show($"Connected device is {deviceModel}, but only P4 devices are supported for updates.", "Unsupported Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Step 3: Set resolution for update
                await parentForm.ExecuteAdbCommand("adb shell wm size 479x480");

                // Check for updates and download
                string jsonUrl = "https://innovo.net/repo/TP4/files.json";
                string jsonString;

                using (HttpClient client = new HttpClient())
                {
                    jsonString = await client.GetStringAsync(jsonUrl);
                }

                JObject jsonData = JObject.Parse(jsonString);
                if (jsonData[appName] == null)
                {
                    MessageBox.Show($"No update data found for {appName}.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string latestVersion = jsonData[appName]["version"].ToString();
                string fileType = jsonData[appName]["type"].ToString();
                string fileName = jsonData[appName]["filename"].ToString();

                string downloadUrl = $"https://innovo.net/repo/TP4/{fileName}";
                string downloadDirectory = Path.Combine(Path.GetTempPath(), fileName);

                using (WebClient client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(new Uri(downloadUrl), downloadDirectory);
                }

                // Use ADB to install/update the app based on file type
                if (fileType == "file")
                {
                    await InstallApk(downloadDirectory);
                }
                else if (fileType == "zip")
                {
                    await UnzipAndInstall(downloadDirectory);
                }

                MessageBox.Show($"Successfully updated {appName} to version {latestVersion}.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during update: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to get the device model
        private async Task<string> GetDeviceModel()
        {
            string modelCommand = "adb shell getprop ro.product.model";
            string modelOutput = await parentForm.ExecuteAdbCommand(modelCommand);
            return modelOutput.Trim();
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

        private async Task InstallApk(string filePath)
        {
            string installCommand = $"adb install -r {filePath}";
            await parentForm.ExecuteAdbCommand(installCommand);
        }

        private async Task UnzipAndInstall(string zipFilePath)
        {
            string extractPath = Path.Combine(Path.GetDirectoryName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));

            // Delete the directory if it already exists
            if (Directory.Exists(extractPath))
            {
                Directory.Delete(extractPath, true);
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath);

            string[] extractedFiles = Directory.GetFiles(extractPath, "*.apk", SearchOption.AllDirectories);
            if (extractedFiles.Length > 0)
            {
                // Prepare the adb install-multiple command
                string installCommand = "adb install-multiple -r --user 0 ";
                foreach (string apkFile in extractedFiles)
                {
                    installCommand += $"\"{apkFile}\" ";
                }

                await parentForm.ExecuteAdbCommand(installCommand.TrimEnd());
            }
            else
            {
                MessageBox.Show("No APK files found in the extracted ZIP.", "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
