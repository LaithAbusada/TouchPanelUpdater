using System;
using System.Windows.Forms;
using RestSharp; // Ensure you have installed RestSharp via NuGet
using Newtonsoft.Json;
using System.Configuration;
using System.Linq;
using System.Drawing;

namespace Innovo_TP4_Updater
{
    public partial class PasswordForm : Form
    {
        private readonly DateTime expiryDate;
        private int dealerId; // Store the dealer ID
        private string link;
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
            updateAppLabel.Visible = false;

            // Check for expiry
            if (IsExpired())
            {
                MessageBox.Show("This application has expired. Please contact support.", "Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            if (!CheckForUpdates())
            {
                updateAppLabel.Visible = true;
                updateAppLabel.Text = "An update is available! Please uninstall the current version, then click here to download the latest version.";
                updateAppLabel.Font = new Font(updateAppLabel.Font, FontStyle.Bold);
                updateAppLabel.LinkColor = Color.DarkBlue;
                updateAppLabel.ActiveLinkColor = Color.DarkRed;
                updateAppLabel.VisitedLinkColor = Color.Purple;
                updateAppLabel.LinkClicked += (s, ev) => System.Diagnostics.Process.Start(link);

                // Disable form fields and prevent user actions
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
                button1.Enabled = false;
                chkRememberMe.Enabled = false;
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

                // Fetch dealer ID
                if (FetchDealerId(username))
                {
                    // Pass dealerId to Form1
                    this.Hide();
                    Form1 mainForm = new Form1(dealerId); // Pass the dealerId to Form1
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    this.Hide();
                    Form1 mainForm = new Form1(-1); // Pass the dealerId to Form1
                    mainForm.ShowDialog();
                    this.Close();
                }
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

            var request = new RestRequest()
            {
                RequestFormat = DataFormat.Json,
                Method = Method.Post
            };
            request.AddHeader("Content-Type", "application/json");

            // Set the JSON body with username and password
            request.AddJsonBody(new
            {
                username = username,
                password = password
            });



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

        private bool FetchDealerId(string dealerUsername)
        {
            var client = new RestClient("https://showroom.innovo.net/LoginDealer.php");

            var request = new RestRequest("", Method.Get);
            request.AddParameter("dealer_username", dealerUsername);

            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                if (jsonResponse.status == "success")
                {
                    dealerId = jsonResponse.dealer_id;
                    return true;
                }
            }

            return false; // Return false if unable to get dealer ID
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
        private bool CheckForUpdates()
        {
            try
            {
                var client = new RestClient("https://innovo.net/repo/TP4/app.json");
                var request = new RestRequest("", Method.Get);
                RestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                    string latestVersionString = jsonResponse.Innovo.version;
                    string savedVersionString = Properties.Settings.Default.AppVersion;

                    // Parse versions using the Version class
                    Version latestVersion = new Version(latestVersionString);
                    Version savedVersion = string.IsNullOrEmpty(savedVersionString) ? null : new Version(savedVersionString);


                    // If no saved version, initialize it with the latest version
                    if (savedVersion == null)
                    {
                        Properties.Settings.Default.AppVersion = latestVersionString;
                        Properties.Settings.Default.Save();
                    }
                    else if (savedVersion.CompareTo(latestVersion) < 0)
                    {
                        // Inform user to update if saved version is less than the latest version
                        link = jsonResponse.Innovo.filepath;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to check for updates. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return true;
        }


        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://innovo.net/my-account");
        }

        private bool IsExpired()
        {
            // Get the current time from the system
            DateTime currentDate = DateTime.Now;

            // Optional: Fetch server time to avoid time tampering
            DateTime? serverDate = GetServerTime();
            if (serverDate.HasValue)
            {
                currentDate = serverDate.Value;
            }

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
