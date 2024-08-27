using System;
using System.Windows.Forms;
using RestSharp; // Ensure you have installed RestSharp via NuGet
using Newtonsoft.Json;

namespace Innovo_TP4_Updater
{
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
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
    }
}
