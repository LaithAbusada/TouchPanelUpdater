using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpAdbClient;

namespace Innovo_TP4_Updater
{
    public partial class Form1 : Form
    {
        private readonly string formname = "Touch Panel Updater";

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.BackColor = Color.Black;
            this.Text = formname;
            label5.Size = new Size(400, 50);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.ControlBox = true;
            this.MinimizeBox = true;
            this.MaximizeBox = true;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Always disconnect all devices when the form loads
            await Disconnect();

            // Load the SettingsForm on startup
            var settingsForm = new SettingsForm(this);
            LoadFormIntoSidebarPanel(settingsForm);

            // Check if the device is connected
            settingsForm.LoadConnectDisconnectForm();
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            // Disconnect all devices before closing the form
            await Disconnect();
            base.OnFormClosing(e);
        }

        public async Task<string> GetDeviceIpAndPort()
        {
            try
            {
                // Get the list of connected devices
                string output = await ExecuteAdbCommand("adb devices -l");

                // Check if the output contains any connected devices
                if (string.IsNullOrWhiteSpace(output) || !output.Contains("\tdevice"))
                {
                    return "No Connected Device";
                }
                MessageBox.Show("yo");

                // Define a regular expression to match the IP and port
                var regex = new System.Text.RegularExpressions.Regex(@"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5})");

                // Parse the output to find the device's IP address and port
                var matches = regex.Matches(output);

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    if (match.Success)
                    {
                        return match.Value; // Return the matched IP and Port
                    }
                }

                return "No IP/Port found";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving IP and port: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Unknown IP/Port";
            }
        }

        public async Task<string> ExecuteAdbCommand(string command)
        {
            string commandOutput = "";
            ProcessStartInfo pStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C " + command,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process cmdProcess = new Process { StartInfo = pStartInfo })
            {
                cmdProcess.Start();
                commandOutput = await cmdProcess.StandardOutput.ReadToEndAsync();
                await WaitForExitAsync(cmdProcess);
            }

            return commandOutput;
        }

        private Task WaitForExitAsync(Process process)
        {
            var tcs = new TaskCompletionSource<object>();

            void ProcessExitedHandler(object sender, EventArgs e)
            {
                // Avoid multiple transitions
                if (!tcs.Task.IsCompleted)
                {
                    tcs.TrySetResult(null);
                }
                // Clean up event handler
                process.Exited -= ProcessExitedHandler;
            }

            process.EnableRaisingEvents = true;
            process.Exited += ProcessExitedHandler;

            // Check if the process has already exited
            if (process.HasExited)
            {
                process.Exited -= ProcessExitedHandler;
                tcs.TrySetResult(null);
            }

            return tcs.Task;
        }

        public async Task<Dictionary<string, int>> GetCurrentVolumeLevels()
        {
            Dictionary<string, int> volumeLevels = new Dictionary<string, int>
            {
                { "Media", ParseVolumeLevel(await ExecuteAdbCommand("adb shell media volume --get --stream 3"), 3) },
                { "Notifications", ParseVolumeLevel(await ExecuteAdbCommand("adb shell media volume --get --stream 5"), 5) },
                { "Alarm", ParseVolumeLevel(await ExecuteAdbCommand("adb shell media volume --get --stream 4"), 4) },
                { "Bluetooth", ParseVolumeLevel(await ExecuteAdbCommand("adb shell media volume --get --stream 6"), 6) } // Assuming stream 6 for Bluetooth
            };
            return volumeLevels;
        }

        private int ParseVolumeLevel(string output, int stream)
        {
            string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (line.Contains($"Volume of stream {stream}"))
                {
                    string[] parts = line.Split(' ');
                    if (int.TryParse(parts.Last(), out int volumeLevel))
                    {
                        return volumeLevel;
                    }
                }
            }
            return 0;
        }

        private void InstallAdb()
        {
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string adbPath = Path.Combine(solutionDirectory, "adb.exe");

            if (File.Exists(adbPath))
            {
                AdbServer server = new AdbServer();
                var result = server.StartServer(adbPath, restartServerIfNewer: false);
                Console.WriteLine("Adb file exists, and success");
            }
            else
            {
                Console.WriteLine("ADB executable not found. Please make sure it is located in the 'Downloads' folder within your solution directory.");
            }
        }

        private void LoadText() { }

        public async Task Disconnect()
        {
            string disconnect = "adb disconnect";
            await ExecuteAdbCommand(disconnect);
            Console.WriteLine("Disconnected devices successfully");
        }

        public void LoadFormIntoPanel(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.mainPanel.Controls.Clear();
            this.mainPanel.Controls.Add(form);
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // SettingsForm settingsForm = new SettingsForm();
            // LoadFormIntoPanel(settingsForm);
        }

        private void button6_Click(object sender, EventArgs e) { }

        private void button7_Click(object sender, EventArgs e)
        {
            LoadFormIntoSidebarPanel(new SettingsForm(this));
        }

        public void LoadFormIntoSidebarPanel(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            // Clear any existing controls and add the new form to the sidebarPanel
            this.sidebarPanel.Controls.Clear();
            this.sidebarPanel.Controls.Add(form);

            // Ensure the form size matches the sidebarPanel size
            form.Size = sidebarPanel.ClientSize;

            // Set the form's location to the top-left of the sidebarPanel
            form.Location = new Point(0, 0);

            // Show the form
            form.Show();
        }

        public async Task<bool> IsConnected()
        {
            string output = await ExecuteAdbCommand("adb devices");
            return output.Contains("\tdevice");
        }

        public void LoadMainOptionsIntoSidebar()
        {
            // Clear the sidebar panel
            sidebarPanel.Controls.Clear();

            // Recreate the original sidebar options (Controller, Apps, Settings)
            sidebarPanel.Controls.Add(button5);
            sidebarPanel.Controls.Add(button6);
            sidebarPanel.Controls.Add(button7);

            // Reset the location of the buttons
            button5.Location = new Point(14, 177);
            button6.Location = new Point(11, 253);
            button7.Location = new Point(12, 331);
        }

        public void clearMainPanel()
        {
            this.mainPanel.Controls.Clear();
        }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
