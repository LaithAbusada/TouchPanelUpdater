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
            await ResetApplication("com.example.app1", button.Text); // Replace with your actual app package name
        }

        private async void btnApp2_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("com.example.app2", button.Text); // Replace with your actual app package name
        }

        private async void btnApp3_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("com.example.app3", button.Text); // Replace with your actual app package name
        }

        private async void btnApp4_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            await ResetApplication("com.example.app4", button.Text); // Replace with your actual app package name
        }

        private async Task ResetApplication(string packageName, string appName)
        {
            // Check if the device is connected
            bool isConnected = await parentForm.IsConnected();

            if (!isConnected)
            {
                MessageBox.Show("No device is currently connected. Please connect a device before attempting to reset.", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                try
                {
                    // Show loading form
                    LoadingForm loadingForm = new LoadingForm("Resetting cache and storage... Please wait.");
                    loadingForm.Show();

                    // Execute the adb commands to reset the cache and storage
                    await parentForm.ExecuteAdbCommand($"adb shell pm clear {packageName}");

                    MessageBox.Show($"{appName} cache and storage reset successfully.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during reset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
