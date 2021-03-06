﻿using Jekov.Nevix.Desktop.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jekov.Nevix.Desktop.Client
{
    static class Program
    {
        private static Form mainForm;
        private static Form loginForm;
        public static bool logOutScheduled;

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!CheckForInternetConnection())
            {
                MessageBox.Show("Nevix needs internet connection to work properly!", "No internet connection!");
                Application.Exit();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var db = NevixLocalDbContext.Instance();

            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName("Jekov.Nevix.Desktop.Client");

            if (processes.Length > 1)
            {
                foreach (var process in processes)
                {
                    if (process.Id == currentProcess.Id)
                    {
                        continue;
                    }

                    process.Kill();
                }
            }

            Form main = GetCurrentForm(db);
            
            Application.Run(main);

            //if (!string.IsNullOrEmpty(db.LocalDb.SessionKey) && main is MainForm)
                //main.Close();
            //using (NotifyIcon icon = new NotifyIcon())
            //{
            //    icon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            //    icon.ContextMenu = new ContextMenu(new MenuItem[] {
            //    new MenuItem("Open Nevix", (s, e) => {
            //        Program.GetCurrentForm(db).Show();
            //    }),
            //    new MenuItem("Exit", (s, e) => { Application.Exit(); })
            //    });

            //    icon.Visible = true;

            //    Application.Run();
            //    icon.Visible = false;
            //}
        }

        public static Form GetCurrentForm(NevixLocalDbContext db)
        {
            if (!string.IsNullOrEmpty(db.LocalDb.SessionKey))
                return MainForm(db.LocalDb.Email, db.LocalDb.SessionKey);
            else
                return LoginForm();
        }

        public static Form MainForm(string email, string sessionKey)
        {
            if (mainForm == null || mainForm.IsDisposed)
            {
                mainForm = new MainForm(email, sessionKey);
            }

            return mainForm;
        }

        public static Form LoginForm()
        {
            if (loginForm == null)
            {
                loginForm = new LoginForm();
            }

            return loginForm;
        }
    }
}
