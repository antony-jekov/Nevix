namespace Jekov.Nevix.Desktop.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class PlayerManager
    {
        private Dictionary<string, string> commands;

        public PlayerManager(string bsplayerLocation)
        {
            bsplayer = Process.GetProcessesByName("bsplayer").FirstOrDefault();

            if (bsplayer == null)
            {
                bsplayer = Process.Start(bsplayerLocation);
            }

            commands = new Dictionary<string, string>();
            commands.Add("play", "x");
            commands.Add("pause", "c");
            commands.Add("next", "b");
            commands.Add("prev", "y");
            commands.Add("volume_up", "{up}");
            commands.Add("volume_down", "{down}");
            commands.Add("rw", "^{F6}");
            commands.Add("ff", "^{F5}");
            commands.Add("full", "f");
            commands.Add("step_f", ".");
            commands.Add("step_b", ",");
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
            if (commands.ContainsKey(cmd))
            {
                string keys = commands[cmd];
                SendKeys.SendWait(keys);
                Console.WriteLine("{0} - {1}", cmd, keys);
            }
            else
            {
                Console.WriteLine("Unknown command: {0}", cmd);
            }
        }
    }
}