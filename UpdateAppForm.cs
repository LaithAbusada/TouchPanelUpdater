using MaterialSkin.Controls;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Drawing;
using Innovo_TP4_Updater.Properties;
using System.Collections.Generic;

namespace Innovo_TP4_Updater
{
	public partial class UpdateAppForm : Form
	{
		private readonly Form1 parentForm;
		private readonly SettingsForm settingsForm;
		private readonly Dictionary<string, Button> _updateButtons = new Dictionary<string, Button>();
		private readonly Dictionary<string, Label> _statusLabels = new Dictionary<string, Label>();
		public UpdateAppForm(Form1 parent, SettingsForm sets)
		{
			InitializeComponent();
			parentForm = parent;
			settingsForm = sets;
		}

		private async void UpdateAppForm_Load(object sender, EventArgs e)
		{
			try
			{
				bool isConnected = await parentForm.IsConnected();

				if (!isConnected)
				{
					parentForm.clearMainPanel();
					materialMultiLineTextBox3.AppendText("No device is currently connected. Please connect a device before proceeding.\n");
					MessageBox.Show("No device is currently connected. Please connect a device before proceeding.",
								   "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				string jsonUrl = "https://innovo.net/repo/TP4/update_apps.json";
				string jsonString;

				materialMultiLineTextBox3.AppendText($"[LOG] Fetching JSON from: {jsonUrl}\n");

				using (HttpClient client = new HttpClient())
				{
					jsonString = await client.GetStringAsync(jsonUrl);
				}

				materialMultiLineTextBox3.AppendText($"[LOG] JSON fetched successfully. Length: {jsonString.Length} chars\n");

				JObject jsonData = JObject.Parse(jsonString);
				// Clear any existing controls
				appsPanel.Controls.Clear();
				_updateButtons.Clear();
				_statusLabels.Clear();
				// Loop through each app in the JSON
				foreach (var prop in jsonData.Properties())
				{
					string appName = prop.Name;
					var appInfo = (JObject)prop.Value;
					string latestVersion = appInfo["version"].ToString();
					string imageFileName = appInfo["image"].ToString(); // e.g. "Nice.jpeg"
					string packageName = appInfo["packageName"].ToString();
					int appCount = jsonData.Properties().Count();
					bool useLargeTiles = appCount <= 4;
					int buttonWidth = useLargeTiles ? 267 : 200;
					int buttonHeight = useLargeTiles ? 142 : 100;

					materialMultiLineTextBox3.AppendText($"[LOG] Loading app: {appName}, version: {latestVersion}, package: {packageName}\n");

					var statusLabel = new Label
					{
						AutoSize = false,                         // ⇐ turn off autosizing
						Size = new Size(buttonWidth, 18),      // fix it to buttonWidth × 18px tall (adjust height as needed)
						Font = new Font("Segoe UI", 8F),
						ForeColor = SystemColors.Highlight,
						Margin = new Padding(2, 2, 2, 2),
						TextAlign = ContentAlignment.MiddleCenter,  // center the text horizontally (optional)
						Text = string.Empty
					};

					Image appIcon = await DownloadImageAsync(imageFileName);


					var btn = new Button
					{
						// keep the core settings
						Text = $"Update {appName}",
						Enabled = false,
						Tag = appName,
						Margin = new Padding(2),
						// fixed size matching your designer
						Size = new Size(buttonWidth, buttonHeight),   // ↓ shrink from 267×142 → 200×100

						// typography & colors
						Font = new Font("Segoe UI", 8F, FontStyle.Bold),
						ForeColor = Color.White,
						BackColor = SystemColors.Highlight,

						// image background
						BackgroundImage = appIcon,
						BackgroundImageLayout = ImageLayout.Stretch,

						// text positioning
						TextAlign = ContentAlignment.BottomCenter,
						TextImageRelation = TextImageRelation.ImageAboveText,

						// if you prefer the MaterialSkin look, swap Button → MaterialRaisedButton & drop BackColor/ForeColor
					};

					// Wire up click to your existing UpdateApp method
					btn.Click += async (s, ev) => {
						await UpdateApp(appName, packageName, btn);
					};

					appsPanel.Padding = new Padding(0);


					// Add both to a panel (or directly to FlowLayoutPanel)
					var appContainer = new FlowLayoutPanel
					{
						FlowDirection = FlowDirection.TopDown,
						AutoSize = true,
						WrapContents = false,
						Margin = new Padding(5)
					};
					appContainer.Controls.Add(btn);
					appContainer.Controls.Add(statusLabel);

					appsPanel.Controls.Add(appContainer);

					_updateButtons[appName] = btn;
					_statusLabels[appName] = statusLabel;

					// Kick off the version check for this app
					_ = CheckAndDisplayVersionStatus(appName, packageName, jsonData, btn, statusLabel);
				}
			}
			catch (Exception ex)
			{
				materialMultiLineTextBox3.AppendText($"Error: Unable to connect to the device. Details: {ex.Message}\n");
				materialMultiLineTextBox3.AppendText($"[LOG] Load Exception Type: {ex.GetType().FullName}\n");
				if (ex.InnerException != null)
					materialMultiLineTextBox3.AppendText($"[LOG] Inner Exception: {ex.InnerException.Message}\n");
			}
		}
		private async Task CheckAndDisplayVersionStatus(string appName, string packageName, JObject jsonData, Button updateButton, Label statusLabel)
		{
			try
			{
				string currentVersion = await GetCurrentVersion(packageName);
				string latestVersion = jsonData[appName]["version"].ToString();

				materialMultiLineTextBox3.AppendText($"[LOG] Version check for {appName}: installed={currentVersion ?? "null"}, latest={latestVersion}\n");

				// If GetCurrentVersion returned null/empty, the app isn't installed:
				if (string.IsNullOrEmpty(currentVersion))
				{
					statusLabel.Text = "App not Installed";
					updateButton.Enabled = true;   // allow the user to install it
					return;
				}

				// Parse versions to enable comparison
				Version localVersion = new Version(currentVersion);
				Version jsonVersion = new Version(latestVersion);

				materialMultiLineTextBox3.AppendText($"[LOG] Parsed versions for {appName}: local={localVersion}, json={jsonVersion}, localVersion >= jsonVersion = {localVersion >= jsonVersion}\n");

				// Compare versions: update needed if localVersion is less than jsonVersion
				if (localVersion >= jsonVersion)
				{
					statusLabel.Text = $"Up to date (v{currentVersion})";
					updateButton.Enabled = false;
				}
				else
				{
					statusLabel.Text = $"Update available: (v{latestVersion})";
					updateButton.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				materialMultiLineTextBox3.AppendText($"[LOG] Version check exception for {appName}: {ex.GetType().FullName}: {ex.Message}\n");
				statusLabel.Text = "App not Installed";
				updateButton.Enabled = true;

			}
		}


		private async

		Task
UpdateApp(string appName, string packageName, Button clickedButton)
		{
			int maxAttempts = 3;
			int attempt = 0;
			bool success = false;
			materialMultiLineTextBox3.Clear();
			settingsForm.DisableAllButtons();
			LoadingForm loadingForm = new LoadingForm("Checking for updates...");
			materialMultiLineTextBox3.AppendText("Checking for Update" + Environment.NewLine);
			loadingForm.Show();
			loadingForm.BringToFront();

			string downloadDirectory = string.Empty;

			while (attempt < maxAttempts && !success)
			{
				settingsForm.DisableAllButtons();
				attempt++;
				materialMultiLineTextBox3.AppendText($"[LOG] ===== Attempt {attempt} of {maxAttempts} for {appName} =====\n");
				try
				{
					loadingForm.UpdateMessage("Checking for connected devices...");
					string connectedDevices = await parentForm.ExecuteAdbCommand("adb devices -l");
					materialMultiLineTextBox3.AppendText($"[LOG] adb devices output: {connectedDevices}\n");
					if (!connectedDevices.Contains("device"))
					{
						materialMultiLineTextBox3.AppendText("No connected device detected. Please connect a device and try again.\n");
						return;
					}

					loadingForm.UpdateMessage("Retrieving device model...");
					string deviceModel = await GetDeviceModel();
					materialMultiLineTextBox3.AppendText($"[LOG] Device model: {deviceModel}\n");
					string lowerCaseModel = deviceModel.ToLower();
					if (!parentForm.supportedModels.Any(s => lowerCaseModel.Contains(s)))
					{
						materialMultiLineTextBox3.AppendText($"Connected device is {deviceModel}, but only P4 or P5 devices are supported for updates.\n");
						return;
					}

					loadingForm.UpdateMessage("Retrieving screen size...");
					string screenSize = await GetScreenSize();
					materialMultiLineTextBox3.AppendText($"[LOG] Screen size: {screenSize}\n");
					if (screenSize == "480x480")
					{
						materialMultiLineTextBox3.AppendText("Adjusting screen resolution from 480x480 to 479x480...\n");
						await AdjustScreenResolution("479x480");
					}
					if (appName == "Control4" && (deviceModel.ToLower().Contains("p4") || deviceModel.ToLower() == "rk3566_t"))
					{
						await parentForm.ExecuteAdbCommand("adb shell wm size 720x720");
					}
					else if (deviceModel.ToLower().Contains("p5")) { }

					DisableOtherButtons(clickedButton);

					loadingForm.UpdateMessage("Checking for available updates...");
					string jsonUrl = "https://innovo.net/repo/TP4/update_apps.json";
					string jsonString;

					materialMultiLineTextBox3.AppendText($"[LOG] Fetching JSON from: {jsonUrl}\n");

					using (HttpClient client = new HttpClient())
					{
						jsonString = await client.GetStringAsync(jsonUrl);
					}

					materialMultiLineTextBox3.AppendText($"[LOG] JSON fetched. Length: {jsonString.Length}\n");

					JObject jsonData = JObject.Parse(jsonString);
					string currentVersion = await GetCurrentVersion(packageName);
					string latestVersion = jsonData[appName]["version"].ToString();

					materialMultiLineTextBox3.AppendText($"[LOG] Current version: {currentVersion ?? "null"}, Latest version: {latestVersion}\n");

					if (string.IsNullOrEmpty(currentVersion))
					{
						materialMultiLineTextBox3.AppendText($"{appName} is not installed. Proceeding to install v{latestVersion}...\n");
						//    // (Don't return—let the code fall through to download/install below.)
					}
					else
					{
						// Now it's safe to parse both versions
						Version localVersion = new Version(currentVersion);
						Version jsonVersion = new Version(latestVersion);

						materialMultiLineTextBox3.AppendText($"[LOG] Parsed: localVersion={localVersion}, jsonVersion={jsonVersion}\n");

						if (localVersion >= jsonVersion)
						{
							materialMultiLineTextBox3.AppendText($"{appName} is already up to date. Version: {latestVersion}\n");
							return;
						}

						materialMultiLineTextBox3.AppendText(
							$"Update available: v{currentVersion} → v{latestVersion}\n");
					}

					string baseDirectory = Path.Combine(Application.StartupPath, "Downloads");  // Using relative path
					downloadDirectory = Path.Combine(baseDirectory, jsonData[appName]["filename"].ToString());

					string downloadFolder = Path.GetDirectoryName(downloadDirectory);
					if (!Directory.Exists(downloadFolder))
					{
						Directory.CreateDirectory(downloadFolder);
					}

					if (File.Exists(downloadDirectory))
					{
						File.Delete(downloadDirectory);
					}

					string extractPath = Path.Combine(Path.GetDirectoryName(downloadDirectory), Path.GetFileNameWithoutExtension(downloadDirectory));
					if (Directory.Exists(extractPath))
					{
						Directory.Delete(extractPath, true);
					}

					materialMultiLineTextBox3.AppendText("Installing Update for " + appName + " this can take up to 2 minutes" + Environment.NewLine);

					loadingForm.UpdateMessage("Downloading the update...");
					string downloadUrl = $"https://innovo.net/repo/TP4/Apks/{jsonData[appName]["filename"]}";

					materialMultiLineTextBox3.AppendText($"[LOG] Download URL: {downloadUrl}\n");
					materialMultiLineTextBox3.AppendText($"[LOG] Save path: {downloadDirectory}\n");
					materialMultiLineTextBox3.AppendText($"[LOG] Filename from JSON: {jsonData[appName]["filename"]}\n");
					materialMultiLineTextBox3.AppendText($"[LOG] Download starting...\n");

					using (WebClient client = new WebClient())
					{
						await client.DownloadFileTaskAsync(new Uri(downloadUrl), downloadDirectory);
					}

					materialMultiLineTextBox3.AppendText($"[LOG] Download finished.\n");
					materialMultiLineTextBox3.AppendText($"[LOG] File exists after download: {File.Exists(downloadDirectory)}\n");
					if (File.Exists(downloadDirectory))
						materialMultiLineTextBox3.AppendText($"[LOG] Downloaded file size: {new FileInfo(downloadDirectory).Length} bytes\n");

					// Check if the downloaded file exists before proceeding
					if (!File.Exists(downloadDirectory))
					{
						materialMultiLineTextBox3.AppendText($"Failed to download the update file for {appName}. Please check the internet connection and try again.\n");
						return;
					}

					loadingForm.UpdateMessage("Installing the update...");
					string fileType = jsonData[appName]["type"].ToString();
					materialMultiLineTextBox3.AppendText($"[LOG] File type: {fileType}\n");

					if (fileType == "file")
					{
						materialMultiLineTextBox3.AppendText($"[LOG] Starting InstallApk...\n");
						await InstallApk(downloadDirectory);
						materialMultiLineTextBox3.AppendText($"[LOG] InstallApk completed.\n");

						if (File.Exists(downloadDirectory))
						{
							File.Delete(downloadDirectory);
						}
					}
					else if (fileType == "zip")
					{
						materialMultiLineTextBox3.AppendText($"[LOG] Starting UnzipAndInstall...\n");
						await UnzipAndInstall(downloadDirectory);
						materialMultiLineTextBox3.AppendText($"[LOG] UnzipAndInstall completed.\n");
					}

					loadingForm.UpdateMessage("Rebooting the device...");
					materialMultiLineTextBox3.AppendText($"[LOG] Checking version after install...\n");
					string postInstallVersion = await GetCurrentVersion(packageName);
					materialMultiLineTextBox3.AppendText($"[LOG] Post-install version: {postInstallVersion ?? "null"}, expected: {latestVersion}\n");

					bool versionCompare = await RebootApp(appName, packageName, latestVersion);
					materialMultiLineTextBox3.AppendText($"[LOG] RebootApp returned: {versionCompare}\n");
					success = versionCompare;

					if (success)
					{
						UpdateStatusLabel(appName, clickedButton);
						materialMultiLineTextBox3.AppendText($"Successfully Updated {appName} to {latestVersion}");
					}
				}
				catch (Exception ex)
				{
					materialMultiLineTextBox3.AppendText($"Attempt {attempt} failed: {ex.Message}\n");
					materialMultiLineTextBox3.AppendText($"[LOG] Exception Type: {ex.GetType().FullName}\n");
					if (ex.InnerException != null)
						materialMultiLineTextBox3.AppendText($"[LOG] Inner Exception: {ex.InnerException.GetType().FullName}: {ex.InnerException.Message}\n");
					materialMultiLineTextBox3.AppendText($"[LOG] Stack Trace: {ex.StackTrace}\n");

					if (attempt == maxAttempts)
					{
						MessageBox.Show($"Failed to install {appName} after {maxAttempts} attempts. Please contact support.", "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				finally
				{
					CleanUp(downloadDirectory);

					loadingForm.Close();
					EnableAllButtons();
					settingsForm.EnableAllButtons();
				}
			}
			if (!success)
			{
				MessageBox.Show($"Failed to install {appName} after {maxAttempts} attempts. Please contact support.", "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async Task<string> GetScreenSize()
		{
			string screenSizeCommand = "adb shell wm size";
			string screenSizeOutput = await parentForm.ExecuteAdbCommand(screenSizeCommand);
			return screenSizeOutput.Split(':')[1].Trim();
		}

		private async Task AdjustScreenResolution(string resolution)
		{
			string adjustResolutionCommand = $"adb shell wm size {resolution}";
			await parentForm.ExecuteAdbCommand(adjustResolutionCommand);
		}

		private async Task<Image> DownloadImageAsync(string imageFileName)
		{
			try
			{
				// Build the full URL
				string imageUrl = $"https://innovo.net/repo/TP4/images/{imageFileName}";

				using (var client = new HttpClient())
				{
					byte[] data = await client.GetByteArrayAsync(imageUrl);

					using (var ms = new MemoryStream(data))
					{
						return Image.FromStream(ms);
					}
				}
			}
			catch
			{
				// OPTIONAL: return a fallback if the download fails
				// e.g. a single "not found" icon embedded in your Resources
				return Resources.Nice;
			}
		}



		private async Task UnzipAndInstall(string zipFilePath)
		{
			string extractPath = Path.Combine(Path.GetDirectoryName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
			string tempDirectory = Path.Combine(Application.StartupPath, "APKFiles");  // Using relative path

			try
			{
				if (Directory.Exists(extractPath))
				{
					Directory.Delete(extractPath, true);
				}

				materialMultiLineTextBox3.AppendText($"[LOG] Extracting zip to: {extractPath}\n");
				System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath);
				materialMultiLineTextBox3.AppendText($"[LOG] Extraction complete.\n");

				string[] apkFiles = Directory.GetFiles(extractPath, "*.apk", SearchOption.AllDirectories);
				materialMultiLineTextBox3.AppendText($"[LOG] Found {apkFiles.Length} APK file(s) in zip.\n");
				foreach (var apk in apkFiles)
					materialMultiLineTextBox3.AppendText($"[LOG]   - {apk} ({new FileInfo(apk).Length} bytes)\n");

				if (apkFiles.Length > 0)
				{
					// Ensure temp directory exists
					if (!Directory.Exists(tempDirectory))
					{
						Directory.CreateDirectory(tempDirectory);
					}

					// Properly quote each file path
					var quotedFilePaths = apkFiles.Select(f => $"\"{f}\"");
					string installCommand = "adb install-multiple -r -d --user 0 " + string.Join(" ", quotedFilePaths);

					materialMultiLineTextBox3.AppendText($"[LOG] Install command: {installCommand}\n");

					string result = await parentForm.ExecuteAdbCommand(installCommand);

					materialMultiLineTextBox3.AppendText($"[LOG] ADB install result: {result}\n");

					Console.WriteLine(result);
					Console.WriteLine("laith");
					Console.WriteLine(installCommand);
					if (!string.IsNullOrWhiteSpace(result))
					{
						// Show the result in a message box
						materialMultiLineTextBox3.AppendText($"Install command result: {result}\n");

						// Check if the installation was successful

					}
					else
					{
						materialMultiLineTextBox3.AppendText("No result from the adb command.\n");
					}
				}
				else
				{
					materialMultiLineTextBox3.AppendText("No APK files found after unzipping.\n");
				}
			}
			catch (Exception ex)
			{
				materialMultiLineTextBox3.AppendText($"Error during unzip and install: {ex.Message}\n");
				materialMultiLineTextBox3.AppendText($"[LOG] UnzipAndInstall Exception Type: {ex.GetType().FullName}\n");
				if (ex.InnerException != null)
					materialMultiLineTextBox3.AppendText($"[LOG] Inner Exception: {ex.InnerException.Message}\n");
			}
			finally
			{
				CleanUp(extractPath);
				CleanUp(tempDirectory);
			}
		}

		private async Task<string> GetDeviceModel()
		{
			string modelCommand = "adb shell getprop ro.product.model";
			string modelOutput = await parentForm.ExecuteAdbCommand(modelCommand);
			return modelOutput.Trim();
		}

		private async Task<string> GetCurrentVersion(string packageName)
		{
			if (string.IsNullOrEmpty(packageName))
			{
				return null;
			}

			string command = $"adb shell dumpsys package {packageName} | findstr versionName";
			string output = await parentForm.ExecuteAdbCommand(command);

			materialMultiLineTextBox3.AppendText($"[LOG] GetCurrentVersion raw output for {packageName}: [{output}]\n");

			if (!string.IsNullOrEmpty(output))
			{
				string versionLine = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
				string versionName = versionLine.Split('=')[1].Trim();
				materialMultiLineTextBox3.AppendText($"[LOG] Parsed version for {packageName}: {versionName}\n");
				return versionName;
			}

			materialMultiLineTextBox3.AppendText($"[LOG] No version found for {packageName}\n");
			return null;
		}


		private async Task InstallApk(string filePath)
		{
			materialMultiLineTextBox3.AppendText("Please wait Installing Update, this can take up to 2 minutes\n");

			string installCommand = $"adb install -r \"{filePath}\"";
			materialMultiLineTextBox3.AppendText($"[LOG] Install command: {installCommand}\n");
			string result = await parentForm.ExecuteAdbCommand(installCommand);
			materialMultiLineTextBox3.AppendText($"[LOG] Install result: {result}\n");
		}



		private async Task<bool> RebootApp(string appName, string packageName, string latestVersion)
		{
			string currentVersion = await GetCurrentVersion(packageName);
			materialMultiLineTextBox3.AppendText($"[LOG] RebootApp version check: current={currentVersion ?? "null"}, expected={latestVersion}\n");

			if (currentVersion == latestVersion)
			{
				materialMultiLineTextBox3.AppendText($"Successfully updated {appName} to version {latestVersion}\n");
				materialMultiLineTextBox3.AppendText("Please wait Rebooting Device, this can take up to 45 seconds\n");

				string rebootCommand = "adb reboot";
				await parentForm.ExecuteAdbCommand(rebootCommand);

				await Task.Delay(45000);
				return true;
			}
			else
			{
				materialMultiLineTextBox3.AppendText($"Failed to install {appName}. Retrying..\n");
				materialMultiLineTextBox3.AppendText($"[LOG] Version mismatch after install: got {currentVersion ?? "null"}, expected {latestVersion}\n");
				return false;
			}
		}

		private void CleanUp(string path)
		{
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				try
				{
					File.Delete(path);
				}
				catch (Exception ex)
				{
					materialMultiLineTextBox3.AppendText($"Error deleting file: {ex.Message}\n");
				}
			}

			if (!string.IsNullOrEmpty(path))
			{
				string extractPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
				if (Directory.Exists(extractPath))
				{
					try
					{
						Directory.Delete(extractPath, true);
					}
					catch (Exception ex)
					{
						materialMultiLineTextBox3.AppendText($"Error deleting directory: {ex.Message}\n");
					}
				}
			}
		}
		private void DisableOtherButtons(Button clickedButton)
		{
			if (!(clickedButton.Tag is string clickedApp)) return;

			foreach (var kv in _updateButtons)
			{
				string appName = kv.Key;
				Button eachBtn = kv.Value;
				// Always disable all, but only keep the clicked button visible
				eachBtn.Enabled = false;
				eachBtn.Visible = (appName == clickedApp);
			}

			foreach (var kv in _statusLabels)
			{
				string appName = kv.Key;
				Label eachLabel = kv.Value;
				eachLabel.Visible = (appName == clickedApp);
			}
		}

		private void EnableAllButtons()
		{
			foreach (var kv in _updateButtons)
			{
				string appName = kv.Key;
				Button eachBtn = kv.Value;

				eachBtn.Visible = true;

				if (_statusLabels.TryGetValue(appName, out var lbl))
				{
					bool isUpToDate = lbl.Text.StartsWith("Up to date", StringComparison.OrdinalIgnoreCase);
					eachBtn.Enabled = !isUpToDate;
				}
				else
				{
					eachBtn.Enabled = true;
				}
			}

			foreach (var lbl in _statusLabels.Values)
			{
				lbl.Visible = true;
			}
		}

		private void UpdateStatusLabel(string appName, Button clickedButton)
		{
			if (!_statusLabels.TryGetValue(appName, out var statusLabel)) return;

			// If previous label text was "Update available: vX → vY", extract vY
			string previousText = statusLabel.Text;
			string newVersion = "";
			if (previousText.Contains("→"))
			{
				var parts = previousText.Split('→');
				newVersion = parts[1].Trim(); // "vY"
			}

			statusLabel.Text = string.IsNullOrEmpty(newVersion)
				? "Up to date"
				: $"Up to date ({newVersion})";

			if (_updateButtons.TryGetValue(appName, out var btn))
			{
				btn.Enabled = false;
			}
		}



		private async Task<string> updateExecuteCommand(string command)
		{
			string commandOutput = "";

			try
			{
				ProcessStartInfo pStartInfo = new ProcessStartInfo
				{
					FileName = "cmd.exe",
					Arguments = $"/K cd /d \"C:/Users/Dev\" && {command}",
					UseShellExecute = true,  // Use the shell to execute the command
					Verb = "runas",          // Prompt to run as administrator
					CreateNoWindow = false,
					WindowStyle = ProcessWindowStyle.Normal,
				};

				pStartInfo.EnvironmentVariables["PATH"] = @"C:\Users\Dev;" + Environment.GetEnvironmentVariable("PATH");



				using (Process cmdProcess = new Process { StartInfo = pStartInfo })
				{
					cmdProcess.Start();
					cmdProcess.WaitForExit();
					// We can't capture output directly because UseShellExecute = true.
					commandOutput = "Command executed with elevated privileges. Please check the result manually.";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to run command as administrator: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return commandOutput;
		}

	}
}