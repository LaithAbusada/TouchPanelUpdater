using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using TimeZoneConverter; // Add this namespace

namespace Innovo_TP4_Updater
{
    public partial class TimeZoneForm : Form
    {
        private Form1 parentForm;
        private SettingsForm settingsForm;
        public TimeZoneForm(Form1 parent, SettingsForm settingsForm)
        {
            InitializeComponent();
            parentForm = parent;
            this.settingsForm = settingsForm;
        }

        private async void TimeZoneForm_Load(object sender, EventArgs e)
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

            // Populate the timezone ComboBox
            PopulateTimeZoneComboBox();

            try
            {
                // Retrieve the current timezone from the connected device
                string deviceTimeZone = (await parentForm.ExecuteAdbCommand("adb shell getprop persist.sys.timezone")).Trim();

                if (!string.IsNullOrEmpty(deviceTimeZone))
                {
                    if (timeZoneComboBox.Items.Cast<string>().Contains(deviceTimeZone))
                    {
                        timeZoneComboBox.SelectedItem = deviceTimeZone;
                    }
                    else
                    {
                        MessageBox.Show("The device's timezone could not be found in the list. Please select manually.",
                                        "Timezone Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Could not retrieve the device's timezone. Please select manually.",
                                    "Timezone Retrieval Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving the device's timezone: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PopulateTimeZoneComboBox()
        {
            // Populate ComboBox with IANA timezones
            foreach (TimeZoneInfo tz in TimeZoneInfo.GetSystemTimeZones())
            {
                string ianaTimeZone = TZConvert.WindowsToIana(tz.Id);
                if (!string.IsNullOrEmpty(ianaTimeZone) && !timeZoneComboBox.Items.Contains(ianaTimeZone))
                {
                    timeZoneComboBox.Items.Add(ianaTimeZone);
                }
            }

            // Set the default selected timezone
            string defaultIanaTimezone = TZConvert.WindowsToIana(TimeZoneInfo.Local.Id);
            if (!string.IsNullOrEmpty(defaultIanaTimezone))
            {
                timeZoneComboBox.SelectedItem = defaultIanaTimezone;
            }
        }

        private async void timeZoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!await parentForm.IsConnected())
            {
                MessageBox.Show("Device should be connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string selectedIanaTimeZone = timeZoneComboBox.SelectedItem.ToString();

                if (!string.IsNullOrEmpty(selectedIanaTimeZone))
                {
                    // Set the timezone using ADB
                    await parentForm.ExecuteAdbCommand($"adb shell setprop persist.sys.timezone \"{selectedIanaTimeZone}\"");

                    // Apply the timezone immediately
                    await parentForm.ExecuteAdbCommand($"adb shell service call alarm 4 s16 \"{selectedIanaTimeZone}\"");

                    // Show success message
                    lblSuccessMessage.Text = "Timezone set successfully and applied immediately!";
                    lblSuccessMessage.Visible = true;
                }
                else
                {
                    MessageBox.Show($"Unable to set the timezone.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblSuccessMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting timezone: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSuccessMessage.Visible = false;
            }
        }

        private void linkLabelTimeZoneInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open the default web browser to a page where the user can find their timezone
            Process.Start(new ProcessStartInfo("https://en.wikipedia.org/wiki/List_of_tz_database_time_zones") { UseShellExecute = true });
        }
    }
}
