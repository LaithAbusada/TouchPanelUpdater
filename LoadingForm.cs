using System;
using System.Windows.Forms;

public class LoadingForm : Form
{
    private Label messageLabel;

    public LoadingForm(string initialMessage)
    {
        messageLabel = new Label()
        {
            Text = initialMessage,
            Dock = DockStyle.Fill,
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };
        Controls.Add(messageLabel);
        this.StartPosition = FormStartPosition.CenterScreen;
    }

    public void UpdateMessage(string message)
    {
        if (messageLabel.InvokeRequired)
        {
            messageLabel.Invoke(new Action(() => messageLabel.Text = message));
        }
        else
        {
            messageLabel.Text = message;
        }
    }
}
