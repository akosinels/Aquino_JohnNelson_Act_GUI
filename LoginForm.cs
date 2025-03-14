using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Aquino_JohnNelson_Act_GUI
{
    public partial class LoginForm : Form
    {
        // counter for tracking login attempts
        private int loginAttempts = 0;
        // database connection instance
        private readonly DatabaseConnection dbConnection;
        // DataTable to hold user data
        private DataTable dt = new DataTable();
        // index for iterating through user data
        private int index = 0;
        // variables to store username and password
        public string username;
        public string password;

        // ErrorProvider instances
        private ErrorProvider errorProviderUsername = new ErrorProvider();
        private ErrorProvider errorProviderPassword = new ErrorProvider();

        public LoginForm()
        {
            InitializeComponent();
            // initialize database connection
            dbConnection = new DatabaseConnection();
            // check if the database connection is successful
            if (!dbConnection.Connect())
            {
                // show error message if connection fails
                MessageBox.Show("Database connection failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // load users from the database if connection is successful
                load_users();
            }
        }

        // method to load users from the database
        private void load_users()
        {
            dbConnection.Command.CommandType = CommandType.Text;
            dbConnection.Command.CommandText = "Select * from USERLOGINTB";
            dbConnection.DataReader = dbConnection.Command.ExecuteReader();
            dt.Clear();
            dt.Load(dbConnection.DataReader);
            dbConnection.DataReader.Close();
        }

        // method to set username and password from the DataTable
        private void usercred(int i)
        {
            username = dt.Rows[i][1].ToString();
            password = dt.Rows[i][2].ToString();
        }

        // event handler for the login button click event
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            usercred(index);
            if (UsernameTextBox.Text == username && PasswordTextBox.Text == password)
            {
                Form studentForm = new StudentForm();
                this.Hide();
                studentForm.Show();
            }
            else
            //below code is data validation for login
            {
                string errorMessage = "";
                loginAttempts++;
                if (loginAttempts >= 5)
                {
                    MessageBox.Show("Too many failed login attempts. Please reset your password using the link below.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Password reset link (for display only)
                    MessageBox.Show("Reset Password Link: www.example.com/resetpassword", "Reset Password Link", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit(); // Or disable login button and show reset link
                    return;
                }
                else
                if (string.IsNullOrEmpty(UsernameTextBox.Text))
                {
                    errorProviderUsername.SetError(UsernameTextBox, "Username is required!");
                    UsernameTextBox.Focus();
                }
                else if (string.IsNullOrEmpty(PasswordTextBox.Text))
                {
                    errorProviderPassword.SetError(PasswordTextBox, "Password is required");
                    PasswordTextBox.Focus();
                }
                else if (string.IsNullOrEmpty(UsernameTextBox.Text) && string.IsNullOrEmpty(PasswordTextBox.Text))
                {
                    errorProviderUsername.SetError(UsernameTextBox, "Username is required!");
                    UsernameTextBox.Focus();
                    errorProviderPassword.SetError(PasswordTextBox, "Password is required");
                    PasswordTextBox.Focus();
                }
                else if (UsernameTextBox.Text != username && PasswordTextBox.Text != password)
                {
                    errorProviderUsername.SetError(UsernameTextBox, "Please input the right username");
                    UsernameTextBox.Focus();
                    errorProviderPassword.SetError(PasswordTextBox, "Please use the right password");
                    PasswordTextBox.Focus();
                }
                else if (UsernameTextBox.Text != username)
                {
                    errorProviderUsername.SetError(UsernameTextBox, "Please input the right username");
                    UsernameTextBox.Focus();
                }
                else if (PasswordTextBox.Text != password)
                {
                    errorProviderPassword.SetError(PasswordTextBox, "Please use the right password");
                    PasswordTextBox.Focus();
                }

                MessageBox.Show(errorMessage + $"\nAttempt: {loginAttempts} of 5", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Clear password field after failed attempt
                PasswordTextBox.Clear();
                PasswordTextBox.Focus(); // Set focus back to password textbox for next attempt
            }
        }
    }
}
