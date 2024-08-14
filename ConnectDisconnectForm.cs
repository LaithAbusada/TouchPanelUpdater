using System;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class ConnectDisconnectForm : Form
    {
        private Form1 parentForm;
        private bool isConnected;
        public event Action<bool> ConnectionStatusChanged;

        public ConnectDisconnectForm(Form1 parentForm, bool isConnected)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.isConnected = isConnected;
        }

        private void ConnectDisconnectForm_Load(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void UpdateFormState()
        {
            if (isConnected)
            {
                btnConnectDisconnect.Text = "Disconnect";
                txtIpAddress.Visible = false;
                txtPort.Visible = false;
                lblIpAddress.Visible = false;
                lblPort.Visible = false;
            }
            else
            {
                btnConnectDisconnect.Text = "Connect";
                txtIpAddress.Visible = true;
                txtPort.Visible = true;
                lblIpAddress.Visible = true;
                lblPort.Visible = true;
            }
        }

        private async void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                await parentForm.ExecuteAdbCommand("adb disconnect");
                MessageBox.Show("Disconnected successfully.");
                isConnected = false;
            }
            else
            {
                string ipAddress = txtIpAddress.Text;
                string port = txtPort.Text;

                if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(port))
                {
                    MessageBox.Show("Please enter a valid IP address and port.");
                    return;
                }

                string command = $"adb connect {ipAddress}:{port}";
                string result = await parentForm.ExecuteAdbCommand(command);
                MessageBox.Show(result);

                if (result.Contains("connected"))
                {
                    isConnected = true;
                }
            }
            ConnectionStatusChanged?.Invoke(isConnected);

            UpdateFormState();
        }
    }
}
