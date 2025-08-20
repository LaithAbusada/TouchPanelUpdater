using System;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class ResetForm
    {
        private System.Windows.Forms.FlowLayoutPanel appsPanel;

        private void InitializeComponent()
        {
            this.appsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnControlApp = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // appsPanel
            // 
            this.appsPanel.Location = new System.Drawing.Point(30, 221);
            this.appsPanel.Name = "appsPanel";
            this.appsPanel.Size = new System.Drawing.Size(942, 416);
            this.appsPanel.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(199, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(517, 36);
            this.label1.TabIndex = 16;
            this.label1.Text = "Reset Application Cache And Storage";
            // 
            // btnControlApp
            // 
            this.btnControlApp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnControlApp.ForeColor = System.Drawing.Color.White;
            this.btnControlApp.Image = global::Innovo_TP4_Updater.Properties.Resources.reboot;
            this.btnControlApp.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnControlApp.ImageSize = new System.Drawing.Size(40, 40);
            this.btnControlApp.Location = new System.Drawing.Point(46, 135);
            this.btnControlApp.Name = "btnControlApp";
            this.btnControlApp.Size = new System.Drawing.Size(250, 60);
            this.btnControlApp.TabIndex = 17;
            this.btnControlApp.Text = "Launch Innovo Control App";
            this.btnControlApp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnControlApp.Click += new System.EventHandler(this.btnControlApp_Click);
            // 
            // ResetForm
            // 
            this.ClientSize = new System.Drawing.Size(1204, 649);
            this.Controls.Add(this.btnControlApp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.appsPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ResetForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label label1;
        private Guna.UI2.WinForms.Guna2Button btnControlApp;
    }
}
