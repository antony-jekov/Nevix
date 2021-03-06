﻿using Jekov.Nevix.Desktop.Common;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Jekov.Nevix.Desktop.Client
{
    public partial class LoginForm : Form
    {
        private NevixLocalDbContext db;
        private PersisterManager persister;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string email = this.email.Text.ToLower().Trim();
            string pass = this.password.Text.Trim();

            if (!ValidateLoginData(email, pass))
            {
                return;
            }

            progressIndicator.Visible = true;
            Login(email, pass);
        }

        private void ToggleControlsEnabled(bool enabled)
        {
            login.Enabled = enabled;
            register.Enabled = enabled;
        }

        private bool ValidateLoginData(string email, string pass)
        {
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email is empty!");
                return false;
            }

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Password is empty!");
                return false;
            }

            Match match = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$").Match(email);
            if (!match.Success)
            {
                MessageBox.Show("Invalid email!");
                return false;
            }

            return true;
        }

        private async void Register_Click(object sender, EventArgs e)
        {
            string email = this.email.Text.ToLower().Trim();
            string pass = this.password.Text.Trim();
            string confirm = this.confirm.Text.Trim();

            if (!ValidateLoginData(email, pass))
            {
                return;
            }

            if (pass.Length < 5)
            {
                MessageBox.Show("Password must be at least 5 characters long!", "Password too short", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pass.Length > 40)
            {
                MessageBox.Show("Password must not be more than 40 characters long!", "Password too long", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //if (!pass.Any(c => char.IsDigit(c)))
            //{
            //    MessageBox.Show("Password must have at least one digit in it!", "Weak password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            if (email.Length < 8)
            {
                MessageBox.Show("The email is too short to be valid!", "Invalid email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (email.Length > 300)
            {
                MessageBox.Show("The email must not be more than 300 character long!", "Invalid email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Please fill in the confirm field", "Confirm is empty!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!pass.Equals(confirm))
            {
                MessageBox.Show("Passwords do not match!", "Password missmatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            progressIndicator.Visible = true;
            register.Enabled = false;
            Register(email, pass, confirm);
        }

        private async void Login(string email, string pass)
        {
            string sessionKey = string.Empty;
            try
            {
                ToggleControlsEnabled(false);
                sessionKey = await persister.Login(email, pass);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Wrong email or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToggleControlsEnabled(true);
                progressIndicator.Visible = false;
                return;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Your email needs to be confirmed.\nPlease visit your email inbox and follow the activation link.\n\nIf you can't see the activation email make sure to check the 'Spam' folder.", "Confirm Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ToggleControlsEnabled(true);
                progressIndicator.Visible = false;
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressIndicator.Visible = false;
                ToggleControlsEnabled(true);
                return;
            }

            progressIndicator.Visible = false;
            db.LocalDb.SessionKey = sessionKey;
            db.LocalDb.Email = email;
            db.LocalDb.Password = pass;

            db.LocalDb.Remember = remember.Checked;
            db.SaveChanges();

            SwitchToMain(email, sessionKey);
        }

        private async void Register(string email, string pass, string confirm)
        {
            try
            {
                ToggleControlsEnabled(false);
                await persister.Register(email, pass, confirm);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("There is already an user with that email.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ToggleControlsEnabled(true);
                progressIndicator.Visible = false;
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Server Down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }

            progressIndicator.Visible = false;

            db.LocalDb.Email = email;
            db.LocalDb.Password = pass;
            db.LocalDb.ClearMedia();
            db.LocalDb.Remember = remember.Checked;
            db.SaveChanges();

            MessageBox.Show(string.Format("Account '{0}' was created successfully!\n\nPlease check your email inbox and follow the confirmation link we sent you.\n\nIf you cannot see the email prease check your 'Spam' folder.", email), "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ToggleControlsEnabled(true);
        }

        private void SwitchToMain(string email, string sessionKey)
        {
            ToggleControlsEnabled(true);

            if (string.IsNullOrEmpty(sessionKey))
            {
                MessageBox.Show("There was an error trying to open your session.\n\nIf the problem appears again please contact us at support@nevix-remote.com\nWe appologize for any inconvinience that this might have cost you!");
                return;
            }

            var frm = Program.MainForm(email, sessionKey);
            
            frm.Show();
            this.Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            db = NevixLocalDbContext.Instance();

            persister = PersisterManager.Instance();
            password.KeyPress += new KeyPressEventHandler(CheckEnterKeyPressLogin);
            confirm.KeyPress += new KeyPressEventHandler(CheckEnterKeyPressRegister);

            if (db.LocalDb.Remember)
            {
                string emailCache = db.LocalDb.Email;

                if (!string.IsNullOrEmpty(emailCache))
                {
                    email.Text = emailCache;
                    password.Focus();
                }

                string pass = db.LocalDb.Password;

                if (!string.IsNullOrEmpty(pass))
                {
                    password.Text = pass;
                }

                remember.Checked = true;

                login.Select();
            }
        }

        private void CheckEnterKeyPressLogin(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                login.PerformClick();
            }
        }

        private void CheckEnterKeyPressRegister(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                register.PerformClick();
            }
        }

        private void LoginForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!Program.logOutScheduled)
            {
                return;
            }

            Program.MainForm("", "").Dispose();
            Program.logOutScheduled = false;
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private async void resetPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string email = this.email.Text.ToLower();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email is empty!");
                return;
            }

            Match match = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$").Match(email);
            if (!match.Success)
            {
                MessageBox.Show("Invalid email!");
                return;
            }

            await persister.ResetPassword(email);
            MessageBox.Show("Check your email inbox and follow the link to change your password.\nIf you cannot find the email make sure to check the 'Spam' folder as well.", "Password Reset Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}