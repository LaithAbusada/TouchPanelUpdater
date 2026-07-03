using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class ConnectDisconnectForm : Form
    {
        private Form1 parentForm;
        private SettingsForm settingsForm;
        private bool isConnected;

        public event Action<bool> ConnectionStatusChanged;

        public ConnectDisconnectForm(Form1 parentForm, bool isConnected, SettingsForm settingsForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.isConnected = isConnected;
            this.settingsForm = settingsForm;
        }

        private void UpdateFormState()
        {
            if (isConnected)
            {
                btnConnectDisconnect.Text = "Disconnect";
                txtIpAddress.Visible = false;
                lblIpAddress.Visible = false;
            }
            else
            {
                btnConnectDisconnect.Text = "Connect";
                txtIpAddress.Visible = true;
                lblIpAddress.Visible = true;
            }
        }

        private async void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            DisableControls();

            if (isConnected)
            {
                // Disconnect logic
                await DisconnectDevice();
                isConnected = false;

                settingsForm.UpdateConnectionStatusLabel("No Connected Device");
                ConnectionStatusChanged?.Invoke(isConnected);
                UpdateFormState();
            }
            else
            {
                string ipAddress = txtIpAddress.Text;

                if (string.IsNullOrEmpty(ipAddress))
                {
                    MessageBox.Show("Please enter a valid IP address.");
                    EnableControls();
                    return;
                }

                using (LoadingForm loadingForm = new LoadingForm("Connecting, please wait..."))
                {
                    loadingForm.Show();
                    loadingForm.BringToFront();

                    try
                    {
                        string command = $"adb connect {ipAddress}";
                        string result = await parentForm.ExecuteAdbCommand(command);

                        if (result.Contains("connected to") && !result.Contains("cannot connect to"))
                        {
                            string deviceModel = await GetDeviceModel();
                            string deviceName = await GetDeviceName();

                            if (string.IsNullOrEmpty(deviceModel) || string.IsNullOrEmpty(deviceName))
                            {
                                MessageBox.Show("Failed to retrieve device information. Please try again.");
                                EnableControls();
                                return;
                            }

                            var lowerCaseModel = deviceModel.ToLower();
                            if (!parentForm.supportedModels.Any(s => lowerCaseModel.Contains(s)))
                            {
                                MessageBox.Show($"Connected device is {deviceModel}, but only P4 or P5 devices are supported. Disconnecting...");
                                await DisconnectDevice();
                                isConnected = false;
                                settingsForm.UpdateConnectionStatusLabel("No Connected Device");
                                ConnectionStatusChanged?.Invoke(isConnected);
                            }
                            else
                            {
                                isConnected = true;
                                settingsForm.UpdateConnectionStatusLabel($"Connected to {ipAddress}");
                                ConnectionStatusChanged?.Invoke(isConnected);
                                MessageBox.Show("Connected successfully.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to connect. Please check the IP address and try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during connection: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            UpdateFormState();
            EnableControls();
        }

        private async Task DisconnectDevice()
        {
            await parentForm.ExecuteAdbCommand("adb disconnect");
        }

        private async Task<string> GetDeviceModel()
        {
            try
            {
                string modelCommand = "adb shell getprop ro.product.model";
                string modelOutput = await parentForm.ExecuteAdbCommand(modelCommand);
                return modelOutput.Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching device model: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private async Task<string> GetDeviceName()
        {
            try
            {
                string nameCommand = "adb shell getprop ro.product.name";
                string nameOutput = await parentForm.ExecuteAdbCommand(nameCommand);
                return nameOutput.Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching device name: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void DisableControls()
        {
            this.Enabled = false;
        }

        private void EnableControls()
        {
            this.Enabled = true;
        }

        private void ConnectDisconnectForm_Load_1Async(object sender, EventArgs e)
        {
            UpdateFormState();
        }

		private void linkWiki4Inch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				// Replace with your actual Wiki URL
				System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
				{
					FileName = "https://wiki.innovo.net/en/4inadapter",
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to open link: " + ex.Message);
			}
		}

		private void linkWiki5Inch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				// Replace with your actual Wiki URL
				System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
				{
					FileName = "https://wiki.innovo.net/en/5intp",
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to open link: " + ex.Message);
			}
		}
	}
}
