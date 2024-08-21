using MaterialSkin.Controls;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Diagnostics;

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
                    statusLabel.Text = $"Up to date (v{currentVersion})";
                    updateButton.Enabled = false;
                }
                else
                {
                    statusLabel.Text = $"Update available: v{currentVersion} → v{latestVersion}";
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
            int maxAttempts = 3;
            int attempt = 0;
            bool success = false;
            materialMultiLineTextBox3.Clear();
            settingsForm.DisableAllButtons();
            LoadingForm loadingForm = new LoadingForm("Checking for updates...");
            materialMultiLineTextBox3.AppendText("Checking for Update" + Environment.NewLine);
            loadingForm.Show();
            loadingForm.BringToFront();

            string downloadDirectory = string.Empty;

            while (attempt < maxAttempts && !success)
            {

                settingsForm.DisableAllButtons();
                attempt++;
                try
                {
                    string setSizeCommand = "adb shell wm size 479x480";
                    await parentForm.ExecuteAdbCommand(setSizeCommand);
                    materialMultiLineTextBox3.AppendText("Screen size set to 479x480\n");
                    loadingForm.UpdateMessage("Checking for connected devices...");
                    string connectedDevices = await parentForm.ExecuteAdbCommand("adb devices -l");
                    if (!connectedDevices.Contains("device"))
                    {
                        materialMultiLineTextBox3.AppendText("No connected device detected. Please connect a device and try again.\n");
                        return;
                    }

                    loadingForm.UpdateMessage("Retrieving device model...");
                    string deviceModel = await GetDeviceModel();
                    if (deviceModel != "P4")
                    {
                        materialMultiLineTextBox3.AppendText($"Connected device is {deviceModel}, but only P4 devices are supported for updates.\n");
                        return;
                    }

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

                    materialMultiLineTextBox3.AppendText("Installing Update for " + appName + " this can take up to 2 minutes" + Environment.NewLine);

                    loadingForm.UpdateMessage("Downloading the update...");
                    string downloadUrl = $"https://innovo.net/repo/TP4/{jsonData[appName]["filename"]}";

                    using (WebClient client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(new Uri(downloadUrl), downloadDirectory);
                    }

                    // Check if the downloaded file exists before proceeding
                    if (!File.Exists(downloadDirectory))
                    {
                        materialMultiLineTextBox3.AppendText($"Failed to download the update file for {appName}. Please check the internet connection and try again.\n");
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

                    loadingForm.UpdateMessage("Rebooting the device...");
                    bool versionCompare = await RebootApp(appName, latestVersion);

                    success = versionCompare;

                    if (success)
                    {
                        UpdateStatusLabel(appName, clickedButton);
                        materialMultiLineTextBox3.AppendText($"Successfully Updated {appName} to {latestVersion}");
                    }
                }
                catch (Exception ex)
                {
                    materialMultiLineTextBox3.AppendText($"Attempt {attempt} failed: {ex.Message}\n");

                    if (attempt == maxAttempts)
                    {
                        MessageBox.Show($"Failed to install {appName} after {maxAttempts} attempts. Please contact support.", "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    CleanUp(downloadDirectory);

                    loadingForm.Close();
                    EnableAllButtons();
                    settingsForm.EnableAllButtons();
                }
            }
            if (!success)
            {
                MessageBox.Show($"Failed to install {appName} after {maxAttempts} attempts. Please contact support.", "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        materialMultiLineTextBox3.AppendText($"Install command result: {result}\n");

                        // Check if the installation was successful
              
                    }
                    else
                    {
                        materialMultiLineTextBox3.AppendText("No result from the adb command.\n");
                    }
                }
                else
                {
                    materialMultiLineTextBox3.AppendText("No APK files found after unzipping.\n");
                }
            }
            catch (Exception ex)
            {
                materialMultiLineTextBox3.AppendText($"Error during unzip and install: {ex.Message}\n");
            }
            finally
            {
                CleanUp(extractPath);
                CleanUp(tempDirectory);
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



        private async Task<bool> RebootApp(string appName, string latestVersion)
        {
            string currentVersion = await GetCurrentVersion(appName);

            if (currentVersion == latestVersion)
            {
                materialMultiLineTextBox3.AppendText($"Successfully updated {appName} to version {latestVersion}\n");
                materialMultiLineTextBox3.AppendText("Please wait Rebooting Device, this can take up to 45 seconds\n");

                string rebootCommand = "adb reboot";
                await parentForm.ExecuteAdbCommand(rebootCommand);

                await Task.Delay(45000);
                return true;
            }
            else
            {
                materialMultiLineTextBox3.AppendText($"Failed to install {appName}. Retrying..\n");
                return false;
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
                    materialMultiLineTextBox3.AppendText($"Error deleting file: {ex.Message}\n");
                }
            }

            if (!string.IsNullOrEmpty(path))
            {
                string extractPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
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

            buttonUpdateNice.Enabled = labelNiceStatus.Text != "Up to date";
            buttonUpdateRako.Enabled = labelRakoStatus.Text != "Up to date";
            buttonUpdateLutron.Enabled = labelLutronStatus.Text != "Up to date";
            buttonUpdateControl4.Enabled = labelControl4Status.Text != "Up to date";
        }

        private void UpdateStatusLabel(string appName, Button clickedButton)
        {
            string currentVersion = labelNiceStatus.Text.Contains("Update available") ? labelNiceStatus.Text.Split('→')[1].Trim() : string.Empty;

            if (clickedButton == buttonUpdateNice) labelNiceStatus.Text = $"Up to date (v{currentVersion})";
            else if (clickedButton == buttonUpdateRako) labelRakoStatus.Text = $"Up to date (v{currentVersion})";
            else if (clickedButton == buttonUpdateLutron) labelLutronStatus.Text = $"Up to date (v{currentVersion})";
            else if (clickedButton == buttonUpdateControl4) labelControl4Status.Text = $"Up to date (v{currentVersion})";
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

        private async Task<string> updateExecuteCommand(string command)
        {
            string commandOutput = "";

            try
            {
                ProcessStartInfo pStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/K cd /d \"C:/Users/Dev\" && {command}",
                    UseShellExecute = true,  // Use the shell to execute the command
                    Verb = "runas",          // Prompt to run as administrator
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal,
                };

                pStartInfo.EnvironmentVariables["PATH"] = @"C:\Users\Dev;" + Environment.GetEnvironmentVariable("PATH");



                using (Process cmdProcess = new Process { StartInfo = pStartInfo })
                {
                    cmdProcess.Start();
                    cmdProcess.WaitForExit();
                    // We can't capture output directly because UseShellExecute = true.
                    commandOutput = "Command executed with elevated privileges. Please check the result manually.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to run command as administrator: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return commandOutput;
        }

    }
}
