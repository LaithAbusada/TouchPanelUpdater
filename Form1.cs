using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpAdbClient;

namespace Innovo_TP4_Updater
{
    public partial class Form1 : Form
    {
        private readonly string formname = "Touch Panel Updater";
        private bool isClosing = false;

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
            var settingsForm = new SettingsForm(this);
            LoadFormIntoSidebarPanel(settingsForm);
            // Always disconnect all devices when the form loads
            await Disconnect();

            // Load the SettingsForm on startup


            // Check if the device is connected
            settingsForm.LoadConnectDisconnectForm();
        }



        public async Task<string> GetDeviceIpAndPort()
        {
            try
            {
                // Get the list of connected devices
                string output = await ExecuteAdbCommand("adb devices");


                // Regular expression to match IP:Port
                Regex regex = new Regex(@"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}):(\d+)\s+device");
                Match match = regex.Match(output);

                if (match.Success)
                {
                    string ip = match.Groups[1].Value;
                    string port = match.Groups[2].Value;
                    return ip + ":" + port;
                }
                else
                {
                    return "No Connected Device";
                }
            }
            catch
            {
                return "No Connected Device";

            }
        }


        public async Task<string> ExecuteAdbCommand(string command)
        {
            string commandOutput = "";

            ProcessStartInfo pStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C " + command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = GetPlatformToolsPath(),  // Updated to use platform-tools path
                Verb = "runas"  // This will prompt to run as administrator
            };

            pStartInfo.EnvironmentVariables["PATH"] = $"{GetPlatformToolsPath()};{Environment.GetEnvironmentVariable("PATH")}";

            using (Process cmdProcess = new Process { StartInfo = pStartInfo })
            {
                cmdProcess.Start();
                commandOutput = await cmdProcess.StandardOutput.ReadToEndAsync();
                string error = await cmdProcess.StandardError.ReadToEndAsync();

                await WaitForExitAsync(cmdProcess);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    commandOutput += "\nError: " + error;
                }
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

        // Helper method to get the platform-tools path
        private string GetPlatformToolsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "platform-tools");
        }
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If the form is already in the process of closing, do not cancel
            if (isClosing)
            {
                e.Cancel = false; // Allow the form to close
                return;
            }

            // Indicate that the closing process has started
            isClosing = false;

            // Set e.Cancel to true to prevent the form from closing immediately
            e.Cancel = true;

            try
            {
                // Ensure all adb-related tasks are completed before proceeding
                await Disconnect();

                string adbExePath = Path.Combine(GetPlatformToolsPath(), "adb.exe");
                if (File.Exists(adbExePath))
                {
                    // Wait for the kill-server command to fully execute
                    await ExecuteAdbCommand("adb kill-server");
                    Console.WriteLine("Killed");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"Error during form closing: {ex.Message}");
            }
            finally
            {
                // Ensure isClosing is reset regardless of success or failure
                isClosing = true; // Reset isClosing in case of failure

                // Allow the form to close
                e.Cancel = false;
                this.Close();
            }
        }
    }
    }
