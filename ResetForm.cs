using Innovo_TP4_Updater.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class ResetForm : Form
    {
        private Form1 parentForm;
        private readonly SettingsForm settingsForm;
        private readonly Dictionary<string, Button> _ResetButtons = new Dictionary<string, Button>();

        public ResetForm(Form1 parent, SettingsForm settingsForm)
        {
            InitializeComponent();
            parentForm = parent;

            // Subscribe to the Load event
            this.Load += new EventHandler(ResetForm_Load);
            this.settingsForm = settingsForm;
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
                    Text = $"Reset {appName}",
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
                    await ResetApplication(appName,packageName,  btn);
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
        private async Task ResetApplication(string appName, string packageName, Button clickedButton)
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

            if (!isConnected)
            {
                MessageBox.Show("No device is currently connected. Please connect a device before attempting to reset.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the package name from the app name
            if (string.IsNullOrEmpty(packageName))
            {
                MessageBox.Show($"No package found for the app: {appName}", "Package Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Show confirmation prompt
            var result = MessageBox.Show(
                $"Are you sure you want to reset the cache and storage for {appName}? This action cannot be undone.",
                "Confirm Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                LoadingForm loadingForm = null;

                try
                {
                    // Disable and hide buttons
                    DisableOtherButtons(clickedButton);

                    // Disable all buttons in the SettingsForm
                    settingsForm.DisableAllButtons();

                    // Show loading form for resetting cache and storage
                    loadingForm = new LoadingForm("Resetting cache and storage... Please wait.");
                    loadingForm.Show();

                    // Execute the adb command to reset the cache and storage
                    await parentForm.ExecuteAdbCommand($"adb shell pm clear {packageName}");


                    // Close the first loading form
                    loadingForm.Close();

                    // Reboot the device
                    await RebootDevice();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during reset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Ensure the loading form is closed in all cases
                    loadingForm?.Close();

                    // Re-enable all buttons in the SettingsForm
                    settingsForm.EnableAllButtons();

                    // Show and enable all buttons again
                    EnableAllButtons();
                }
            }
        }
        

        private async Task RebootDevice()
        {
            LoadingForm loadingForm = null;

            try
            {
                // Show a new loading form for rebooting
                loadingForm = new LoadingForm("Reset Successful,Rebooting device... Please wait.");
                loadingForm.Show();

                // Execute the adb command to reboot the device
                await parentForm.ExecuteAdbCommand("adb reboot");

                // Delay for 15 seconds to allow the device to turn back on
                await Task.Delay(45000);

                MessageBox.Show("The device has rebooted and should be back online now.", "Reboot Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                // Ensure the loading form is closed
                loadingForm?.Close();
            }
        }

    }
}
