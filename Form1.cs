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
        private StreamWriter stdin = null;
        private Process cmdProcess;
        private readonly string formname = "Touch Panel Updater";
        private string commandOutput = "";
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

        private void Form1_Load(object sender, EventArgs e)
        {
            InstallAdb();
            LoadText();
        }

        private void StartCmdProcess()
        {
            ProcessStartInfo pStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C", // Ensure the command completes
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            cmdProcess = new Process
            {
                StartInfo = pStartInfo,
                EnableRaisingEvents = true,
            };

            cmdProcess.OutputDataReceived += (s, evt) =>
            {
                if (evt.Data != null)
                {
                    commandOutput += evt.Data + Environment.NewLine;
                }
            };

            cmdProcess.ErrorDataReceived += (s, evt) =>
            {
                if (evt.Data != null)
                {
                    commandOutput += "Error: " + evt.Data + Environment.NewLine;
                }
            };

            cmdProcess.Exited += (s, evt) =>
            {
                BeginInvoke(new Action(() =>
                {
                    Console.WriteLine("Process exited.");
                }));
            };

            cmdProcess.Start();
            cmdProcess.BeginOutputReadLine();
            cmdProcess.BeginErrorReadLine();
            stdin = cmdProcess.StandardInput;
        }

        private Task WaitForExitAsync()
        {
            var tcs = new TaskCompletionSource<object>();

            void ProcessExited(object sender, EventArgs e)
            {
                cmdProcess.Exited -= ProcessExited;
                tcs.SetResult(null);
            }

            cmdProcess.Exited += ProcessExited;

            if (cmdProcess.HasExited)
            {
                cmdProcess.Exited -= ProcessExited;
                return Task.CompletedTask;
            }

            return tcs.Task;
        }

        public async Task<string> ExecuteAdbCommand(string command)
        {
            commandOutput = "";
            StartCmdProcess();
            try
            {
                stdin.WriteLine(command);
                stdin.WriteLine("exit"); // Ensure the command ends properly
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            await WaitForExitAsync(); // Ensure we wait for the process to exit
            return commandOutput;
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

        public async void Disconnect()
        {
            label1.Text = string.Empty;
            string disconnect = "adb disconnect ";
            StartCmdProcess();
            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Trying to establish connection --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(disconnect + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            await Task.Delay(4000);
            Console.WriteLine("Disconnect Devices successfully");
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
            LoadFormIntoPanel(new SettingsForm(this));
        }

        public async Task<bool> IsConnected()
        {
            string output = await ExecuteAdbCommand("adb devices");
            return output.Contains("\tdevice");
        }
    }
}
