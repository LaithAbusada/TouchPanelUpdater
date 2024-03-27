using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace TouchPanelUpdater
{
    public partial class PasswordForm : Form
    {
        private const string HashedPassword = "faQI6c2h2ZM2jl3xtNkRH8XrbBc=";

        public PasswordForm()
        {
            InitializeComponent();
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            string x = EncodePasswordToBase64(HashedPassword);
            
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string enteredPassword = txtPassword.Text;

            // Hash the entered password
            string enteredPasswordHash = EncodePasswordToBase64(enteredPassword);
            
            // Compare the hashed passwords
            if (enteredPasswordHash == HashedPassword)
            {
                this.Hide();   
                // Password is correct, close the PasswordForm and open the main form of your application
                Form1 mainForm = new Form1();

                mainForm.ShowDialog();

                this.Close();

                
            }
            else
            {
                // Display an error message for incorrect password
                MessageBox.Show("Incorrect password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
            }   
        }


        public static string EncodePasswordToBase64(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
            return Convert.ToBase64String(inArray);
        }
    }
}
