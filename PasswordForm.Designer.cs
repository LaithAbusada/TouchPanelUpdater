namespace Innovo_TP4_Updater
{
    partial class PasswordForm
    {
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordForm));
            this.button1 = new System.Windows.Forms.Button();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.lblPin = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.updateAppLabel = new System.Windows.Forms.LinkLabel();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            //
            // button1
            //
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(255, 250);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(222, 49);
            this.button1.TabIndex = 1;
            this.button1.Text = "Enter";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // txtPin
            //
            this.txtPin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPin.Location = new System.Drawing.Point(180, 180);
            this.txtPin.Margin = new System.Windows.Forms.Padding(2);
            this.txtPin.Name = "txtPin";
            this.txtPin.PasswordChar = '*';
            this.txtPin.Size = new System.Drawing.Size(454, 29);
            this.txtPin.TabIndex = 0;
            //
            // lblPin
            //
            this.lblPin.AutoSize = true;
            this.lblPin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lblPin.Location = new System.Drawing.Point(68, 180);
            this.lblPin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(103, 24);
            this.lblPin.TabIndex = 2;
            this.lblPin.Text = "PIN Code:";
            //
            // panelHeader
            //
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.panelHeader.Controls.Add(this.lblHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(2);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(675, 65);
            this.panelHeader.TabIndex = 6;
            //
            // lblHeader
            //
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(248, 16);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(214, 37);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Innovo Panel";
            //
            // updateAppLabel
            //
            this.updateAppLabel.AutoSize = true;
            this.updateAppLabel.Location = new System.Drawing.Point(25, 97);
            this.updateAppLabel.Name = "updateAppLabel";
            this.updateAppLabel.Size = new System.Drawing.Size(55, 13);
            this.updateAppLabel.TabIndex = 8;
            this.updateAppLabel.TabStop = true;
            this.updateAppLabel.Text = "linkLabel1";
            //
            // PasswordForm
            //
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(675, 366);
            this.Controls.Add(this.updateAppLabel);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.lblPin);
            this.Controls.Add(this.txtPin);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PasswordForm";
            this.Text = "Enter PIN";
            this.Load += new System.EventHandler(this.PasswordForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.LinkLabel updateAppLabel;
    }
}
