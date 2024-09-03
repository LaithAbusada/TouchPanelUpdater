using System;
using System.Windows.Forms;
using RestSharp; // Ensure you have installed RestSharp via NuGet
using Newtonsoft.Json;
using System.Configuration;

namespace Innovo_TP4_Updater
{
    public partial class PasswordForm : Form
    {
        private readonly DateTime expiryDate;

        public PasswordForm()
        {
            InitializeComponent();

            // Load the expiry date from app.config
            string expiryDateString = ConfigurationManager.AppSettings["ExpiryDate"];
            if (DateTime.TryParse(expiryDateString, out DateTime parsedExpiryDate))
            {
                expiryDate = parsedExpiryDate;
            }
            else
            {
                // If parsing fails, set a default expiry date or handle the error as needed
                expiryDate = DateTime.MaxValue; // Default to a far future date
            }
        }
        private void PasswordForm_Load(object sender, EventArgs e)
        {
            // Check for expiry
            if (IsExpired())
            {
                MessageBox.Show("This application has expired. Please contact support.", "Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            // Load saved credentials if "Remember Me" was previously checked
            if (Properties.Settings.Default.RememberMe)
            {
                txtUsername.Text = Properties.Settings.Default.Username;
                txtPassword.Text = Properties.Settings.Default.Password;
                chkRememberMe.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (AuthenticateUser(username, password))
            {
                SaveCredentials(username, password); // Save credentials if login is successful
                this.Hide();
                Form1 mainForm = new Form1();
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                // Display an error message for incorrect credentials
                MessageBox.Show("Incorrect username or password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            var client = new RestClient("https://innovo.net/wp-json/api/v1/token");

            var request = new RestRequest("", Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Handle the response, e.g., extracting the token if needed
                dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                string token = jsonResponse.jwt_token;

                // Store the token if needed for further API calls
                // ...

                return true;
            }
            else
            {
                // Handle error response
                return false;
            }
        }

        private void SaveCredentials(string username, string password)
        {
            if (chkRememberMe.Checked)
            {
                Properties.Settings.Default.Username = username;
                Properties.Settings.Default.Password = password;
                Properties.Settings.Default.RememberMe = true;
            }
            else
            {
                Properties.Settings.Default.Username = string.Empty;
                Properties.Settings.Default.Password = string.Empty;
                Properties.Settings.Default.RememberMe = false;
            }
            Properties.Settings.Default.Save();
        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://innovo.net/my-account");
        }

        private bool IsExpired()
        {
            // Get the current time from the system
            DateTime currentDate = DateTime.Now;
            MessageBox.Show("curr" + currentDate.ToString() + "expirty" + expiryDate.ToString());

            // Optional: Fetch server time to avoid time tampering
            DateTime? serverDate = GetServerTime();
            if (serverDate.HasValue)
            {
                currentDate = serverDate.Value;
            }
            MessageBox.Show("curr" + currentDate.ToString() + "expirty" + expiryDate.ToString());

            return currentDate > expiryDate;
        }

        private DateTime? GetServerTime()
        {
            try
            {
                string serverTimeApiUrl = ConfigurationManager.AppSettings["ServerTimeApiUrl"];

                var client = new RestClient(serverTimeApiUrl); // Use the URL from app.config
                var request = new RestRequest("", Method.Get);
                RestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                    string utcDateTimeString = jsonResponse.datetime;
                    DateTime utcDateTime = DateTime.Parse(utcDateTimeString).ToUniversalTime();
                    MessageBox.Show(utcDateTime.ToString());
                    return utcDateTime;
                }
            }
            catch
            {
                // Handle exceptions or fallback to local time if needed
            }

            return null; // Return null if unable to get server time
        }
    }
}
