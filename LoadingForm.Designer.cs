using System;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class LoadingForm : Form
    {
        private Label lblMessage;

        public LoadingForm(string message)
        {
            InitializeComponent();
            lblMessage.Text = message;
            CenterToScreen();
        }

        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(12, 33);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(202, 25);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Please wait, loading...";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadingForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(519, 100);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void UpdateMessage(string message)
        {
            if (lblMessage.InvokeRequired)
            {
                lblMessage.Invoke(new Action(() => lblMessage.Text = message));
            }
            else
            {
                lblMessage.Text = message;
            }
        }
    }
}
