namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Desktop.Common.Contracts;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using WindowsInput;
    using WindowsInput.Native;

    public abstract class Player : IPlayer
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr handleWindow);

        protected InputSimulator input;

        public Player(string playerProcessName, string playerLocation)
        {
            input = new InputSimulator();
            player = Process.GetProcessesByName(playerProcessName).FirstOrDefault();

            if (player == null)
            {
                player = Process.Start(playerLocation);
            }
        }

        protected readonly Process player;

        public virtual void Play()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
        }

        public virtual void Pause()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
        }

        public virtual void Stop()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.MEDIA_STOP);
        }

        public virtual void Next()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.MEDIA_NEXT_TRACK);
        }

        public virtual void Previous()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PREV_TRACK);
        }

        public virtual void SystemVolumeUp()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VOLUME_UP);
        }

        public virtual void SystemVolumeDown()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VOLUME_DOWN);
        }

        public virtual void Mute()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VOLUME_MUTE);
        }

        public virtual void Power()
        {
            Process.Start("shutdown", "/s /t 0");
        }

        public abstract void StepForward();

        public abstract void StepBackward();

        public abstract void FastForward();

        public abstract void Rewind();

        public abstract void OpenFile(string location);

        public abstract void VolumeUp();

        public abstract void VolumeDown();

        public abstract void FullScreen();
    }
}