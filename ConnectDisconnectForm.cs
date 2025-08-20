using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;

namespace Innovo_TP4_Updater
{
    public partial class ConnectDisconnectForm : Form
    {
        private Form1 parentForm;
        private SettingsForm settingsForm;
        private bool isConnected;
        private int dealerID; // Store Dealer ID after login
        private List<Project> projectsList = new List<Project>(); // List to store project info

        public event Action<bool> ConnectionStatusChanged;

        public ConnectDisconnectForm(Form1 parentForm, bool isConnected, SettingsForm settingsForm, int dealerID)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.isConnected = isConnected;
            this.settingsForm = settingsForm;
            this.dealerID = dealerID; // DealerID is already passed to the form
        }

        private void UpdateFormState()
        {
            if (isConnected)
            {
                btnConnectDisconnect.Text = "Disconnect";
                txtIpAddress.Visible = false;
                lblIpAddress.Visible = false;
            }
            else
            {
                btnConnectDisconnect.Text = "Connect";
                txtIpAddress.Visible = true;
                lblIpAddress.Visible = true;
            }
        }

        private async void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            DisableControls();

            if (isConnected)
            {
                // Disconnect logic
                await DisconnectDevice();
                isConnected = false;

                settingsForm.UpdateConnectionStatusLabel("No Connected Device");
                ConnectionStatusChanged?.Invoke(isConnected);
                UpdateFormState();
            }
            else
            {
                string ipAddress = txtIpAddress.Text;

                if (string.IsNullOrEmpty(ipAddress))
                {
                    MessageBox.Show("Please enter a valid IP address.");
                    EnableControls();
                    return;
                }

                using (LoadingForm loadingForm = new LoadingForm("Connecting, please wait..."))
                {
                    loadingForm.Show();
                    loadingForm.BringToFront();

                    try
                    {
                        string command = $"adb connect {ipAddress}";
                        string result = await parentForm.ExecuteAdbCommand(command);

                        if (result.Contains("connected to") && !result.Contains("cannot connect to"))
                        {
                            string deviceModel = await GetDeviceModel();
                            string deviceName = await GetDeviceName();

                            if (string.IsNullOrEmpty(deviceModel) || string.IsNullOrEmpty(deviceName))
                            {
                                MessageBox.Show("Failed to retrieve device information. Please try again.");
                                EnableControls();
                                return;
                            }

                            var lowerCaseModel = deviceModel.ToLower();
                            if (!parentForm.supportedModels.Any(s => lowerCaseModel.Contains(s)))
                            {
                                MessageBox.Show($"Connected device is {deviceModel}, but only P4 or P5 devices are supported. Disconnecting...");
                                await DisconnectDevice();
                                isConnected = false;
                                settingsForm.UpdateConnectionStatusLabel("No Connected Device");
                                ConnectionStatusChanged?.Invoke(isConnected);
                            }
                            else
                            {
                                isConnected = true;
                                settingsForm.UpdateConnectionStatusLabel($"Connected to {ipAddress}");
                                ConnectionStatusChanged?.Invoke(isConnected);
                                MessageBox.Show("Connected successfully.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to connect. Please check the IP address and try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during connection: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            UpdateFormState();
            EnableControls();
        }

        private async Task FetchProjects()
        {
            
            var client = new RestClient("https://auth.innovo.net/GetProjects.php");
            var request = new RestRequest("", Method.Get);
            request.AddParameter("dealer_id", dealerID);



            RestResponse response = await client.ExecuteAsync(request);

            comboProjectName.Items.Clear(); // Clear any existing items
            projectsList.Clear(); // Clear existing project list
            comboPanelLocation.Items.Add("----Add New----");
            comboProjectName.Items.Add("---Add New----");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                if (jsonResponse.status == "success" && jsonResponse.count > 0)
                {

                    foreach (var project in jsonResponse.projects)
                    {
                        var newProject = new Project
                        {
                            ProjectId = project.id, // Assuming 'id' is the project ID field
                            ProjectName = project.name
                        };

                        projectsList.Add(newProject);
                        comboProjectName.Items.Add(newProject); // The ToString method will display the ProjectName
                    }
                }
                else
                {
                    MessageBox.Show("No projects found for this dealer.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show($"Error fetching projects: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



        }
        private List<ProjectPanel> panelsList = new List<ProjectPanel>(); // Store fetched panels

        private async Task FetchPanels(int projectId)
        {
            var client = new RestClient("https://auth.innovo.net/GetPanels.php");
            var request = new RestRequest("", Method.Get);
            request.AddParameter("project_id", projectId); // Use projectId to fetch relevant panels

            RestResponse response = await client.ExecuteAsync(request);

            comboPanelLocation.Items.Clear(); // Clear any existing items
            panelsList.Clear(); // Clear existing panels list

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                if (jsonResponse.status == "success" && jsonResponse.count > 0)
                {

                    foreach (var panel in jsonResponse.panels)
                    {
                        // Assuming the API provides 'id', 'name', 'ip', 'port', and 'android_version'
                        var newPanel = new ProjectPanel
                        {
                            PanelId = panel.id, // Assuming 'id' is the panel ID field
                            PanelName = panel.name, // Assuming 'name' is the panel name field
                            IPAddress = panel.ip, // Assuming 'ip' is the IP address field
                            Port = panel.port, // Assuming 'port' is the port field
                            AndroidVersion = panel.android_version, // Assuming 'android_version' is the Android version field
                            ProjectId = projectId // Store the project ID
                        };

                        panelsList.Add(newPanel);
                        comboPanelLocation.Items.Add(newPanel); // Add the panel to the dropdown
                    }
                }
                else
                {
                    MessageBox.Show("No panels found for this project.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show($"Error fetching panels: {response.Content} {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            comboPanelLocation.Items.Add("---Add New----");
        }



        private async Task DisconnectDevice()
        {
            await parentForm.ExecuteAdbCommand("adb disconnect");
        }

        private async Task<string> GetDeviceModel()
        {
            try
            {
                string modelCommand = "adb shell getprop ro.product.model";
                string modelOutput = await parentForm.ExecuteAdbCommand(modelCommand);
                return modelOutput.Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching device model: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private async Task<string> GetDeviceName()
        {
            try
            {
                string nameCommand = "adb shell getprop ro.product.name";
                string nameOutput = await parentForm.ExecuteAdbCommand(nameCommand);
                return nameOutput.Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching device name: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void DisableControls()
        {
            this.Enabled = false;
        }

        private void EnableControls()
        {
            this.Enabled = true;
        }

        private string PromptForNewProjectName()
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Enter New Project Name";
                inputForm.Size = new Size(300, 150);
                inputForm.StartPosition = FormStartPosition.CenterScreen;

                TextBox txtNewProjectName = new TextBox() { Left = 20, Top = 20, Width = 240 };
                Button btnOk = new Button() { Text = "OK", Left = 170, Top = 50, Width = 100 };
                btnOk.Click += (sender, e) => { inputForm.DialogResult = DialogResult.OK; inputForm.Close(); };

                inputForm.Controls.Add(txtNewProjectName);
                inputForm.Controls.Add(btnOk);

                return inputForm.ShowDialog() == DialogResult.OK ? txtNewProjectName.Text : string.Empty;
            }
        }

        private string PromptForNewPanelDetails(out string ipAddress, out string port)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Enter New Panel Details";
                inputForm.Size = new Size(400, 250); // Adjusted size to reflect removal of Android version
                inputForm.StartPosition = FormStartPosition.CenterScreen;

                // Panel name input
                Label lblPanelName = new Label() { Text = "Panel Name", Left = 20, Top = 20, Width = 100 };
                TextBox txtPanelName = new TextBox() { Left = 140, Top = 20, Width = 200 };

                // IP address input
                Label lblIp = new Label() { Text = "IP Address", Left = 20, Top = 60, Width = 100 };
                TextBox txtIp = new TextBox() { Left = 140, Top = 60, Width = 200 };

                // Port input with default value of 555
                Label lblPort = new Label() { Text = "Port", Left = 20, Top = 100, Width = 100 };
                TextBox txtPort = new TextBox() { Left = 140, Top = 100, Width = 200, Text = "5555" }; // Default value is set here

                // OK button
                Button btnOk = new Button() { Text = "OK", Left = 240, Top = 160, Width = 100 };
                btnOk.Click += (sender, e) => { inputForm.DialogResult = DialogResult.OK; inputForm.Close(); };

                // Add controls to the form
                inputForm.Controls.Add(lblPanelName);
                inputForm.Controls.Add(txtPanelName);
                inputForm.Controls.Add(lblIp);
                inputForm.Controls.Add(txtIp);
                inputForm.Controls.Add(lblPort);
                inputForm.Controls.Add(txtPort);
                inputForm.Controls.Add(btnOk);

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    ipAddress = txtIp.Text;
                    port = txtPort.Text; // Will default to "555" if unchanged
                    return txtPanelName.Text;
                }
                else
                {
                    ipAddress = string.Empty;
                    port = string.Empty;
                    return string.Empty;
                }
            }
        }


        private async void ConnectDisconnectForm_Load_1Async(object sender, EventArgs e)
        {

            await FetchProjects();

            UpdateFormState();
        }

        private async void comboProjectName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboProjectName.SelectedItem != null && comboProjectName.SelectedItem.ToString() == "---Add New----")
            {
                // Prompt the user for a new project name
                string newProjectName = PromptForNewProjectName();

                if (!string.IsNullOrEmpty(newProjectName))
                {
                    // Attempt to add the new project name via POST request
                    bool isProjectAdded = await AddNewProject(newProjectName);

                    if (isProjectAdded)
                    {
                        // Refetch the projects list after successfully adding the new project
                        await FetchProjects();

                        // Select the newly added project from the dropdown list
                        var addedProject = projectsList.FirstOrDefault(p => p.ProjectName == newProjectName);
                        if (addedProject != null)
                        {
                            comboProjectName.SelectedItem = addedProject;
                        }
                    }
                }
            }
            else if (comboProjectName.SelectedIndex != -1)
            {
                // Fetch panels for the selected project
                int projectId = GetSelectedProjectId(comboProjectName.SelectedItem.ToString());
                await FetchPanels(projectId); // Fetch panels

                comboPanelLocation.Enabled = true;
            }
            else
            {
                comboPanelLocation.Enabled = false;
            }
        }

        private async Task<bool> AddNewProject(string newProjectName)
        {
            string url = "https://auth.innovo.net/AddProject.php";
            var client = new RestClient(url);
            var request = new RestRequest("", Method.Post);

            // Add parameters using AddParameter method
            request.Resource = $"?dealer_id={dealerID}&name={newProjectName}";

            if (dealerID == -1)
            {
                MessageBox.Show("You can't use this feature. Please contact support.", "Feature Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse.status == "success")
                    {
                        MessageBox.Show($"Project '{newProjectName}' added successfully. {jsonResponse.msg}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to add project: {jsonResponse.msg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Error adding project: {response.StatusCode} - {response.Content}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during the request: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }
        private async Task<bool> AddNewPanel(int projectId, string panelName, string ipAddress, string port)
        {
            string androidVersion = "0"; // Android version is set to 0
            string url = "https://auth.innovo.net/AddPanel.php";
            var client = new RestClient(url);
            var request = new RestRequest("", Method.Post);

            // Add parameters using AddParameter method
            request.Resource = $"?project_id={projectId}&name={panelName}&ip={ipAddress}&port={port}&android_version={androidVersion}";

            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse.status == "success")
                    {
                        MessageBox.Show($"Panel '{panelName}' added successfully. {jsonResponse.msg}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refetch the panels after successfully adding a new panel
                        await FetchPanels(projectId);

                        // Find and select the newly added panel in the comboPanelLocation
                        var newlyAddedPanel = panelsList.FirstOrDefault(panel => panel.PanelName == panelName);
                        if (newlyAddedPanel != null)
                        {
                            comboPanelLocation.SelectedItem = newlyAddedPanel;
                            // Immediately populate the txtIpAddress with the new panel's IP and port
                            txtIpAddress.Text = $"{newlyAddedPanel.IPAddress}:{newlyAddedPanel.Port}";
                        }

                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to add panel: {jsonResponse.msg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Error adding panel: {response.StatusCode} - {response.Content}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during the request: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private async Task DeleteProject(int projectId)
        {
            string url = $"https://auth.innovo.net/DeleteProject.php?project_id={projectId}";
            var client = new RestClient(url);
            var request = new RestRequest("", Method.Delete); // Use GET request for deletion as per the API

            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse.status == "success")
                    {
                        MessageBox.Show($"Project deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refetch the projects after deletion
                        await FetchProjects();

                        // Clear the IP address field and the panels list since the project is deleted
                        txtIpAddress.Text = string.Empty;
                        comboPanelLocation.Items.Clear(); // Clear the panel dropdown list

                        // Optionally, you may want to disable the panel dropdown until a new project is selected
                        comboPanelLocation.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete project: {jsonResponse.msg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Error deleting project: {response.StatusCode} - {response.Content}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during the request: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async Task DeletePanel(int panelId)
        {
            string url = $"https://auth.innovo.net/DeletePanel.php?panel_id={panelId}";
            var client = new RestClient(url);
            var request = new RestRequest("", Method.Delete); // Use GET request for deletion as per the API

            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse.status == "success")
                    {
                        MessageBox.Show($"Panel deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtIpAddress.Text = string.Empty;

                        int projectId = GetSelectedProjectId(comboProjectName.SelectedItem.ToString());
                        await FetchPanels(projectId); // Refetch panels after deletion
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete panel: {jsonResponse.msg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Error deleting panel: {response.StatusCode} - {response.Content}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during the request: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnDeleteProject_Click(object sender, EventArgs e)
        {
            if (comboProjectName.SelectedItem == null || comboProjectName.SelectedItem.ToString() == "---Add New----")
            {
                MessageBox.Show("Please select a valid project to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedProject = comboProjectName.SelectedItem as Project;

            if (selectedProject != null)
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to delete the project '{selectedProject.ProjectName}'?",
                                                     "Confirm Delete",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    await DeleteProject(selectedProject.ProjectId);
                }
            }
        }

        private async void btnDeletePanel_Click(object sender, EventArgs e)
        {
            if (comboPanelLocation.SelectedItem == null || comboPanelLocation.SelectedItem.ToString() == "---Add New----")
            {
                MessageBox.Show("Please select a valid panel to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedPanel = comboPanelLocation.SelectedItem as ProjectPanel;

            if (selectedPanel != null)
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to delete the panel '{selectedPanel.PanelName}'?",
                                                     "Confirm Delete",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    await DeletePanel(selectedPanel.PanelId);
                }
            }
        }

        private async void comboPanelLocation_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboPanelLocation.SelectedItem != null && comboPanelLocation.SelectedItem.ToString() == "---Add New----")
            {
                string ipAddress, port;
                string newPanelName = PromptForNewPanelDetails(out ipAddress, out port);

                if (!string.IsNullOrEmpty(newPanelName) && !string.IsNullOrEmpty(ipAddress) && !string.IsNullOrEmpty(port))
                {
                    int projectId = GetSelectedProjectId(comboProjectName.SelectedItem.ToString());

                    bool isPanelAdded = await AddNewPanel(projectId, newPanelName, ipAddress, port);

                    if (isPanelAdded)
                    {
                        // No need to manually insert the new panel here since FetchPanels will refresh the list
                        comboPanelLocation.SelectedItem = newPanelName; // Set the newly added panel as the selected item
                    }
                }
            }
            else if (comboPanelLocation.SelectedIndex != -1)
            {
                // Retrieve the selected panel object
                var selectedPanel = comboPanelLocation.SelectedItem as ProjectPanel;
                if (selectedPanel != null)
                {
                    // Load the panel details into the corresponding text boxes
                    txtIpAddress.Text = selectedPanel.IPAddress + ":" + selectedPanel.Port;
                    txtIpAddress.ReadOnly = true;

                }
            }
            else
            {
                txtIpAddress.ReadOnly = false;

            }
        }

        private int GetSelectedProjectId(string projectName)
        {
            var selectedProject = projectsList.FirstOrDefault(p => p.ProjectName == projectName);
            return selectedProject != null ? selectedProject.ProjectId : -1;
        }
        private async void btnEditProject_Click(object sender, EventArgs e)
        {
            if (comboProjectName.SelectedItem == null || comboProjectName.SelectedItem.ToString() == "---Add New----")
            {
                MessageBox.Show("Please select a valid project to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedProject = comboProjectName.SelectedItem as Project;

            if (selectedProject != null)
            {
                string newProjectName = PromptForEditProjectName(selectedProject.ProjectName);

                if (!string.IsNullOrEmpty(newProjectName) && newProjectName != selectedProject.ProjectName)
                {
                    bool isProjectEdited = await EditProject(selectedProject.ProjectId, newProjectName);

                    if (isProjectEdited)
                    {
                        await FetchProjects(); // Refresh projects list after successful edit
                        comboProjectName.SelectedItem = projectsList.FirstOrDefault(p => p.ProjectName == newProjectName); // Select updated project
                    }
                }
            }
        }

        private async void btnEditPanel_Click(object sender, EventArgs e)
        {
            if (comboPanelLocation.SelectedItem == null || comboPanelLocation.SelectedItem.ToString() == "---Add New----")
            {
                MessageBox.Show("Please select a valid panel to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedPanel = comboPanelLocation.SelectedItem as ProjectPanel;

            if (selectedPanel != null)
            {
                string newPanelName, newIpAddress, newPort;
                string editedPanelName = PromptForEditPanelDetails(selectedPanel.PanelName, selectedPanel.IPAddress, selectedPanel.Port, out newIpAddress, out newPort);

                if (!string.IsNullOrEmpty(editedPanelName) && !string.IsNullOrEmpty(newIpAddress) && !string.IsNullOrEmpty(newPort))
                {
                    bool isPanelEdited = await EditPanel(selectedPanel.PanelId, editedPanelName, newIpAddress, newPort);

                    if (isPanelEdited)
                    {
                        int projectId = selectedPanel.ProjectId;
                        await FetchPanels(projectId); // Refresh panels list after successful edit
                        comboPanelLocation.SelectedItem = panelsList.FirstOrDefault(p => p.PanelName == editedPanelName); // Select updated panel
                    }
                }
            }
        }

        private string PromptForEditProjectName(string currentProjectName)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Edit Project Name";
                inputForm.Size = new Size(300, 150);
                inputForm.StartPosition = FormStartPosition.CenterScreen;

                TextBox txtProjectName = new TextBox() { Left = 20, Top = 20, Width = 240 };
                txtProjectName.Text = currentProjectName;

                Button btnOk = new Button() { Text = "OK", Left = 170, Top = 50, Width = 100 };
                btnOk.Click += (sender, e) => { inputForm.DialogResult = DialogResult.OK; inputForm.Close(); };

                inputForm.Controls.Add(txtProjectName);
                inputForm.Controls.Add(btnOk);

                return inputForm.ShowDialog() == DialogResult.OK ? txtProjectName.Text : string.Empty;
            }
        }

        private string PromptForEditPanelDetails(string currentPanelName, string currentIp, string currentPort, out string ipAddress, out string port)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Edit Panel Details";
                inputForm.Size = new Size(400, 250); // Adjusted size to reflect removal of Android version
                inputForm.StartPosition = FormStartPosition.CenterScreen;

                // Panel name input
                Label lblPanelName = new Label() { Text = "Panel Name", Left = 20, Top = 20, Width = 100 };
                TextBox txtPanelName = new TextBox() { Left = 140, Top = 20, Width = 200 };
                txtPanelName.Text = currentPanelName;

                // IP address input
                Label lblIp = new Label() { Text = "IP Address", Left = 20, Top = 60, Width = 100 };
                TextBox txtIp = new TextBox() { Left = 140, Top = 60, Width = 200 };
                txtIp.Text = currentIp;

                // Port input
                Label lblPort = new Label() { Text = "Port", Left = 20, Top = 100, Width = 100 };
                TextBox txtPort = new TextBox() { Left = 140, Top = 100, Width = 200 };
                txtPort.Text = currentPort;

                // OK button
                Button btnOk = new Button() { Text = "OK", Left = 240, Top = 160, Width = 100 };
                btnOk.Click += (sender, e) => { inputForm.DialogResult = DialogResult.OK; inputForm.Close(); };

                // Add controls to the form
                inputForm.Controls.Add(lblPanelName);
                inputForm.Controls.Add(txtPanelName);
                inputForm.Controls.Add(lblIp);
                inputForm.Controls.Add(txtIp);
                inputForm.Controls.Add(lblPort);
                inputForm.Controls.Add(txtPort);
                inputForm.Controls.Add(btnOk);

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    ipAddress = txtIp.Text;
                    port = txtPort.Text;
                    return txtPanelName.Text;
                }
                else
                {
                    ipAddress = string.Empty;
                    port = string.Empty;
                    return string.Empty;
                }
            }
        }

        private async Task<bool> EditProject(int projectId, string newProjectName)
        {
            string url = $"https://auth.innovo.net/EditProject.php?project_id={projectId}&name={newProjectName}";
            var client = new RestClient(url);
            var request = new RestRequest("", Method.Post);

            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse.status == "success")
                    {
                        MessageBox.Show($"Project edited successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to edit project: {jsonResponse.msg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Error editing project: {response.StatusCode} - {response.Content}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during the request: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private async Task<bool> EditPanel(int panelId, string newPanelName, string newIpAddress, string newPort)
        {
            string androidVersion = "0"; // Set Android version to 0
            string url = $"https://auth.innovo.net/EditPanel.php?panel_id={panelId}&ip={newIpAddress}&port={newPort}&name={newPanelName}&android_version={androidVersion}";
            var client = new RestClient(url);
            var request = new RestRequest("", Method.Post);

            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse.status == "success")
                    {
                        MessageBox.Show($"Panel edited successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to edit panel: {jsonResponse.msg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Error editing panel: {response.StatusCode} - {response.Content}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during the request: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }
    }

        public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public override string ToString()
        {
            return ProjectName;
        }
    }


    public class DeviceInfo
    {
        public string IPAddress { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string ConnectionStatus { get; set; } = "Not Connected";

        public override string ToString()
        {
            return $"{Name} - {Model} ({IPAddress})";
        }
    }
}
public class ProjectPanel
{
    public int PanelId { get; set; }
    public string PanelName { get; set; }
    public string IPAddress { get; set; }
    public string Port { get; set; }
    public string AndroidVersion { get; set; }
    public int ProjectId { get; set; }  // Add ProjectId to store the project this panel belongs to

    public override string ToString()
    {
        return PanelName; // This will be displayed in the dropdown
    }
}