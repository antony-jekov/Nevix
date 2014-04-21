using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace Jekov.Nevix.Desktop.Common.Players
{
    public class GomPlayer : Player
    {
        private const string GOM_PROCESS_NAME = "GOM player";
        public GomPlayer(string playerLocation)
            : base(GOM_PROCESS_NAME, playerLocation)
        {
        }

        public override void Exit()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4);
        }

        public override void FullScreen()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);
        }

        public override void OpenFile(string location)
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_O);
            Thread.Sleep(500);
            base.OpenFile(location);
        }

        public override void Play()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.SPACE);
        }

        public override void Pause()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.SPACE);
        }

        public override void Stop()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.SPACE);
        }

        public override void VolumeUp()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.UP);
        }

        public override void VolumeDown()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.DOWN);
        }

        public override void Next()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.NEXT);
        }

        public override void Previous()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.PRIOR);
        }

        public override void FastForward()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VK_C);
        }

        public override void Rewind()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VK_X);
        }
    }
}
