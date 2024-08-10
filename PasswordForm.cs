using System;
using System.Windows.Forms;
using RestSharp; // Ensure you have installed RestSharp via NuGet

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
            // Any initialization if needed
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (AuthenticateUser(username, password))
            {
                this.Hide();
                // Password is correct, close the PasswordForm and open the main form of your application
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

            var request = new RestRequest("" , Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Here you should handle the response, e.g., extracting the token if needed
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
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

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://innovo.net/my-account");
        }
    }
}
