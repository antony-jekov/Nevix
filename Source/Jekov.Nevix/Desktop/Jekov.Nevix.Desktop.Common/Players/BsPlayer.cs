namespace Jekov.Nevix.Desktop.Common.Players
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
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
        }

        public override void StepBackward()
        {
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.LEFT);
        }

        public override void FastForward()
        {
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL, VirtualKeyCode.F5);
        }

        public override void Rewind()
        {
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL, VirtualKeyCode.F6);
        }

        public override void OpenFile(string location)
        {
            Thread.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.VK_L);
            Thread.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.DELETE);
            Thread.Sleep(DELAY_TIME << 4);
            input.Keyboard.Sleep(DELAY_TIME << 4);
            input.Keyboard.TextEntry(location);            
            Thread.Sleep(DELAY_TIME << 4);
            input.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }

        public override void VolumeUp()
        {
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.UP);
        }

        public override void VolumeDown()
        {
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.DOWN);
        }

        public override void FullScreen()
        {
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.VK_F);
        }

        public override void Play()
        {
            input.Keyboard.Sleep(DELAY_TIME);
            input.Keyboard.KeyPress(VirtualKeyCode.VK_X);
        }
    }
}