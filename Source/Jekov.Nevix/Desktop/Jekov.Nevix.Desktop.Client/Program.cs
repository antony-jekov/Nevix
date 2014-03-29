using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jekov.Nevix.Desktop.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form main;
            var db = Nevix.Desktop.Common.NevixLocalDbContext.Instance();
            if (!string.IsNullOrEmpty(db.LocalDb.SessionKey))
                main = new MainForm(db.LocalDb.Email, db.LocalDb.SessionKey);
            else
                main = new LoginForm();

            Application.Run(main);
        }
    }
}
