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
            UpdateConnectDisconnectButton();
            this.btnBack.Visible = false;
        }

        private async void UpdateConnectDisconnectButton()
        {
            bool isConnected = await parentForm.IsConnected();
            if (isConnected)
            {
                btnConnectDisconnect.Text = "Disconnect Device";
                Connected = true;
            }
            else
            {
                btnConnectDisconnect.Text = "Connect Device";
                Connected = false;
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;

            if (button == btnSound)
            {
                SoundSettingsForm soundSettingsForm = new SoundSettingsForm(parentForm);
                parentForm.LoadFormIntoPanel(soundSettingsForm);
            }
            else if (button == btnDisplay)
            {
                DisplaySettingsForm displaySettingsForm = new DisplaySettingsForm(parentForm);
                parentForm.LoadFormIntoPanel(displaySettingsForm);
            }
            else if (button == btnUpdate)
            {
                UpdateAppForm updateAppForm = new UpdateAppForm(parentForm);
                parentForm.LoadFormIntoPanel(updateAppForm);
            }
            else if (button == btnConnectDisconnect)
            {
                ConnectDisconnectForm connectDisconnectForm = new ConnectDisconnectForm(parentForm, Connected);

                // Subscribe to the ConnectionStatusChanged event
                connectDisconnectForm.ConnectionStatusChanged += (isConnected) =>
                {
                    Connected = isConnected;
                    UpdateConnectDisconnectButton();
                };

                parentForm.LoadFormIntoPanel(connectDisconnectForm);
            }
            else if (button == btnReboot)
            {
                ShowRebootConfirmation();
            }
            else if (button == btnReset)
            {
                ResetForm resetForm = new ResetForm(parentForm);
                parentForm.LoadFormIntoPanel(resetForm);
            }
            else if (button == btnTimeZone)
            {
                TimeZoneForm timeZoneForm = new TimeZoneForm(parentForm);
                parentForm.LoadFormIntoPanel(timeZoneForm);
            }
            else if (button== btnFactory)
            {
                FactoryDefaultForm factoryDefaultForm = new FactoryDefaultForm(parentForm);
                parentForm.LoadFormIntoPanel(factoryDefaultForm);
            }
        }




        private async void ShowRebootConfirmation()
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

            if (!isConnected)
            {
                MessageBox.Show("No device is currently connected. Please connect a device before attempting to reboot.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to reboot the device? This may take up to 20 seconds.", "Confirm Reboot", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Show loading form
                LoadingForm loadingForm = new LoadingForm("Rebooting device... Please wait.");
                loadingForm.Show();

                try
                {
                    // Execute reboot command
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
                finally
                {
                    loadingForm.Close(); // Close the loading form when done
                }
            }
        }

        private async void ShowResetConfirmation()
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

            if (!isConnected)
            {
                MessageBox.Show("No device is currently connected. Please connect a device before attempting to reset.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to reset the device to factory settings? This action cannot be undone.", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Show loading form
                LoadingForm loadingForm = new LoadingForm("Resetting device... Please wait.");
                loadingForm.Show();

                try
                {
                    // Execute reset command
                    await Task.Delay(500); // Small delay to ensure the loading form is visible
                    await parentForm.ExecuteAdbCommand("adb shell am broadcast -a android.intent.action.MASTER_CLEAR"); // Example command for factory reset

                    // Wait for up to 20 seconds to allow the device to reset
                    await Task.Delay(20000);

                    MessageBox.Show("Device reset successfully.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during reset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    loadingForm.Close(); // Close the loading form when done
                }
            }
        }
        public void LoadConnectDisconnectForm()
        {
            ConnectDisconnectForm connectDisconnectForm = new ConnectDisconnectForm(parentForm, Connected);
            parentForm.LoadFormIntoPanel(connectDisconnectForm);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            parentForm.LoadMainOptionsIntoSidebar();
        }
    }
}
