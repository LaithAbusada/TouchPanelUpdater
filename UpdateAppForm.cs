using MaterialSkin.Controls;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Linq;
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
                    MessageBox.Show("No device is currently connected. Please connect a device before proceeding.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string connectedDevices = await parentForm.ExecuteAdbCommand("adb devices -l");

                if (connectedDevices.Contains("device"))
                {
                    string[] lines = connectedDevices.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                    string ipAddressAndPort = null;

                    foreach (string line in lines)
                    {
                        if (line.Contains("device") && line.Contains(":"))
                        {
                            ipAddressAndPort = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(ipAddressAndPort))
                    {
                        label5.Text = $"Connected: {ipAddressAndPort}";
                    }
                    else
                    {
                        label5.Text = "No Connected Device";
                    }
                }
                else
                {
                    label5.Text = "No Connected Device";
                }
            }
            catch (Exception ex)
            {
                label5.Text = $"Error: {ex.Message}";
            }
        }


        private async void UpdateApp(string appName, Button clickedButton)
        {
            // Step 1: Check for connected devices
            string connectedDevices = await parentForm.ExecuteAdbCommand("adb devices -l");

            if (!connectedDevices.Contains("device"))
            {
                materialMultiLineTextBox3.Text = "No connected device detected. Please connect a device and try again.\n";
                label5.Text = "no connected devices";
                return;
            }

            // Step 2: Retrieve the device model name
            string deviceModel = await GetDeviceModel();

            if (deviceModel != "P4")
            {
                materialMultiLineTextBox3.Text = $"Connected device is {deviceModel}, but only P4 devices are supported for updates.\n";
                return;
            }

            // Step 3: Show LoadingForm and disable buttons
            LoadingForm loadingForm = new LoadingForm("Please wait attempting to install update");
            loadingForm.Show();
            loadingForm.BringToFront();

            // Disable all buttons in the SettingsForm
            settingsForm.DisableAllButtons();

            string downloadDirectory = string.Empty;

            try
            {
                await parentForm.ExecuteAdbCommand("adb shell wm size 479x480");
                DisableOtherButtons(clickedButton);
                materialMultiLineTextBox3.Text = $"Please wait... Checking for updates for {appName}.\n";

                string jsonUrl = "https://innovo.net/repo/TP4/files.json";
                string jsonString = string.Empty;

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        jsonString = await client.GetStringAsync(jsonUrl);
                    }
                }
                catch (Exception ex)
                {
                    materialMultiLineTextBox3.AppendText($"Error fetching JSON: {ex.Message}\n");
                    EnableAllButtons();
                    settingsForm.EnableAllButtons();
                    return;
                }

                JObject jsonData = JObject.Parse(jsonString);
                string currentVersion = await GetCurrentVersion(appName);
                string latestVersion = jsonData[appName]["version"].ToString();
                string fileType = jsonData[appName]["type"].ToString();
                string fileName = jsonData[appName]["filename"].ToString();

                if (currentVersion == latestVersion)
                {
                    materialMultiLineTextBox3.AppendText($"{appName} is already up to date.\n");
                    EnableAllButtons();
                   settingsForm.EnableAllButtons();
                    return;
                }

                string downloadUrl = $"https://innovo.net/repo/TP4/{fileName}";
                string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                downloadDirectory = Path.Combine(solutionDirectory, fileName);

                try
                {
                    WebClient clients = new WebClient();
                    await clients.DownloadFileTaskAsync(new Uri(downloadUrl), downloadDirectory);
                    materialMultiLineTextBox3.AppendText($"Downloaded {fileName} successfully.\n");
                }
                catch (Exception ex)
                {
                    materialMultiLineTextBox3.AppendText($"Error downloading file: {ex.Message}\n");
                    EnableAllButtons();
                   settingsForm.EnableAllButtons();
                    return;
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

                materialMultiLineTextBox3.AppendText($"Successfully updated {appName} to version {latestVersion}.\n");
               await RebootApp();
            }
            finally
            {
                // Ensure that the loading form is closed after the update process completes
                loadingForm.Close();

                // Clean up downloaded files
                if (!string.IsNullOrEmpty(downloadDirectory) && File.Exists(downloadDirectory))
                {
                    try
                    {
                        File.Delete(downloadDirectory);
                        materialMultiLineTextBox3.AppendText($"Deleted downloaded file: {downloadDirectory}\n");
                    }
                    catch (Exception ex)
                    {
                        materialMultiLineTextBox3.AppendText($"Error deleting file: {ex.Message}\n");
                    }
                }

                // Enable all buttons again in both forms
                EnableAllButtons();
                settingsForm.EnableAllButtons();

                // Reboot the device
            }
        }

        // Helper method to get the device model
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
                // Extract the version name from the output
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
            try
            {
                materialMultiLineTextBox3.AppendText($"Installing {Path.GetFileName(filePath)}...\n");

                // Ensure the file path is correctly formatted
                string installCommand = $"adb install -r \"{filePath}\"";
                materialMultiLineTextBox3.AppendText($"Executing: {installCommand}\n");

                string result = await parentForm.ExecuteAdbCommand(installCommand);

                if (!string.IsNullOrEmpty(result))
                {
                    materialMultiLineTextBox3.AppendText(result);
                }

                materialMultiLineTextBox3.AppendText($"Installed {Path.GetFileName(filePath)}.\n");

                // Attempt to delete the file after installation
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                        materialMultiLineTextBox3.AppendText($"Deleted file: {filePath}\n");
                    }
                    catch (Exception ex)
                    {
                        materialMultiLineTextBox3.AppendText($"Error deleting file: {ex.Message}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                materialMultiLineTextBox3.AppendText($"Error installing APK: {ex.Message}\n");
            }
        }



        private async Task UnzipAndInstall(string zipFilePath)
        {
            materialMultiLineTextBox3.AppendText($"Extracting {Path.GetFileName(zipFilePath)}...\n");
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
                materialMultiLineTextBox3.AppendText("Installing multiple APKs...\n");

                // Prepare the adb install-multiple command
                string installCommand = "adb install-multiple -r --user 0 ";
                foreach (string apkFile in extractedFiles)
                {
                    installCommand += $"\"{apkFile}\" ";
                }

                await parentForm.ExecuteAdbCommand(installCommand.TrimEnd());
                materialMultiLineTextBox3.AppendText("Installed all APKs.\n");
            }
            else
            {
                materialMultiLineTextBox3.AppendText("No APK files found in the extracted ZIP.\n");
            }

            // After installation, delete the ZIP file
            try
            {
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                    materialMultiLineTextBox3.AppendText($"Deleted ZIP file: {zipFilePath}\n");
                }
                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                    materialMultiLineTextBox3.AppendText($"Deleted Directory : {extractPath}");
                }
            }
            catch (Exception ex)
            {
                materialMultiLineTextBox3.AppendText($"Error deleting ZIP file: {ex.Message}\n");
            }
        }

        private async Task RebootApp()
        {
            materialMultiLineTextBox3.AppendText("Rebooting the device...\n");
            string rebootCommand = "adb reboot";

            try
            {
                // Execute the reboot command
                await parentForm.ExecuteAdbCommand(rebootCommand);
                materialMultiLineTextBox3.AppendText("Device rebooted.\n");

                // Delay for 15 seconds to allow the device to turn back on
                materialMultiLineTextBox3.AppendText("Waiting for the device to turn back on...\n");
                await Task.Delay(15000); // 15000 milliseconds = 15 seconds

                materialMultiLineTextBox3.AppendText("Device back online now.\n");

                // Re-enable all buttons and make update buttons visible only after the delay
                EnableAllButtons();
                settingsForm.EnableAllButtons();
            }
            catch (Exception ex)
            {
                materialMultiLineTextBox3.AppendText($"Error during reboot: {ex.Message}\n");
            }
        }


        private void DisableOtherButtons(Button clickedButton)
        {
            // Disable the clicked button and keep it visible
            clickedButton.Enabled = false;
            clickedButton.Visible = true;

            // Hide the other buttons
            if (clickedButton != buttonUpdateNice)
            {
                buttonUpdateNice.Visible = false;
            }
            if (clickedButton != buttonUpdateRako)
            {
                buttonUpdateRako.Visible = false;
            }
            if (clickedButton != buttonUpdateLutron)
            {
                buttonUpdateLutron.Visible = false;
            }
            if (clickedButton != buttonUpdateControl4)
            {
                buttonUpdateControl4.Visible = false;
            }
        }

        private void EnableAllButtons()
        {
            // Re-enable and show all buttons
            buttonUpdateNice.Enabled = true;
            buttonUpdateRako.Enabled = true;
            buttonUpdateLutron.Enabled = true;
            buttonUpdateControl4.Enabled = true;

            buttonUpdateNice.Visible = true;
            buttonUpdateRako.Visible = true;
            buttonUpdateLutron.Visible = true;
            buttonUpdateControl4.Visible = true;
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
