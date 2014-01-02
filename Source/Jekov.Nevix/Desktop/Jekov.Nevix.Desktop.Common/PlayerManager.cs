namespace Jekov.Nevix.Desktop.Common
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class PlayerManager
    {
        public PlayerManager()
        {
            fileManager = new FileManager();
            bsplayer = Process.GetProcessesByName("bsplayer").FirstOrDefault();

            if (bsplayer == null)
            {
                string bsplayerLocation = fileManager.FindBsPlayerLocation();

                if (bsplayerLocation == string.Empty)
                {
                    bsplayerLocation = fileManager.AskBsPlayerLocation();
                    if (!bsplayerLocation.EndsWith("bsplayer.exe"))
                    {
                        throw new ArgumentException("bsplayerLocation", "Bad location of BSPlayer.");
                    }
                }

                try
                {
                    bsplayer = Process.Start(bsplayerLocation);
                }
                catch (Exception)
                {
                    //
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr handleWindow);

        private readonly FileManager fileManager;

        private readonly Process bsplayer;

        public void ExecuteCmd(string cmd)
        {
            if (bsplayer.HasExited)
            {
                Console.WriteLine("Restarting BSPlayer...");
                bsplayer.Start();
                Thread.Sleep(5000);
            }

            SetForegroundWindow(bsplayer.MainWindowHandle);
            SendKeys.SendWait(cmd);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Executing cmd '{0}'", cmd);
        }
    }
}