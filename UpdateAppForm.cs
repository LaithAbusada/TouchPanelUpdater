using MaterialSkin.Controls;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;

namespace Innovo_TP4_Updater
{
    public partial class UpdateAppForm : Form
    {
        private readonly Form1 parentForm;
        private readonly SettingsForm settingsForm;

        public UpdateAppForm(Form1 parent, SettingsForm sets)
        {
            InitializeComponent();
            parentForm = parent;
            settingsForm = sets;
        }

        private async void UpdateAppForm_Load(object sender, EventArgs e)
        {
            try
            {
                bool isConnected = await parentForm.IsConnected();

                if (!isConnected)
                {
                    parentForm.clearMainPanel();
                    materialMultiLineTextBox3.AppendText("No device is currently connected. Please connect a device before proceeding.\n");
                    MessageBox.Show("No device is currently connected. Please connect a device before proceeding.",
                                   "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string jsonUrl = "https://innovo.net/repo/TP4/files.json";
                string jsonString;

                using (HttpClient client = new HttpClient())
                {
                    jsonString = await client.GetStringAsync(jsonUrl);
                }

                JObject jsonData = JObject.Parse(jsonString);

                // Check versions for each app and update UI
                await CheckAndDisplayVersionStatus("Nice", jsonData, buttonUpdateNice, labelNiceStatus);
                await CheckAndDisplayVersionStatus("Rako", jsonData, buttonUpdateRako, labelRakoStatus);
                await CheckAndDisplayVersionStatus("Lutron", jsonData, buttonUpdateLutron, labelLutronStatus);
                await CheckAndDisplayVersionStatus("Control4", jsonData, buttonUpdateControl4, labelControl4Status);
            }
            catch (Exception ex)
            {
                materialMultiLineTextBox3.AppendText($"Error: Unable to connect to the device. Details: {ex.Message}\n");
            }
        }

        private async Task CheckAndDisplayVersionStatus(string appName, JObject jsonData, Button updateButton, Label statusLabel)
        {
            try
            {
                string currentVersion = await GetCurrentVersion(appName);
                string latestVersion = jsonData[appName]["version"].ToString();

                if (currentVersion == latestVersion)
                {
                    statusLabel.Text = "Up to date";
                    updateButton.Enabled = false;
                }
                else
                {
                    statusLabel.Text = "Update available";
                    updateButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error checking version";
                materialMultiLineTextBox3.AppendText($"Error checking version for {appName}: {ex.Message}\n");
            }
        }

        private async void UpdateApp(string appName, Button clickedButton)
        {
            int maxAttempts = 3; // Maximum number of attempts
            int attempt = 0; // Initialize the attempt counter
            bool success = false; // Flag to check if the installation was successful

            // Initialize the LoadingForm and display it
            settingsForm.DisableAllButtons();
            LoadingForm loadingForm = new LoadingForm("Checking for updates...");
            materialMultiLineTextBox3.AppendText("Checking for Update" + Environment.NewLine);
            loadingForm.Show();
            loadingForm.BringToFront();

            string downloadDirectory = string.Empty;

            while (attempt < maxAttempts && !success)
            {
                attempt++; // Increment the attempt counter
                try
                {
                    // Step 1: Check for connected devices
                    loadingForm.UpdateMessage("Checking for connected devices...");
                    string connectedDevices = await parentForm.ExecuteAdbCommand("adb devices -l");
                    if (!connectedDevices.Contains("device"))
                    {
                        materialMultiLineTextBox3.AppendText("No connected device detected. Please connect a device and try again.\n");
                        return;
                    }

                    // Step 2: Retrieve the device model name
                    loadingForm.UpdateMessage("Retrieving device model...");
                    string deviceModel = await GetDeviceModel();
                    if (deviceModel != "P4")
                    {
                        materialMultiLineTextBox3.AppendText($"Connected device is {deviceModel}, but only P4 devices are supported for updates.\n");
                        return;
                    }

                    // Step 3: Check for updates
                    DisableOtherButtons(clickedButton);

                    loadingForm.UpdateMessage("Checking for available updates...");
                    string jsonUrl = "https://innovo.net/repo/TP4/files.json";
                    string jsonString;

                    using (HttpClient client = new HttpClient())
                    {
                        jsonString = await client.GetStringAsync(jsonUrl);
                    }

                    JObject jsonData = JObject.Parse(jsonString);
                    string currentVersion = await GetCurrentVersion(appName);
                    string latestVersion = jsonData[appName]["version"].ToString();

                    if (currentVersion == latestVersion)
                    {
                        materialMultiLineTextBox3.AppendText($"{appName} is already up to date. Version: {latestVersion}\n");
                        return;
                    }

                    // Step 4: Prepare download directory and file
                    downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), jsonData[appName]["filename"].ToString());

                    // Check if the file already exists and delete it
                    if (File.Exists(downloadDirectory))
                    {
                        File.Delete(downloadDirectory);
                    }

                    // Check if the directory already exists and delete it
                    string extractPath = Path.Combine(Path.GetDirectoryName(downloadDirectory), Path.GetFileNameWithoutExtension(downloadDirectory));
                    if (Directory.Exists(extractPath))
                    {
                        Directory.Delete(extractPath, true);
                    }

                    // Step 5: Download the update
                    materialMultiLineTextBox3.AppendText("Installing Update for " + appName + Environment.NewLine);

                    loadingForm.UpdateMessage("Downloading the update...");
                    string downloadUrl = $"https://innovo.net/repo/TP4/{jsonData[appName]["filename"]}";

                    using (WebClient client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(new Uri(downloadUrl), downloadDirectory);
                    }

                    // Step 6: Install the app
                    loadingForm.UpdateMessage("Installing the update...");
                    string fileType = jsonData[appName]["type"].ToString();
                    if (fileType == "file")
                    {
                        await InstallApk(downloadDirectory);

                        // Delete the APK file after installation
                        if (File.Exists(downloadDirectory))
                        {
                            File.Delete(downloadDirectory);
                        }
                    }
                    else if (fileType == "zip")
                    {
                        await UnzipAndInstall(downloadDirectory);
                    }

                    // Step 7: Reboot the app and cleanup
                    loadingForm.UpdateMessage("Rebooting the device...");
                    bool versionCompare = await RebootApp(appName, latestVersion);

                    // If everything succeeds, set success to true
                    success = versionCompare;


                    if (success)
                    {// Update the label to "Up to date"
                        if (clickedButton == buttonUpdateNice) labelNiceStatus.Text = "Up to date";
                        else if (clickedButton == buttonUpdateRako) labelRakoStatus.Text = "Up to date";
                        else if (clickedButton == buttonUpdateLutron) labelLutronStatus.Text = "Up to date";
                        else if (clickedButton == buttonUpdateControl4) labelControl4Status.Text = "Up to date";
                    }
                }
                catch (Exception ex)
                {
                    materialMultiLineTextBox3.AppendText($"Attempt {attempt} failed: {ex.Message}\n");

                    if (attempt == maxAttempts)
                    {
                        // If this was the last attempt, display a message to contact support
                        MaterialMessageBox.Show($"Failed to install {appName} after {maxAttempts} attempts. Please contact support.", "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    // Clean up downloaded files and directories after installation
                    if (!string.IsNullOrEmpty(downloadDirectory) && File.Exists(downloadDirectory))
                    {
                        try
                        {
                            File.Delete(downloadDirectory);
                        }
                        catch (Exception ex)
                        {
                            materialMultiLineTextBox3.AppendText($"Error deleting file: {ex.Message}\n");
                        }
                    }

                    if (!string.IsNullOrEmpty(downloadDirectory))
                    {
                        string extractPath = Path.Combine(Path.GetDirectoryName(downloadDirectory), Path.GetFileNameWithoutExtension(downloadDirectory));
                        if (Directory.Exists(extractPath))
                        {
                            try
                            {
                                Directory.Delete(extractPath, true);
                            }
                            catch (Exception ex)
                            {
                                materialMultiLineTextBox3.AppendText($"Error deleting directory: {ex.Message}\n");
                            }
                        }
                    }

                    // Always close the LoadingForm and re-enable buttons at the end
                    loadingForm.Close();
                    EnableAllButtons(); // This should be called at the very end to re-enable all buttons
                    settingsForm.EnableAllButtons();
                }
            }
        }

        private async Task<string> GetDeviceModel()
        {
            string modelCommand = "adb shell getprop ro.product.model";
            string modelOutput = await parentForm.ExecuteAdbCommand(modelCommand);
            return modelOutput.Trim();
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
            materialMultiLineTextBox3.AppendText("Please wait Installing Update, this can take up to 2 minutes\n");

            string installCommand = $"adb install -r \"{filePath}\"";
            await parentForm.ExecuteAdbCommand(installCommand);
        }

        private async Task UnzipAndInstall(string zipFilePath)
        {
            materialMultiLineTextBox3.AppendText("Please wait Installing Update, this can take up to 2 minutes\n");

            string extractPath = Path.Combine(Path.GetDirectoryName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath);

            string[] extractedFiles = Directory.GetFiles(extractPath, "*.apk", SearchOption.AllDirectories);
            string installCommand = "adb install-multiple -r --user 0 " + string.Join(" ", extractedFiles.Select(file => $"\"{file}\""));

            await parentForm.ExecuteAdbCommand(installCommand.TrimEnd());

            // Delete the extracted files after installation
            if (Directory.Exists(extractPath))
            {
                try
                {
                    Directory.Delete(extractPath, true);

                }
                catch (Exception ex)
                {
                    materialMultiLineTextBox3.AppendText($"Error deleting directory: {ex.Message}\n");
                }
            }
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }
        }

        private async Task<bool> RebootApp(string appName, string latestVersion)
        {
            materialMultiLineTextBox3.AppendText("Please wait Rebooting Device, this can take up to 45 seconds\n");

            string rebootCommand = "adb reboot";
            await parentForm.ExecuteAdbCommand(rebootCommand);

            // Wait for the device to reboot
            await Task.Delay(45000); // Adjusted to 45 seconds as per your comment

            // After reboot, check the current version of the app
            string currentVersion = await GetCurrentVersion(appName);

            if (currentVersion == latestVersion)
            {
                materialMultiLineTextBox3.AppendText($"Successfully updated {appName} to version {latestVersion}\n");
                return true; // Success
            }
            else
            {
                materialMultiLineTextBox3.AppendText($"Failed to install {appName}. Please contact support.\n");
                return false; // Failure
            }
        }


        private void DisableOtherButtons(Button clickedButton)
        {
            clickedButton.Enabled = false;
            clickedButton.Visible = true;

            buttonUpdateNice.Visible = clickedButton == buttonUpdateNice;
            labelNiceStatus.Visible = clickedButton == buttonUpdateNice;

            buttonUpdateRako.Visible = clickedButton == buttonUpdateRako;
            labelRakoStatus.Visible = clickedButton == buttonUpdateRako;

            buttonUpdateLutron.Visible = clickedButton == buttonUpdateLutron;
            labelLutronStatus.Visible = clickedButton == buttonUpdateLutron;

            buttonUpdateControl4.Visible = clickedButton == buttonUpdateControl4;
            labelControl4Status.Visible = clickedButton == buttonUpdateControl4;
        }

        private void EnableAllButtons()
        {
            

            buttonUpdateNice.Visible = true;
            labelNiceStatus.Visible = true;

            buttonUpdateRako.Visible = true;
            labelRakoStatus.Visible = true;

            buttonUpdateLutron.Visible = true;
            labelLutronStatus.Visible = true;

            buttonUpdateControl4.Visible = true;
            labelControl4Status.Visible = true;
            if (labelNiceStatus.Text != "Up to date")
            {
                buttonUpdateNice.Enabled = true;
            }
            else
            {
                buttonUpdateNice.Enabled = false;
            }

            if (labelRakoStatus.Text != "Up to date")
            {
                buttonUpdateRako.Enabled = true;
            }
            else
            {
                buttonUpdateRako.Enabled = false;
            }

            if (labelLutronStatus.Text != "Up to date")
            {
                buttonUpdateLutron.Enabled = true;
            }
            else
            {
                buttonUpdateLutron.Enabled = false;
            }

            if (labelControl4Status.Text != "Up to date")
            {
                buttonUpdateControl4.Enabled = true;
            }
            else
            {
                buttonUpdateControl4.Enabled = false;
            }
        }


        private void buttonUpdateNice_Click(object sender, EventArgs e)
        {
            UpdateApp("Nice", buttonUpdateNice);
        }

        private void buttonUpdateRako_Click(object sender, EventArgs e)
        {
            UpdateApp("Rako", buttonUpdateRako);
        }

        private void buttonUpdateLutron_Click(object sender, EventArgs e)
        {
            UpdateApp("Lutron", buttonUpdateLutron);
        }

        private void buttonUpdateControl4_Click(object sender, EventArgs e)
        {
            UpdateApp("Control4", buttonUpdateControl4);
        }
    }
}
