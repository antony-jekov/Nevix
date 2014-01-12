using Jekov.Nevix.Desktop.Common;
using System;
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
            string email = this.email.Text;
            string pass = this.password.Text;

            if (!ValidateLoginData(email, pass))
            {
                return;
            }


            login.Enabled = false;
            Login(email, pass);
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

            return true;
        }

        private void Register_Click(object sender, EventArgs e)
        {
            string email = this.email.Text.Trim();
            string pass = this.password.Text.Trim();
            string confirm = this.confirm.Text.Trim();

            if (!ValidateLoginData(email, pass))
            {
                return;
            }

            if (string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Confirm is empty!");
                return;
            }

            if (!pass.Equals(confirm))
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            register.Enabled = false;
            Register(email, pass, confirm);
        }

        private void Login(string email, string pass)
        {
            string sessionKey = string.Empty;
            try
            {
                sessionKey = persister.Login(email, pass);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Wrong email or password.");
                login.Enabled = true;
                return;
            }

            db.LocalDb.SessionKey = sessionKey;
            db.LocalDb.SessionKey = persister.Login(email, pass);
            db.LocalDb.Email = email;
            db.LocalDb.Password = pass;

            db.LocalDb.Remember = remember.Checked;
            db.SaveChanges();

            SwitchToMain(email, sessionKey);
        }

        private void Register(string email, string pass, string confirm)
        {
            string sessionKey = string.Empty;
            try
            {
                sessionKey = persister.Register(email, pass, confirm);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("There is already an user with that email.");
                register.Enabled = true;
                return;
            }

            db.LocalDb.SessionKey = sessionKey;

            db.LocalDb.Email = email;
            db.LocalDb.Password = pass;
            db.LocalDb.ClearMedia();
            db.LocalDb.Remember = remember.Checked;
            db.SaveChanges();

            SwitchToMain(email, sessionKey);
        }

        private void SwitchToMain(string email, string sessionKey)
        {
            var frm = new MainForm(email, sessionKey);
            frm.FormClosed += delegate { this.Close(); };
            frm.Show();
            this.Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            db = new NevixLocalDbContext();
            persister = new PersisterManager();
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
    }
}