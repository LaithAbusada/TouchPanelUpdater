
using MaterialSkin.Controls;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using WK.Libraries.BetterFolderBrowserNS;
using System.Net;
using System.Threading.Tasks;
using AltoHttp;
using System.Drawing;
using System.Web.UI.Design;
using MaterialSkin;
using System.Reflection;

namespace Innovo_TP4_Updater
{
    public partial class Form1 : MaterialForm
    {
        StreamWriter stdin = null;
        String path = null;
        String formname = "Touch Panel Updater";

        int install_count = 0;
        private Process cmdProcess;

        public Form1()
        {
            this.BackColor = Color.Black;
            InitializeComponent();
            this.Text = formname;
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = false;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Yellow400, Primary.Yellow400, Primary.Yellow100, Accent.Yellow200, TextShade.BLACK);

            button3.BringToFront();
            label5.Size = new Size(400, 50);


        }

        public void installadb()
        {

            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

            // Combine solution directory with "Downloads" folder and adb.exe
            string adbPath = Path.Combine(solutionDirectory,"adb.exe");

            Console.WriteLine(adbPath);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            installadb();
            loadtext();
          

            // Start with the PasswordForm
           


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //                 

        }

        private void Button2_MouseEnter(object sender, EventArgs e)
        {
            button1.Text = "Disconnect";
        }

        private void Button2_MouseLeave(object sender, EventArgs e)
        {
            button1.Text = "Disconnect";
        }


