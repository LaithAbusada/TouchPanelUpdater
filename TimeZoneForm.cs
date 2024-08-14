using System;
using System.Diagnostics;
using System.Windows.Forms;
using TimeZoneConverter; // Add this namespace

namespace Innovo_TP4_Updater
{
    public partial class TimeZoneForm : Form
    {
        private Form1 parentForm;

        public TimeZoneForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void TimeZoneForm_Load(object sender, EventArgs e)
        {
            PopulateTimeZoneComboBox();
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
                MessageBox.Show(selectedIanaTimeZone);

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
