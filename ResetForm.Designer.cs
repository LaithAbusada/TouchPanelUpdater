using System;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class ResetForm
    {
        private Guna.UI2.WinForms.Guna2Button btnApp1;
        private Guna.UI2.WinForms.Guna2Button btnApp2;
        private Guna.UI2.WinForms.Guna2Button btnApp3;
        private Guna.UI2.WinForms.Guna2Button btnApp4;

        private void InitializeComponent()
        {
            this.btnApp1 = new Guna.UI2.WinForms.Guna2Button();
            this.btnApp2 = new Guna.UI2.WinForms.Guna2Button();
            this.btnApp3 = new Guna.UI2.WinForms.Guna2Button();
            this.btnApp4 = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // btnApp1
            // 
            this.btnApp1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnApp1.ForeColor = System.Drawing.Color.White;
            this.btnApp1.Image = global::Innovo_TP4_Updater.Properties.Resources.Nice;
            this.btnApp1.ImageSize = new System.Drawing.Size(50, 50);
            this.btnApp1.Location = new System.Drawing.Point(262, 110);
            this.btnApp1.Name = "btnApp1";
            this.btnApp1.Size = new System.Drawing.Size(200, 60);
            this.btnApp1.TabIndex = 0;
            this.btnApp1.Text = "Reset Nice";
            this.btnApp1.Click += new System.EventHandler(this.btnApp1_Click);
            // 
            // btnApp2
            // 
            this.btnApp2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnApp2.ForeColor = System.Drawing.Color.White;
            this.btnApp2.Image = global::Innovo_TP4_Updater.Properties.Resources.four;
            this.btnApp2.ImageSize = new System.Drawing.Size(50, 50);
            this.btnApp2.Location = new System.Drawing.Point(262, 219);
            this.btnApp2.Name = "btnApp2";
            this.btnApp2.Size = new System.Drawing.Size(200, 60);
            this.btnApp2.TabIndex = 1;
            this.btnApp2.Text = "Reset Control4";
            this.btnApp2.Click += new System.EventHandler(this.btnApp2_Click);
            // 
            // btnApp3
            // 
            this.btnApp3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnApp3.ForeColor = System.Drawing.Color.White;
            this.btnApp3.Image = global::Innovo_TP4_Updater.Properties.Resources.ral;
            this.btnApp3.ImageSize = new System.Drawing.Size(50, 50);
            this.btnApp3.Location = new System.Drawing.Point(575, 110);
            this.btnApp3.Name = "btnApp3";
            this.btnApp3.Size = new System.Drawing.Size(200, 60);
            this.btnApp3.TabIndex = 2;
            this.btnApp3.Text = "Reset Ral";
            this.btnApp3.Click += new System.EventHandler(this.btnApp3_Click);
            // 
            // btnApp4
            // 
            this.btnApp4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnApp4.ForeColor = System.Drawing.Color.White;
            this.btnApp4.Image = global::Innovo_TP4_Updater.Properties.Resources.Lutron;
            this.btnApp4.ImageSize = new System.Drawing.Size(50, 50);
            this.btnApp4.Location = new System.Drawing.Point(575, 219);
            this.btnApp4.Name = "btnApp4";
            this.btnApp4.Size = new System.Drawing.Size(200, 60);
            this.btnApp4.TabIndex = 3;
            this.btnApp4.Text = "Reset Lutron";
            this.btnApp4.Click += new System.EventHandler(this.btnApp4_Click);
            // 
            // ResetForm
            // 
            this.ClientSize = new System.Drawing.Size(975, 648);
            this.Controls.Add(this.btnApp1);
            this.Controls.Add(this.btnApp2);
            this.Controls.Add(this.btnApp3);
            this.Controls.Add(this.btnApp4);
            this.Name = "ResetForm";
            this.ResumeLayout(false);

        }
    }
}