        private void StartCmdProcess()
        {
            ProcessStartInfo pStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "start /WAIT",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
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

            cmdProcess.Start();
            materialMultiLineTextBox1.Text = string.Empty;
            cmdProcess.BeginErrorReadLine();
            cmdProcess.BeginOutputReadLine();
            stdin = cmdProcess.StandardInput;

            cmdProcess.OutputDataReceived += (s, evt) =>
            {
                if (evt.Data != null)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        Console.WriteLine(evt.Data);
                        materialMultiLineTextBox1.AppendText(evt.Data + Environment.NewLine);
                        materialMultiLineTextBox1.ScrollToCaret();
                    }));
                }
            };

            cmdProcess.ErrorDataReceived += (s, evt) =>
            {
                if (evt.Data != null)
                {
                    BeginInvoke(new Action(() =>
                    {
                        Console.WriteLine(evt.Data);
                        //rtbStdErr.AppendText(evt.Data + Environment.NewLine);
                        //rtbStdErr.ScrollToCaret();
                    }));
                }
            };

            cmdProcess.Exited += (s, evt) =>
            {
                // cmdProcess?.Dispose();
            };
        }






        private void materialMultiLineTextBox1_TextChanged(object sender, EventArgs e)
        {

            try
            {

                replace();

                int maxLines = materialMultiLineTextBox1.Lines.Length;
                string lastLine = materialMultiLineTextBox1.Lines[maxLines - 2];
                string lastWord = lastLine.Split(' ').Last();

                if (lastLine == "Success")
                {
                    install_count++;
                    progresstext();
                }
            }
            catch
            {

            }



        }


        void progresstext()
        {
            materialMultiLineTextBox1.AppendText("----------------------------------------------" + Environment.NewLine + "-- Installed number of apk " + install_count + " --" + Environment.NewLine + "----------------------------------------------" + Environment.NewLine);
        }
        void loadtext()
        {

        }
        void replace()
        {
            String Directory = System.Windows.Forms.Application.StartupPath;
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace(Directory, "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace(">", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("for %e in", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("%", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("(", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace(path, "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("*.apk)", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("do", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("adb install", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("@echo", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("off", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("-r", "");
            materialMultiLineTextBox1.Text = materialMultiLineTextBox1.Text.Replace("\"%e\"", "");
        }


        private async void disconnect()
        {
            label1.Text = String.Empty;
            materialMultiLineTextBox3.AppendText(Environment.NewLine);
            string disconnect = "adb disconnect ";
            materialMultiLineTextBox3.AppendText("Disconnecting  devices..." + Environment.NewLine);
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
            materialMultiLineTextBox3.AppendText("Disconnect Devices successfully" + Environment.NewLine);
        }

        private async Task rebootAsync()
        {

        }
        private async void button3_Click_1Async(object sender, EventArgs e)
        {


        }

        private void button4_Click(object sender, EventArgs e)
        {


        }

        private async void button1_Click_1Async(object sender, EventArgs e)
        {

            materialMultiLineTextBox3.Text = "";
            Console.WriteLine(Application.StartupPath);
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
            }

            WebClient client = new WebClient();
            string latest_version = client.DownloadString("https://innovo.net/repo/TP4/version");






            string ipAddress = textBoxIP.Text;
            ipAddress = ipAddress.Replace(" ", "");
            int port = int.Parse(textBoxPort.Text);

            string connect = "adb connect " + ipAddress + ":" + port;
            materialMultiLineTextBox3.AppendText("Attempting to connect to device, Please wait" + Environment.NewLine);

            StartCmdProcess();
            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Trying to establish connection --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(connect + Environment.NewLine);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Add a delay to allow time for the connection process
            await Task.Delay(7000);

            // Now check if the connection was successful by searching for the confirmation message
            string[] lines = materialMultiLineTextBox1.Lines;
            string searchWord = "connected"; // Update this with the confirmation message
            bool wordFound = false;
            string searchWord2 = "cannot";
            bool wordFound2 = false;

            foreach (string line in lines)
            {
                if (line.Contains(searchWord))
                {
                    // Word found in this line
                    wordFound = true;
                    // Exit the loop since we found the word
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

            cmdProcess.CloseMainWindow();
            materialMultiLineTextBox3.AppendText("Connected successfully" + Environment.NewLine);
            materialMultiLineTextBox3.AppendText(Environment.NewLine);
            await Task.Delay(4000);


           


            string version = "adb shell dumpsys package com.homelogic | findstr versionName";
            StartCmdProcess();

            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Checking App Version --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(version + Environment.NewLine);

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            cmdProcess.CloseMainWindow();
            materialMultiLineTextBox3.AppendText("Checking if update is available, Please wait " + Environment.NewLine);
            await Task.Delay(8000);
            string deviceVersion = null;
            string[] lines2 = materialMultiLineTextBox1.Lines;
            foreach (string line in lines2)
            {
                if (line.Contains("versionName="))
                {
                    deviceVersion = line.Split('=')[1].Trim(); // Extract the version name

                    break;
                }
            }

            // Compare the device version with the latest version from the server
            Console.WriteLine(deviceVersion);
            latest_version = latest_version.Trim();
            if (deviceVersion != null && deviceVersion.Equals(latest_version.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                 disconnect();
                // Versions match, no update available
                MessageBox.Show("Panel already up to date ! ", "Up to Date", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
                
            }
            else
            {
                // Versions don't match, update available
                materialMultiLineTextBox3.AppendText("Update Available" + Environment.NewLine);
            }
            materialMultiLineTextBox3.AppendText("Update Available" + Environment.NewLine);
            materialMultiLineTextBox3.AppendText(Environment.NewLine);


            materialMultiLineTextBox3.AppendText("Checking device model and screen size" + Environment.NewLine);

            // Check device model
            string modelCommand = "adb shell getprop ro.product.model";
            string versionCommand = "adb shell getprop ro.build.version.release";

            StartCmdProcess();
            string deviceModel = null;
           
            try
            {
                stdin.WriteLine("@echo off");
                stdin.WriteLine("---------------------------------------------------------------");
                stdin.WriteLine("-- Checking Device Model --");
                stdin.WriteLine("---------------------------------------------------------------");
                stdin.WriteLine(modelCommand);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmdProcess.CloseMainWindow();
            await Task.Delay(3000);

            // Capture the device model from output
           var lines3 = materialMultiLineTextBox1.Lines;
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
            // Check device version
            StartCmdProcess();
            try
            {
                stdin.WriteLine("@echo off");
                stdin.WriteLine("---------------------------------------------------------------");
                stdin.WriteLine("-- Checking Device Version --");
                stdin.WriteLine("---------------------------------------------------------------");
                stdin.WriteLine(versionCommand);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cmdProcess.CloseMainWindow();
            await Task.Delay(2000);

           

            materialMultiLineTextBox3.AppendText("Model: " + deviceModel + ", Version: " + deviceVersion + Environment.NewLine);

            // Check if the model is P4 and version is greater than 8.9.02
            if (deviceModel == "P4" && string.Compare(deviceVersion, "8.9.02") > 0)
            {
                StartCmdProcess();
                try
                {
                    stdin.Write("@echo off" + Environment.NewLine);
                    stdin.WriteLine("---------------------------------------------------------------");
                    stdin.WriteLine("-- Changing Screen Size --");
                    stdin.WriteLine("---------------------------------------------------------------");
                    stdin.Write("adb shell wm size 479x480" + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cmdProcess.CloseMainWindow();
                }
                MessageBox.Show("Changed screen size successfully");
            }
            return;







            materialMultiLineTextBox3.AppendText("Starting Installation Process, This can take up to 20 seconds " + Environment.NewLine);
            string installcom = $@"for %f in ({downloadDirectory}\*.apk) do adb install -g -r -d ""%f""";
            StartCmdProcess();
            MessageBox.Show("Installation can take up to 20 seconds, please wait", "Installation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Wait a minute the installation started --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(installcom + Environment.NewLine);
            }
            catch
            {
                // Handle exception
                MessageBox.Show("installation error has occured");
            }
            finally
            {



            }

            await Task.Delay(5000);
            materialMultiLineTextBox3.AppendText("Installation was successful, please wait for device reboot" + Environment.NewLine);
            cmdProcess.CloseMainWindow();
            await Task.Delay(2000);
            materialMultiLineTextBox3.AppendText("Rebooting Device ,This can take up to 20 seconds, please Wait" + Environment.NewLine);
            ipPort = label5.Text;
            string rebootCommand = "adb reboot";

            StartCmdProcess();
            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Rebooting Device --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(rebootCommand + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            materialMultiLineTextBox3.AppendText("Rebooting Device in 3 ... 2 ... 1 ... " + Environment.NewLine);
            materialMultiLineTextBox3.AppendText("Please wait at until reboot is complete" + Environment.NewLine);
            MessageBox.Show("Device reboot can take up to 20 seconds, please wait", "Reboot", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await Task.Delay(20000);



            // Add a delay to allow time for the reboot process

            label5.Text = string.Empty; // Clear the device information after rebooting
            materialMultiLineTextBox3.AppendText("Reboot completed successfully" + Environment.NewLine);
            materialMultiLineTextBox1.Text = "Device Rebooted Successfully";

            cmdProcess.CloseMainWindow();

            materialMultiLineTextBox3.AppendText(Environment.NewLine);
            disconnect();

            MessageBox.Show("Upgrade for NICE was installed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Add a delay to allow time for the connection process
            await Task.Delay(5000);

            
            }
        

        private void materialMultiLineTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            label5.Text = string.Empty;
            materialMultiLineTextBox3.Text = string.Empty;
            string disconnect = "adb disconnect ";
            materialMultiLineTextBox3.AppendText("Disconnecting  devices...");
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
            await Task.Delay(3000);
            materialMultiLineTextBox3.AppendText("Disconnected Device Successfully" + Environment.NewLine);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Warning, This will delete all application data and reset the app from scratch, Do you want to continue?", "Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No) return;

            materialMultiLineTextBox1.Text = String.Empty;
            materialMultiLineTextBox3.Text = String.Empty;
            materialMultiLineTextBox3.AppendText("Attempting to connect and reset, please wait" + Environment.NewLine);
            string ipAddress = textBoxIP.Text;
            ipAddress = ipAddress.Replace(" ", "");
            int port = int.Parse(textBoxPort.Text);

            string connect = "adb connect " + ipAddress + ":" + port;

            string reset = "adb shell pm clear com.homelogic";
            StartCmdProcess();
            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Trying to establish connection --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(connect + Environment.NewLine);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            await Task.Delay(7000);
            cmdProcess.CloseMainWindow();
            // Now check if the connection was successful by searching for the confirmation message
            string[] lines = materialMultiLineTextBox1.Lines;
            string searchWord = "connected"; // Update this with the confirmation message
            bool wordFound = false;
            string searchWord2 = "cannot";
            bool wordFound2 = false;

            foreach (string line in lines)
            {
                if (line.Contains(searchWord))
                {
                    // Word found in this line
                    wordFound = true;
                    // Exit the loop since we found the word
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

            materialMultiLineTextBox3.AppendText("Connected Successfully" + Environment.NewLine);
            StartCmdProcess();
            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Rebooting Device --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(reset + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            await Task.Delay(5000);
            cmdProcess.CloseMainWindow();
            materialMultiLineTextBox3.AppendText("Application reseted successfully" + Environment.NewLine);

            string rebootCommand = "adb reboot";
            StartCmdProcess();
            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Rebooting Device --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(rebootCommand + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            materialMultiLineTextBox3.AppendText("Rebooting Device in 3 ... 2 ... 1 ... " + Environment.NewLine);
            materialMultiLineTextBox3.AppendText("Please wait at until reboot is complete" + Environment.NewLine);
            MessageBox.Show("Device reboot can take up to 20 seconds, please wait", "Reboot", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await Task.Delay(20000);
            materialMultiLineTextBox3.AppendText("Reboot Complete" + Environment.NewLine);
            cmdProcess.CloseMainWindow();

            await Task.Delay(2000);
            disconnect();

        }

        private async void button4_Click_1(object sender, EventArgs e)
        {
            materialMultiLineTextBox1.Text = String.Empty;
            materialMultiLineTextBox3.Text = String.Empty;
            string ipAddress = textBoxIP.Text;
            ipAddress = ipAddress.Replace(" ", "");
            int port = int.Parse(textBoxPort.Text);
            string connect = "adb connect " + ipAddress + ":" + port;


            materialMultiLineTextBox3.AppendText("Attempting to connect and reboot, please wait" + Environment.NewLine);
            StartCmdProcess();
            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Trying to establish connection --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(connect + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            await Task.Delay(7000);

            string[] lines = materialMultiLineTextBox1.Lines;
            string searchWord = "connected"; // Update this with the confirmation message
            bool wordFound = false;
            string searchWord2 = "cannot";
            bool wordFound2 = false;

            foreach (string line in lines)
            {
                if (line.Contains(searchWord))
                {
                    // Word found in this line
                    wordFound = true;
                    // Exit the loop since we found the word
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
            cmdProcess.CloseMainWindow();

            string rebootCommand = "adb reboot";
            StartCmdProcess();
            try
            {
                stdin.Write("\u0040echo off" + Environment.NewLine);
                stdin.Write("---------------------------------------------------------------" + Environment.NewLine + "-- Rebooting Device --" + Environment.NewLine + "---------------------------------------------------------------" + Environment.NewLine);
                stdin.Write(rebootCommand + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            materialMultiLineTextBox3.AppendText("Rebooting Device in 3 ... 2 ... 1 ... " + Environment.NewLine);
            materialMultiLineTextBox3.AppendText("Please wait at until reboot is complete" + Environment.NewLine);
            MessageBox.Show("Device reboot can take up to 20 seconds, please wait", "Reboot", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await Task.Delay(20000);
            materialMultiLineTextBox3.AppendText("Reboot Complete" + Environment.NewLine);
            cmdProcess.CloseMainWindow();
            await Task.Delay(2000);
            disconnect();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}


