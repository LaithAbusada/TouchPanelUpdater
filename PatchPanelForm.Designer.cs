namespace Innovo_TP4_Updater
{
    partial class PatchPanelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPatch = new Guna.UI2.WinForms.Guna2Button();
            this.btnBootApp = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // btnPatch
            // 
            this.btnPatch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnPatch.ForeColor = System.Drawing.Color.White;
            this.btnPatch.Image = global::Innovo_TP4_Updater.Properties.Resources.patch;
            this.btnPatch.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnPatch.ImageSize = new System.Drawing.Size(40, 40);
            this.btnPatch.Location = new System.Drawing.Point(69, 107);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(250, 60);
            this.btnPatch.TabIndex = 12;
            this.btnPatch.Text = "Patch Panel\nPatch 4 Inch Touch Panel";
            this.btnPatch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // btnBootApp
            // 
            this.btnBootApp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBootApp.ForeColor = System.Drawing.Color.White;
            this.btnBootApp.Image = global::Innovo_TP4_Updater.Properties.Resources.reboot;
            this.btnBootApp.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnBootApp.ImageSize = new System.Drawing.Size(40, 40);
            this.btnBootApp.Location = new System.Drawing.Point(451, 107);
            this.btnBootApp.Name = "btnBootApp";
            this.btnBootApp.Size = new System.Drawing.Size(250, 60);
            this.btnBootApp.TabIndex = 13;
            this.btnBootApp.Text = "Launch Boot App";
            this.btnBootApp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnBootApp.Click += new System.EventHandler(this.btnBootApp_Click);
            // 
            // PatchPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(777, 360);
            this.Controls.Add(this.btnBootApp);
            this.Controls.Add(this.btnPatch);
            this.Name = "PatchPanelForm";
            this.Text = "PatchPanelForm";
            this.Load += new System.EventHandler(this.PatchPanelForm_LoadAsync);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnPatch;
        private Guna.UI2.WinForms.Guna2Button btnBootApp;
    }
}