using MaterialSkin.Controls;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class UpdateAppForm : Form
    {
        private readonly Form1 parentForm;

        public UpdateAppForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        public void AppendTextToMaterialMultiLineTextBox3(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    materialMultiLineTextBox3.AppendText(text);
                }));
            }
            else
            {
                materialMultiLineTextBox3.AppendText(text);
            }
        }

        private async void buttonUpdate_Click(object sender, EventArgs e)
        {
            materialMultiLineTextBox3.Text = "Please wait... Attempting to connect to the device.\n";
            string websiteUrl = "https://innovo.net/repo/TP4/nice-latest.apk";
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string downloadDirectory = Path.Combine(solutionDirectory);

            try
            {
                WebClient clients = new WebClient();
                clients.DownloadFile(websiteUrl, Path.Combine(downloadDirectory, "nice-latest.apk"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
                materialMultiLineTextBox3.AppendText($"Error downloading file: {ex.Message}\n");
                return;
            }

            WebClient client = new WebClient();
            string latest_version = client.DownloadString("https://innovo.net/repo/TP4/version");

            string ipAddress = textBoxIP.Text;
            ipAddress = ipAddress.Replace(" ", "");
            int port = int.Parse(textBoxPort.Text);

            string connect = "adb connect " + ipAddress + ":" + port;

            try
            {
                string connectResult = await parentForm.ExecuteAdbCommand(connect);
                materialMultiLineTextBox3.AppendText(connectResult);

                string[] lines = connectResult.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string searchWord = "connected"; // Update this with the confirmation message
                bool wordFound = false;
                string searchWord2 = "cannot";
                bool wordFound2 = false;

                foreach (string line in lines)
                {
                    if (line.Contains(searchWord))
                    {
                        wordFound = true;
                    }
                    if (line.Contains(searchWord2))
                    {
                        wordFound2 = true;
                    }
                }
                string ipPort = ipAddress + ':' + port;
                if (wordFound)
                {
                    if (label5.Text == "No Connected Device" || label5.Text == String.Empty)
                    {
                        label5.Text = ipPort;
                    }
                }
                else if (wordFound2)
                {
                    materialMultiLineTextBox3.AppendText("Device unable to connect, please check IP and port" + Environment.NewLine);
                    return;
                }
                else
                {
                    materialMultiLineTextBox3.AppendText("Device unable to connect, please check IP and port" + Environment.NewLine);
                    return;
                }

                materialMultiLineTextBox3.AppendText("Connected successfully" + Environment.NewLine);
                materialMultiLineTextBox3.AppendText(Environment.NewLine);
                await Task.Delay(4000);

                string version = "adb shell dumpsys package com.homelogic | findstr versionName";
                string versionResult = await parentForm.ExecuteAdbCommand(version);
                materialMultiLineTextBox3.AppendText(versionResult);

                materialMultiLineTextBox3.AppendText("Checking if update is available, Please wait " + Environment.NewLine);
                await Task.Delay(8000);
                string deviceVersion = null;
                string[] lines2 = versionResult.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string line in lines2)
                {
                    if (line.Contains("versionName="))
                    {
                        deviceVersion = line.Split('=')[1].Trim(); // Extract the version name
                        break;
                    }
                }

                latest_version = latest_version.Trim();
                if (deviceVersion != null && deviceVersion.Equals(latest_version.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    parentForm.Disconnect();
                    MessageBox.Show("Panel already up to date!", "Up to Date", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    materialMultiLineTextBox3.AppendText("Update Available" + Environment.NewLine);
                }

                materialMultiLineTextBox3.AppendText("Checking device model and screen size" + Environment.NewLine);
                string modelCommand = "adb shell getprop ro.product.model";
                string versionCommand = "adb shell getprop ro.build.version.release";

                string modelResult = await parentForm.ExecuteAdbCommand(modelCommand);
                materialMultiLineTextBox3.AppendText(modelResult);
                string deviceModel = null;

                var lines3 = modelResult.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                bool captureNextLine = false;
                foreach (string line in lines3)
                {
                    if (captureNextLine)
                    {
                        deviceModel = line.Trim();
                        break;
                    }
                    if (line.Contains("ro.product.model"))
                    {
                        captureNextLine = true;
                    }
                }

                string versionResult2 = await parentForm.ExecuteAdbCommand(versionCommand);
                materialMultiLineTextBox3.AppendText(versionResult2);

                materialMultiLineTextBox3.AppendText("Model: " + deviceModel + ", Version: " + deviceVersion + Environment.NewLine);

                if (deviceModel == "P4" && string.Compare(deviceVersion, "8.9.02") > 0)
                {
                    string changeSizeCommand = "adb shell wm size 479x480";
                    await parentForm.ExecuteAdbCommand(changeSizeCommand);
                    MessageBox.Show("Changed screen size successfully");
                }

                materialMultiLineTextBox3.AppendText("Starting Installation Process, This can take up to 20 seconds " + Environment.NewLine);
                string installCommand = $@"for %f in ({downloadDirectory}\*.apk) do adb install -g -r -d ""%f""";
                MessageBox.Show("Installation can take up to 20 seconds, please wait", "Installation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string installResult = await parentForm.ExecuteAdbCommand(installCommand);
                materialMultiLineTextBox3.AppendText(installResult);

                materialMultiLineTextBox3.AppendText("Installation was successful, please wait for device reboot" + Environment.NewLine);

                await Task.Delay(2000);

                materialMultiLineTextBox3.AppendText("Rebooting Device ,This can take up to 20 seconds, please Wait" + Environment.NewLine);
                string rebootCommand = "adb reboot";

                string rebootResult = await parentForm.ExecuteAdbCommand(rebootCommand);
                materialMultiLineTextBox3.AppendText(rebootResult);

                materialMultiLineTextBox3.AppendText("Rebooting Device in 3 ... 2 ... 1 ... " + Environment.NewLine);
                materialMultiLineTextBox3.AppendText("Please wait at until reboot is complete" + Environment.NewLine);
                MessageBox.Show("Device reboot can take up to 20 seconds, please wait", "Reboot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await Task.Delay(20000);

                label5.Text = string.Empty;
                materialMultiLineTextBox3.AppendText("Reboot completed successfully" + Environment.NewLine);

                parentForm.Disconnect();
                MessageBox.Show("Upgrade for NICE was installed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                materialMultiLineTextBox3.AppendText($"An error occurred: {ex.Message}\n");
            }
        }
    }
}
