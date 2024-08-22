using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class ResetForm : Form
    {
        private Form1 parentForm;
        private readonly SettingsForm settingsForm;

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
        }

        private async void btnApp1_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Nice", button);
        }

        private async void btnApp2_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Control4", button);
        }

        private async void btnApp3_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Rako", button);
        }

        private async void btnApp4_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Lutron", button);
        }

        private async Task ResetApplication(string appName, Guna.UI2.WinForms.Guna2Button clickedButton)
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

            if (!isConnected)
            {
                MessageBox.Show("No device is currently connected. Please connect a device before attempting to reset.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the package name from the app name
            string packageName = GetPackageName(appName);
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
                    DisableAndHideButtons(clickedButton);

                    // Disable all buttons in the SettingsForm
                    settingsForm.DisableAllButtons();

                    // Show loading form for resetting cache and storage
                    loadingForm = new LoadingForm("Resetting cache and storage... Please wait.");
                    loadingForm.Show();

                    // Execute the adb command to reset the cache and storage
                    await parentForm.ExecuteAdbCommand($"adb shell pm clear {packageName}");

                    MessageBox.Show($"{appName} cache and storage reset successfully.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    ShowAndEnableButtons();
                }
            }
        }
        private void DisableAndHideButtons(Guna.UI2.WinForms.Guna2Button clickedButton)
        {
            // Disable and keep the clicked button visible
            clickedButton.Enabled = false;

            // Hide the other buttons
            if (clickedButton != btnApp1)
            {
                btnApp1.Visible = false;
            }
            if (clickedButton != btnApp2)
            {
                btnApp2.Visible = false;
            }
            if (clickedButton != btnApp3)
            {
                btnApp3.Visible = false;
            }
            if (clickedButton != btnApp4)
            {
                btnApp4.Visible = false;
            }
        }

        private void ShowAndEnableButtons()
        {
            // Show and enable all buttons
            btnApp1.Visible = true;
            btnApp2.Visible = true;
            btnApp3.Visible = true;
            btnApp4.Visible = true;

            btnApp1.Enabled = true;
            btnApp2.Enabled = true;
            btnApp3.Enabled = true;
            btnApp4.Enabled = true;
        }
        private async Task RebootDevice()
        {
            LoadingForm loadingForm = null;

            try
            {
                // Show a new loading form for rebooting
                loadingForm = new LoadingForm("Rebooting the device... Please wait.");
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

        private string GetPackageName(string appName)
        {
            switch (appName)
            {
                case "Nice":
                    return "com.homelogic";
                case "Lutron":
                    return "com.lutron.mmw";
                case "Control4":
                    return "com.control4.phoenix";
                case "Rako":
                    return "com.rakocontrols.android";
                default:
                    return null;
            }
        }
    }
}
