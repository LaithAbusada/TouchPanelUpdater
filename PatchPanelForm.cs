using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class PatchPanelForm : Form
    {
        private Form1 parentForm;
        private SettingsForm settingsForm;
        public PatchPanelForm(Form1 parent, SettingsForm settingsForm)
        {
            InitializeComponent();
            parentForm = parent;
            this.settingsForm = settingsForm;
        }


        private async void PatchPanelForm_LoadAsync(object sender, EventArgs e)
        {
            bool isConnected = await parentForm.IsConnected();
            await settingsForm.TriggerConnectionStatusUpdate(isConnected);

            // Check if the device is connected
            if (!isConnected)
            {
                parentForm.clearMainPanel();
                MessageBox.Show("No device is currently connected. Please connect a device before proceeding.",
                                "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        public async Task<bool> UninstallWithRetryAsync(string packageName)
        {
            const int maxAttempts = 3;
            bool uninstalled = false;
            int attempts = 0;

            while (attempts < maxAttempts && !uninstalled)
            {
                // 1) Attempt uninstall for user 0
                string uninstallCmd = $"adb shell pm uninstall --user 0 {packageName}";
                var uninstallResult = await parentForm.ExecuteAdbCommand(uninstallCmd);
                await Task.Delay(7000);

                // 2) Check result – adb returns "Success" when user‑uninstall succeeds
                if (uninstallResult.Trim().StartsWith("Success", StringComparison.OrdinalIgnoreCase))
                {
                    // 3) Immediately verify by listing packages
                    string listCmd = $"adb shell pm list packages | findstr {packageName}";
                    var listResult = await parentForm.ExecuteAdbCommand(listCmd);
                    if (string.IsNullOrWhiteSpace(listResult))
                    {
                        // Gone for good
                        uninstalled = true;
                    }
                    else
                    {
                        // Still present, retry
                        attempts++;
                        await Task.Delay(500);
                    }
                }
                else
                {
                    // Uninstall call itself failed or returned something unexpected
                    attempts++;
                    await Task.Delay(500);
                }
            }

            return uninstalled;
        }

        async private void btnPatch_Click(object sender, EventArgs e)
        {
            LoadingForm loadingForm = new LoadingForm("Checking For Panel Eligiblity For Patch");
            loadingForm.Show();
            loadingForm.BringToFront();
            const string expectedBuildId = "Innovo-P6601_ZZW395WHS-1254B.480x480-20250701-Z-V0.02";
            try
            {
                // 1. Read the build ID and trim whitespace/newlines
                var rawBuildId = await parentForm.ExecuteAdbCommand(
                    "adb shell getprop ro.build.display.id");
                var buildId = rawBuildId.Trim();

                // 2. Compare
                if (!buildId.Equals(expectedBuildId, StringComparison.Ordinal))
                {
                    MessageBox.Show(
                        $"Patch not applicable.\nBuild ID is '{buildId}'",
                        "Info",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                loadingForm.UpdateMessage("Patching Panel, Please wait ....");

                // 3. Enable TCP‑ADB on port 5555
                await parentForm.ExecuteAdbCommand(
                    "adb shell setprop persist.adb.tcp.port 5555");


                await Task.Delay(3000);

                // 4. Attempt uninstall up to 3 times
                bool uninstalled = await UninstallWithRetryAsync("com.hykonmobile.mobile.phone");


                // 5. Final feedback
                if (uninstalled)
                {
                    MessageBox.Show(
                        "Patch applied and app uninstalled successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    loadingForm.UpdateMessage("Rebooting the device...");

                    string rebootCommand = "adb reboot";
                    await parentForm.ExecuteAdbCommand(rebootCommand);

                    await Task.Delay(45000);

                }
                else
                {
                    MessageBox.Show(
                        "Failed to uninstall hykonMobile after 3 attempts.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unexpected error during patch:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                loadingForm.Close();

            }
        }

        async private void btnBootApp_Click(object sender, EventArgs e)
        {
            bool launchedAny = false;

            // Helper to try launching and flip the flag on success
            async Task TryLaunch(string adbCommand, string appLabel)
            {
                try
                {
                    var result = await parentForm.ExecuteAdbCommand(adbCommand);
                    // You could inspect `result` for more detailed success-check
                    launchedAny = true;
                }
                catch ( Exception ex )
                {
                    Console.WriteLine($"[{appLabel}] launch failed: {ex.Message}");

                }
            }

            // Try TP4
            await TryLaunch(
                "adb shell am start -n com.portworld.bootstartapp/com.portworld.bootstartapp.MainActivity",
                "TP4"
            );

            // Try TP5
            await TryLaunch(
                "adb shell am start -n com.adw.bootapp/com.adw.bootapp.MainActivity",
                "TP5"
            );

            // Final notification
            if (launchedAny)
            {
                MessageBox.Show(
                    "Launched Boot App",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "Unable to launch either Boot App (TP4 or TP5).",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

    }
}
