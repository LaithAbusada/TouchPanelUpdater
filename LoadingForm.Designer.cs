using System;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class LoadingForm : Form
    {
        public LoadingForm(string message)
        {
            InitializeComponent();
            lblMessage.Text = message;
        }

        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(229, 21);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Please wait, loading...";
            // 
            // LoadingForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 61);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblMessage;
    }
}
