namespace Innovo_TP4_Updater
{
    partial class UpdateAppForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button buttonUpdateNice;
        private System.Windows.Forms.Button buttonUpdateRako;
        private System.Windows.Forms.Button buttonUpdateLutron;
        private System.Windows.Forms.Button buttonUpdateControl4;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox3;
        private System.Windows.Forms.Label labelStatus;

        // New labels to display status
        private System.Windows.Forms.Label labelNiceStatus;
        private System.Windows.Forms.Label labelRakoStatus;
        private System.Windows.Forms.Label labelLutronStatus;
        private System.Windows.Forms.Label labelControl4Status;

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

        private void InitializeComponent()
        {
            this.buttonUpdateNice = new System.Windows.Forms.Button();
            this.buttonUpdateRako = new System.Windows.Forms.Button();
            this.buttonUpdateLutron = new System.Windows.Forms.Button();
            this.buttonUpdateControl4 = new System.Windows.Forms.Button();
            this.materialMultiLineTextBox3 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelNiceStatus = new System.Windows.Forms.Label();
            this.labelRakoStatus = new System.Windows.Forms.Label();
            this.labelLutronStatus = new System.Windows.Forms.Label();
            this.labelControl4Status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonUpdateNice
            // 
            this.buttonUpdateNice.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonUpdateNice.BackgroundImage = global::Innovo_TP4_Updater.Properties.Resources.Nice;
            this.buttonUpdateNice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonUpdateNice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonUpdateNice.ForeColor = System.Drawing.Color.White;
            this.buttonUpdateNice.Location = new System.Drawing.Point(29, 20);
            this.buttonUpdateNice.Name = "buttonUpdateNice";
            this.buttonUpdateNice.Size = new System.Drawing.Size(200, 115);
            this.buttonUpdateNice.TabIndex = 4;
            this.buttonUpdateNice.Text = "Update Nice";
            this.buttonUpdateNice.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonUpdateNice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonUpdateNice.UseVisualStyleBackColor = false;
            this.buttonUpdateNice.Click += new System.EventHandler(this.buttonUpdateNice_Click);
            // 
            // buttonUpdateRako
            // 
            this.buttonUpdateRako.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonUpdateRako.BackgroundImage = global::Innovo_TP4_Updater.Properties.Resources.ral;
            this.buttonUpdateRako.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonUpdateRako.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonUpdateRako.ForeColor = System.Drawing.Color.White;
            this.buttonUpdateRako.Location = new System.Drawing.Point(254, 20);
            this.buttonUpdateRako.Name = "buttonUpdateRako";
            this.buttonUpdateRako.Size = new System.Drawing.Size(206, 115);
            this.buttonUpdateRako.TabIndex = 5;
            this.buttonUpdateRako.Text = "Update Rako";
            this.buttonUpdateRako.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonUpdateRako.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonUpdateRako.UseVisualStyleBackColor = false;
            this.buttonUpdateRako.Click += new System.EventHandler(this.buttonUpdateRako_Click);
            // 
            // buttonUpdateLutron
            // 
            this.buttonUpdateLutron.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonUpdateLutron.BackgroundImage = global::Innovo_TP4_Updater.Properties.Resources.Lutron;
            this.buttonUpdateLutron.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonUpdateLutron.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonUpdateLutron.ForeColor = System.Drawing.Color.White;
            this.buttonUpdateLutron.Location = new System.Drawing.Point(29, 164);
            this.buttonUpdateLutron.Name = "buttonUpdateLutron";
            this.buttonUpdateLutron.Size = new System.Drawing.Size(206, 115);
            this.buttonUpdateLutron.TabIndex = 6;
            this.buttonUpdateLutron.Text = "Update Lutron";
            this.buttonUpdateLutron.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonUpdateLutron.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonUpdateLutron.UseVisualStyleBackColor = false;
            this.buttonUpdateLutron.Click += new System.EventHandler(this.buttonUpdateLutron_Click);
            // 
            // buttonUpdateControl4
            // 
            this.buttonUpdateControl4.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonUpdateControl4.BackgroundImage = global::Innovo_TP4_Updater.Properties.Resources.four;
            this.buttonUpdateControl4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonUpdateControl4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonUpdateControl4.ForeColor = System.Drawing.Color.White;
            this.buttonUpdateControl4.Location = new System.Drawing.Point(254, 164);
            this.buttonUpdateControl4.Name = "buttonUpdateControl4";
            this.buttonUpdateControl4.Size = new System.Drawing.Size(206, 115);
            this.buttonUpdateControl4.TabIndex = 7;
            this.buttonUpdateControl4.Text = "Update Control4";
            this.buttonUpdateControl4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonUpdateControl4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonUpdateControl4.UseVisualStyleBackColor = false;
            this.buttonUpdateControl4.Click += new System.EventHandler(this.buttonUpdateControl4_Click);
            // 
            // materialMultiLineTextBox3
            // 
            this.materialMultiLineTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialMultiLineTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialMultiLineTextBox3.Depth = 0;
            this.materialMultiLineTextBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.materialMultiLineTextBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialMultiLineTextBox3.Hint = "";
            this.materialMultiLineTextBox3.Location = new System.Drawing.Point(26, 312);
            this.materialMultiLineTextBox3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialMultiLineTextBox3.Name = "materialMultiLineTextBox3";
            this.materialMultiLineTextBox3.Size = new System.Drawing.Size(634, 156);
            this.materialMultiLineTextBox3.TabIndex = 8;
            this.materialMultiLineTextBox3.Text = "";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(26, 477);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(39, 15);
            this.labelStatus.TabIndex = 9;
            this.labelStatus.Text = "Status";
            // 
            // labelNiceStatus
            // 
            this.labelNiceStatus.AutoSize = true;
            this.labelNiceStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelNiceStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelNiceStatus.Location = new System.Drawing.Point(29, 140);
            this.labelNiceStatus.Name = "labelNiceStatus";
            this.labelNiceStatus.Size = new System.Drawing.Size(0, 15);
            this.labelNiceStatus.TabIndex = 11;
            // 
            // labelRakoStatus
            // 
            this.labelRakoStatus.AutoSize = true;
            this.labelRakoStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelRakoStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelRakoStatus.Location = new System.Drawing.Point(254, 140);
            this.labelRakoStatus.Name = "labelRakoStatus";
            this.labelRakoStatus.Size = new System.Drawing.Size(0, 15);
            this.labelRakoStatus.TabIndex = 12;
            // 
            // labelLutronStatus
            // 
            this.labelLutronStatus.AutoSize = true;
            this.labelLutronStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelLutronStatus.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelLutronStatus.Location = new System.Drawing.Point(29, 284);
            this.labelLutronStatus.Name = "labelLutronStatus";
            this.labelLutronStatus.Size = new System.Drawing.Size(0, 15);
            this.labelLutronStatus.TabIndex = 13;
            // 
            // labelControl4Status
            // 
            this.labelControl4Status.AutoSize = true;
            this.labelControl4Status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelControl4Status.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelControl4Status.Location = new System.Drawing.Point(254, 284);
            this.labelControl4Status.Name = "labelControl4Status";
            this.labelControl4Status.Size = new System.Drawing.Size(0, 15);
            this.labelControl4Status.TabIndex = 14;
            // 
            // UpdateAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(900, 520);
            this.Controls.Add(this.labelControl4Status);
            this.Controls.Add(this.labelLutronStatus);
            this.Controls.Add(this.labelRakoStatus);
            this.Controls.Add(this.labelNiceStatus);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.materialMultiLineTextBox3);
            this.Controls.Add(this.buttonUpdateControl4);
            this.Controls.Add(this.buttonUpdateLutron);
            this.Controls.Add(this.buttonUpdateRako);
            this.Controls.Add(this.buttonUpdateNice);
            this.Name = "UpdateAppForm";
            this.Text = "Update App";
            this.Load += new System.EventHandler(this.UpdateAppForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
