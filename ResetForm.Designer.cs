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
            this.SuspendLayout();
            // 
            // appsPanel
            // 
            this.appsPanel.Location = new System.Drawing.Point(25, 166);
            this.appsPanel.Name = "appsPanel";
            this.appsPanel.Size = new System.Drawing.Size(942, 416);
            this.appsPanel.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(199, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(517, 36);
            this.label1.TabIndex = 16;
            this.label1.Text = "Reset Application Cache And Storage";
            // 
            // ResetForm
            // 
            this.ClientSize = new System.Drawing.Size(1204, 649);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.appsPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ResetForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label label1;
    }
}
