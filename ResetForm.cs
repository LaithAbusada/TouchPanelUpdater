using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class ResetForm : Form
    {
        private Form1 parentForm;

        public ResetForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;

            // Subscribe to the Load event
            this.Load += new EventHandler(ResetForm_Load);
        }

        private async void ResetForm_Load(object sender, EventArgs e)
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

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
            await ResetApplication("Nice"); // Use the app name to determine the package name
        }

        private async void btnApp2_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Control4");
        }

        private async void btnApp3_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Rako");
        }

        private async void btnApp4_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("Lutron");
        }

        private async Task ResetApplication(string appName)
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
                    // Show loading form for resetting cache and storage
                    loadingForm = new LoadingForm("Resetting cache and storage... Please wait.");
                    loadingForm.Show();

                    // Execute the adb command to reset the cache and storage
                    await parentForm.ExecuteAdbCommand($"adb shell pm clear {packageName}");

                    MessageBox.Show($"{appName} cache and storage reset successfully.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Close the first loading form
                    loadingForm.Close();

                    // Show a new loading form for rebooting
                    loadingForm = new LoadingForm("Rebooting the device... Please wait.");
                    loadingForm.Show();

                    // Execute the adb command to reboot the device
                    await parentForm.ExecuteAdbCommand("adb reboot");

                    MessageBox.Show("The device is rebooting.", "Rebooting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during reset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Ensure the loading form is closed in all cases
                    loadingForm?.Close();
                }
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
