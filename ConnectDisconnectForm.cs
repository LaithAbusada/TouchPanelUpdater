using System;
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

        private void ConnectDisconnectForm_Load(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void UpdateFormState()
        {
            if (isConnected)
            {
                btnConnectDisconnect.Text = "Disconnect";
                txtIpAddress.Visible = false;
                txtPort.Visible = false;
                lblIpAddress.Visible = false;
                lblPort.Visible = false;
            }
            else
            {
                btnConnectDisconnect.Text = "Connect";
                txtIpAddress.Visible = true;
                txtPort.Visible = true;
                lblIpAddress.Visible = true;
                lblPort.Visible = true;
            }
        }

        private async void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            DisableControls();

            if (isConnected)
            {
                // Disconnect logic
                await parentForm.ExecuteAdbCommand("adb disconnect");
                MessageBox.Show("Disconnected successfully.");
                isConnected = false;

                // Update label in SettingsForm
                settingsForm.UpdateConnectionStatusLabel("No Connected Device");

                ConnectionStatusChanged?.Invoke(isConnected);
                UpdateFormState();
            }
            else
            {
                // Validate input
                string ipAddress = txtIpAddress.Text;
                string port = txtPort.Text;

                if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(port))
                {
                    MessageBox.Show("Please enter a valid IP address and port.");
                    EnableControls();
                    return;
                }

                // Show the loading form
                using (LoadingForm loadingForm = new LoadingForm("Connecting, please wait..."))
                {
                    loadingForm.Show();
                    loadingForm.BringToFront();

                    try
                    {
                        // Attempt to connect
                        string command = $"adb connect {ipAddress}:{port}";
                        string result = await parentForm.ExecuteAdbCommand(command);
                        // Check if the connection was successful
                        if (result.Contains("connected to") && !result.Contains("cannot connect to"))
                        {
                            isConnected = true;

                            // Update label in SettingsForm
                            settingsForm.UpdateConnectionStatusLabel($"Connected to {ipAddress}:{port}");

                            ConnectionStatusChanged?.Invoke(isConnected);
                            MessageBox.Show("Connected successfully.");
                        }
                        else
                        {
                           
                            MessageBox.Show("Failed to connect. Please check the IP address and port and try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during connection: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // Update the form state and re-enable controls
            UpdateFormState();
            EnableControls();
        }

        private void DisableControls()
        {
            this.Enabled = false; // Disable the entire form to prevent interaction
        }

        private void EnableControls()
        {
            this.Enabled = true; // Re-enable the form after the connection process is complete
        }
    }
}
