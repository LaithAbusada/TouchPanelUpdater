using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class SettingsForm : Form
    {
        private Form1 parentForm;
        private bool Connected;

        public SettingsForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
            this.btnBack.Visible = false;
        }

        private async void SettingsForm_Load(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            await UpdateConnectDisconnectButton();

            if (Connected)
            {
                // Add a small delay to ensure the device connection is fully established
              

                // Try to retrieve the IP and port information after the delay
                string connectionDetails = await parentForm.GetDeviceIpAndPort();
                UpdateConnectionStatusLabel($"Connected to Device at {connectionDetails}");
            }
            else
            {
                UpdateConnectionStatusLabel("No Connected Device");
            }
        }


        private async Task UpdateConnectDisconnectButton()
        {
            Connected = await parentForm.IsConnected();
            btnConnectDisconnect.Text = Connected ? "Disconnect Device" : "Connect Device";
        }
        public void UpdateConnectDisconnectButton(string buttonText)
        {
            btnConnectDisconnect.Text = buttonText;
        }

        public void UpdateConnectionStatusLabel(string status)
        {
            lblConnectionStatus.Text = status;
        }

        public void DisableAllButtons()
        {
            SetButtonState(false);
        }

        public void EnableAllButtons()
        {
            SetButtonState(true);
        }

        private void SetButtonState(bool state)
        {
            btnSound.Enabled = state;
            btnDisplay.Enabled = state;
            btnUpdate.Enabled = state;
            btnConnectDisconnect.Enabled = state;
            btnReboot.Enabled = state;
            btnReset.Enabled = state;
            btnTimeZone.Enabled = state;
            btnFactory.Enabled = state;
        }

        private async void SettingsButton_Click(object sender, EventArgs e)
        {
            var button = sender as Guna.UI2.WinForms.Guna2Button;

            if (button == btnSound)
            {
                LoadFormIntoPanel(new SoundSettingsForm(parentForm, this));
            }
            else if (button == btnDisplay)
            {
                LoadFormIntoPanel(new DisplaySettingsForm(parentForm, this));
            }
            else if (button == btnUpdate)
            {
                LoadFormIntoPanel(new UpdateAppForm(parentForm, this));
            }
            else if (button == btnConnectDisconnect)
            {
                if (Connected)
                {
                    // Directly disconnect the device without loading a new form
                    await DisconnectDevice();
                }
                else
                {
                    LoadConnectDisconnectForm();
                }
            }
            else if (button == btnReboot)
            {
                ShowRebootConfirmation();
            }
            else if (button == btnReset)
            {
                LoadFormIntoPanel(new ResetForm(parentForm, this));
            }
            else if (button == btnTimeZone)
            {
                LoadFormIntoPanel(new TimeZoneForm(parentForm, this));
            }
            else if (button == btnFactory)
            {
                LoadFormIntoPanel(new FactoryDefaultForm(parentForm, this));
            }
        }

        private async Task DisconnectDevice()
        {
            // Logic to disconnect the device
            await parentForm.ExecuteAdbCommand("adb disconnect");
            MessageBox.Show("Disconnected successfully.", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Update UI to reflect the disconnected state
            Connected = false;
            UpdateConnectDisconnectButton("Connect Device");
            UpdateConnectionStatusLabel("No Connected Device");
            parentForm.clearMainPanel();
        }


        private async void ShowRebootConfirmation()
        {
            if (!await ConfirmDeviceConnection("No device is currently connected. Please connect a device before attempting to reboot."))
                return;

            var result = MessageBox.Show("Are you sure you want to reboot the device? This may take up to 20 seconds.", "Confirm Reboot", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (LoadingForm loadingForm = new LoadingForm("Rebooting device... Please wait."))
                {
                    loadingForm.Show();

                    try
                    {
                        await Task.Delay(500); // Small delay to ensure the loading form is visible
                        await parentForm.ExecuteAdbCommand("adb reboot");

                        // Wait for up to 20 seconds to allow the device to reboot
                        await Task.Delay(20000);

                        MessageBox.Show("Device rebooted successfully.", "Reboot Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during reboot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void ShowResetConfirmation()
        {
            if (!await ConfirmDeviceConnection("No device is currently connected. Please connect a device before attempting to reset."))
                return;

            var result = MessageBox.Show("Are you sure you want to reset the device to factory settings? This action cannot be undone.", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (LoadingForm loadingForm = new LoadingForm("Resetting device... Please wait."))
                {
                    loadingForm.Show();

                    try
                    {
                        await Task.Delay(500); // Small delay to ensure the loading form is visible
                        await parentForm.ExecuteAdbCommand("adb shell am broadcast -a android.intent.action.MASTER_CLEAR");

                        // Wait for up to 20 seconds to allow the device to reset
                        await Task.Delay(20000);

                        MessageBox.Show("Device reset successfully.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during reset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async Task<bool> ConfirmDeviceConnection(string warningMessage)
        {
            bool isConnected = await parentForm.IsConnected();
            if (!isConnected)
            {
                MessageBox.Show(warningMessage, "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void LoadFormIntoPanel(Form form)
        {
            parentForm.LoadFormIntoPanel(form);
        }

        public void LoadConnectDisconnectForm()
        {
            var connectDisconnectForm = new ConnectDisconnectForm(parentForm, Connected, this);

            connectDisconnectForm.ConnectionStatusChanged += (isConnected) =>
            {
                Connected = isConnected;
                UpdateConnectDisconnectButton();
            };

            parentForm.LoadFormIntoPanel(connectDisconnectForm);
        }
        public async Task TriggerConnectionStatusUpdate(bool isConnected)
        {
            Connected = isConnected;

            if (isConnected)
            {
                string connectionDetails = await parentForm.GetDeviceIpAndPort();
                UpdateConnectionStatusLabel($"Connected to {connectionDetails}");
            }
            else
            {
                UpdateConnectionStatusLabel("No Connected Device");
            }

            UpdateConnectDisconnectButton(isConnected ? "Disconnect Device" : "Connect Device");
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            parentForm.LoadMainOptionsIntoSidebar();
        }

        private void settingsPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
