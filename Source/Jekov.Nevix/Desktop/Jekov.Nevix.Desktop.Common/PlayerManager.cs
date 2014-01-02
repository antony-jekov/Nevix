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
        public PlayerManager(string bsplayerLocation)
        {
            bsplayer = Process.GetProcessesByName("bsplayer").FirstOrDefault();

            if (bsplayer == null)
            {
                bsplayer = Process.Start(bsplayerLocation);
            }
        }

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr handleWindow);

        private readonly Process bsplayer;

        public void ExecuteCmd(string cmd)
        {
            if (bsplayer.HasExited)
            {
                bsplayer.Start();
                Thread.Sleep(5000);
            }

            SetForegroundWindow(bsplayer.MainWindowHandle);
            SendKeys.SendWait(cmd);
        }
    }
}