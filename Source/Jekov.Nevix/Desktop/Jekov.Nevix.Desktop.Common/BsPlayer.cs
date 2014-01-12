namespace Jekov.Nevix.Desktop.Common
{
    using System;
    using System.Threading;
    using WindowsInput.Native;

    public class BsPlayer : Player
    {
        private const string BSPLAYER_PROCESS_NAME = "bsplayer";
        private const int DELAY_TIME = 50;

        public BsPlayer(string bsplayerLocation)
            : base(BSPLAYER_PROCESS_NAME, bsplayerLocation)
        {
        }

        public override void StepForward()
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
        }

        public override void StepBackward()
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.LEFT);
        }

        public override void FastForward()
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL, VirtualKeyCode.F5);
        }

        public override void Rewind()
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL, VirtualKeyCode.F6);
        }

        public override void OpenFile(string location)
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.KeyPress(VirtualKeyCode.VK_L);
            Thread.Sleep(1000);
            input.Keyboard.TextEntry(location);
            Thread.Sleep(1000);
            input.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }

        public override void VolumeUp()
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.UP);
        }

        public override void VolumeDown()
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.DOWN);
        }

        public override void FullScreen()
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.VK_F);
        }

        public override void Play()
        {
            SetForegroundWindow(player.MainWindowHandle);
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.VK_X);
        }
    }
}