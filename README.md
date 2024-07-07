# Touch Panel Updater

## Objective

The Touch Panel Updater is designed to automate the process of updating and managing applications on a touch panel device. This tool facilitates connecting to the device, checking for updates, installing the latest versions of applications, and rebooting the device when necessary. It aims to simplify the maintenance and update procedures for touch panel devices.

## Technology

The application is built using the following technologies:

- **C# and .NET Framework**: The primary programming language and framework for developing the Windows Forms application.
- **MaterialSkin**: A library used to implement Material Design principles in the Windows Forms application.
- **SharpAdbClient**: A .NET library for communicating with Android devices via ADB (Android Debug Bridge).
- **WK.Libraries.BetterFolderBrowserNS**: A library for enhanced folder browsing functionality.
- **AltoHttp**: A lightweight HTTP client for downloading files.
- **Windows Forms**: The graphical user interface (GUI) framework for the application.

## Usage

### Prerequisites

1. Ensure you have `adb.exe` located in the `Downloads` folder within the solution directory.
2. Make sure the touch panel device is accessible via IP and port.

### Installation

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to restore dependencies and compile the application.

### Running the Application

1. Launch the application by running `TouchPanelUpdater.exe`.
2. On startup, the application will attempt to locate `adb.exe` in the `Downloads` folder and start the ADB server.

### Connecting to the Device

1. Enter the IP address and port of the touch panel device in the respective text boxes.
2. Click the "Connect" button to establish a connection with the device.
3. The application will display the connection status in the text box.

### Checking for Updates

1. Click the "Check for Updates" button to compare the installed version of the application on the device with the latest available version.
2. If an update is available, the application will prompt you to proceed with the update.

### Installing Updates

1. Click the "Install Update" button to start the update process.
2. The application will download the latest APK file, install it on the device, and provide status updates in the text box.
3. Once the installation is complete, the device will automatically reboot.

### Resetting the Application

1. Click the "Reset Application" button to clear all application data and reset the app from scratch.
2. The application will prompt for confirmation before proceeding.
3. After the reset, the device will automatically reboot.

### Disconnecting the Device

1. Click the "Disconnect" button to disconnect the device from the application.
2. The application will provide status updates in the text box.

### Additional Information

- The application provides detailed logs and status updates in the multi-line text boxes for easier troubleshooting.
- Ensure the device is on the same network as the PC running the application for seamless connectivity.

### Disclaimer

Use this tool responsibly and ensure you have the necessary permissions to update and manage applications on the touch panel device.
