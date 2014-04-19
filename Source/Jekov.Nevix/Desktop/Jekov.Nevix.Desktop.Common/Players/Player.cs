namespace Jekov.Nevix.Desktop.Common.Players
{
    using Jekov.Nevix.Desktop.Common.Contracts;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using WindowsInput;
    using WindowsInput.Native;

    public abstract class Player : SystemPlayer
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr handleWindow);

        public void BringPlayerToFront(IntPtr handleWindow) {
            SetForegroundWindow(handleWindow);
            Thread.Sleep(500);
        }
        
        private Process playerProcess;

        protected string PlayerLocation { get; set; }

        protected string PlayerProcessName { get; set; }

        protected Process PlayerProcess
        {
            get
            {
                if (playerProcess == null)
                    playerProcess = Process.GetProcessesByName(PlayerProcessName).FirstOrDefault();
                
                if (playerProcess == null)
                    playerProcess = Process.Start(PlayerLocation);

                try
                {
                    if (playerProcess.HasExited)
                        playerProcess.Start();
                }
                catch (Exception)
                {
                    playerProcess = null;
                    return PlayerProcess;
                }

                return playerProcess;
            }
            set
            {
                playerProcess = value;
            }
        }

        public override void BringUp()
        {
            BringPlayerToFront(PlayerProcess.MainWindowHandle);
        }

        public override void Exit()
        {
            PlayerProcess.Kill();
        }

        public Player(string playerProcessName, string playerLocation)
        {
            PlayerLocation = playerLocation;
            PlayerProcessName = playerProcessName;
        }

        public override void OpenFile(string location)
        {
            if (!string.IsNullOrEmpty(PlayerLocation))
            {
                if(!PlayerProcess.HasExited)
                    PlayerProcess.Kill();
                PlayerProcess = Process.Start(PlayerLocation, location);
            }
            else
            {
                base.OpenFile(location);
            }
        }
    }
}