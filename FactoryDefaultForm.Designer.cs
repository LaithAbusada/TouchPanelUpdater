using System;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    partial class FactoryDefaultForm
    {
        private System.Windows.Forms.FlowLayoutPanel appsPanel;

        private void InitializeComponent()
        {
            this.appsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // appsPanel
            // 
            this.appsPanel.Location = new System.Drawing.Point(21, 212);
            this.appsPanel.Name = "appsPanel";
            this.appsPanel.Size = new System.Drawing.Size(937, 416);
            this.appsPanel.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(288, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 36);
            this.label1.TabIndex = 17;
            this.label1.Text = "Factory Reset Application";
            // 
            // FactoryDefaultForm
            // 
            this.ClientSize = new System.Drawing.Size(1204, 649);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.appsPanel);
            this.Name = "FactoryDefaultForm";
            this.Load += new System.EventHandler(this.FactoryDefaultForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label label1;
    }
}
