using System;
using System.Windows.Forms;

namespace Innovo_TP4_Updater
{
    public partial class SettingsForm : Form
    {
        private Form1 parentForm;

        public SettingsForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;
            MessageBox.Show($"{button.Text.Split('\n')[0]} button clicked.");

            if (button == btnSound)
            {
                SoundSettingsForm soundSettingsForm = new SoundSettingsForm(parentForm);
              
                parentForm.LoadFormIntoPanel(soundSettingsForm);
            }
            else if (button == btnDisplay)
            {
                DisplaySettingsForm displaySettingsForm = new DisplaySettingsForm(parentForm);
                parentForm.LoadFormIntoPanel(displaySettingsForm);
            }
            else if (button == btnUpdate)
            {
                UpdateAppForm updateAppForm = new UpdateAppForm(parentForm);
                parentForm.LoadFormIntoPanel(updateAppForm);
            }
        }
    }
}
