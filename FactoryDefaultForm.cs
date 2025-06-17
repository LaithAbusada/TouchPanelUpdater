using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Innovo_TP4_Updater.Properties;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;

namespace Innovo_TP4_Updater
{
    public partial class FactoryDefaultForm : Form
    {
        private readonly Form1 parentForm;
        private readonly SettingsForm settingsForm;
        private readonly Dictionary<string, Button> _ResetButtons = new Dictionary<string, Button>();

        public FactoryDefaultForm(Form1 parent, SettingsForm settingsForm)
        {
            InitializeComponent();
            parentForm = parent;
            this.settingsForm = settingsForm;


        }




        private async Task<Image> DownloadImageAsync(string imageFileName)
        {
            try
            {
                // Build the full URL
                string imageUrl = $"https://innovo.net/repo/TP4/images/{imageFileName}";

                using (var client = new HttpClient())
                {
                    byte[] data = await client.GetByteArrayAsync(imageUrl);

                    using (var ms = new MemoryStream(data))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                // OPTIONAL: return a fallback if the download fails
                // e.g. a single “not found” icon embedded in your Resources
                return Resources.Nice;
            }
        }


        private void DisableOtherButtons(Button clickedButton)
        {
            if (!(clickedButton.Tag is string clickedApp)) return;

            foreach (var kv in _ResetButtons)
            {
                string appName = kv.Key;
                Button eachBtn = kv.Value;
                // Always disable all, but only keep the clicked button visible
                eachBtn.Enabled = false;
                eachBtn.Visible = (appName == clickedApp);
            }


        }

        private void EnableAllButtons()
        {
            foreach (var kv in _ResetButtons)
            {
                string appName = kv.Key;
                Button eachBtn = kv.Value;

                eachBtn.Visible = true;


                eachBtn.Enabled = true;

            }

        }

        private async Task ResetApplication(string appName,string packageName, Button clickedButton)
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

            if (!isConnected)
            {
                MessageBox.Show("No device is currently connected. Please connect a device before attempting to reset.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the package name using the app name
       
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
                    DisableOtherButtons(clickedButton);

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

                 
                    await UpdateApp(appName,packageName,loadingForm,screenSize);


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
                    EnableAllButtons();
                }
            }

        }

        private async Task<string> GetCurrentVersion(string packageName)
        {
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

        private async Task UpdateApp(string appName , string packageName, LoadingForm loadingForm,string screenSize)
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

                string jsonUrl = "https://innovo.net/repo/TP4/reset_apps.json";
                string jsonString;

                using (HttpClient client = new HttpClient())
                {
                    jsonString = await client.GetStringAsync(jsonUrl);
                }

                JObject jsonData = JObject.Parse(jsonString);
                string currentVersion = await GetCurrentVersion(packageName);
                string latestVersion = jsonData[appName]["version"].ToString();


                if (!string.IsNullOrEmpty(currentVersion))
                {
                    // Now it’s safe to parse both versions
                    Version localVersion = new Version(currentVersion);
                    Version jsonVersion = new Version(latestVersion);

                    if (localVersion >= jsonVersion)
                    {
                        return;
                    }

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
                string downloadUrl = $"https://innovo.net/repo/TP4/Apks/{jsonData[appName]["filename"]}";

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

        private async void FactoryDefaultForm_Load(object sender, EventArgs e)
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

            string jsonUrl = "https://innovo.net/repo/TP4/reset_apps.json";
            string jsonString;

            using (HttpClient client = new HttpClient())
            {
                jsonString = await client.GetStringAsync(jsonUrl);
            }

            JObject jsonData = JObject.Parse(jsonString);
            // Clear any existing controls
            appsPanel.Controls.Clear();
            _ResetButtons.Clear();
            // Loop through each app in the JSON
            foreach (var prop in jsonData.Properties())
            {
                string appName = prop.Name;
                var appInfo = (JObject)prop.Value;
                string latestVersion = appInfo["version"].ToString();
                string imageFileName = appInfo["image"].ToString(); // e.g. "Nice.jpeg"
                string packageName = appInfo["packageName"].ToString();
                int appCount = jsonData.Properties().Count();
                bool useLargeTiles = appCount <= 4;
                int buttonWidth = useLargeTiles ? 267 : 200;
                int buttonHeight = useLargeTiles ? 142 : 100;

                var statusLabel = new Label
                {
                    AutoSize = false,                         // ⇐ turn off autosizing
                    Size = new Size(buttonWidth, 18),      // fix it to buttonWidth × 18px tall (adjust height as needed)
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = SystemColors.Highlight,
                    Margin = new Padding(2, 2, 2, 2),
                    TextAlign = ContentAlignment.MiddleCenter,  // center the text horizontally (optional)
                    Text = string.Empty
                };

                Image appIcon = await DownloadImageAsync(imageFileName);


                var btn = new Button
                {
                    // keep the core settings
                    Text = $"Factory Reset {appName}",
                    Enabled = true,
                    Tag = appName,
                    Margin = new Padding(2),
                    // fixed size matching your designer
                    Size = new Size(buttonWidth, buttonHeight),   // ↓ shrink from 267×142 → 200×100

                    // typography & colors
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = SystemColors.Highlight,

                    // image background
                    BackgroundImage = appIcon,
                    BackgroundImageLayout = ImageLayout.Stretch,

                    // text positioning
                    TextAlign = ContentAlignment.BottomCenter,
                    TextImageRelation = TextImageRelation.ImageAboveText,

                    // if you prefer the MaterialSkin look, swap Button → MaterialRaisedButton & drop BackColor/ForeColor
                };

                // Wire up click to your existing UpdateApp method
                btn.Click += async (s, ev) =>
                {
                    await ResetApplication(appName, packageName, btn);
                };

                appsPanel.Padding = new Padding(0);


                // Add both to a panel (or directly to FlowLayoutPanel)
                var appContainer = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    WrapContents = false,
                    Margin = new Padding(5)
                };
                appContainer.Controls.Add(btn);
                appContainer.Controls.Add(statusLabel);

                appsPanel.Controls.Add(appContainer);

                _ResetButtons[appName] = btn;
            }
        }
    }
}
