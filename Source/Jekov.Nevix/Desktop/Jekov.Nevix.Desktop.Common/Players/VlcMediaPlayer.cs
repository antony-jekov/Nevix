using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace Jekov.Nevix.Desktop.Common.Players
{
    public class VlcMediaPlayer : Player
    {
        private const string VLC_PROCESS_NAME = "VLC media player";
        public VlcMediaPlayer(string playerLocation)
            : base(VLC_PROCESS_NAME, playerLocation)
        {
        }

        public override void OpenFile(string location)
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_O);
            Thread.Sleep(1000);
            input.Keyboard.TextEntry(location);
            Thread.Sleep(1000);
            input.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }
        public override void FullScreen()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VK_F);
        }

        public override void Play()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.SPACE);
        }

        public override void Pause()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.SPACE);
        }

        public override void Exit()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_Q);
        }

        public override void FastForward()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LSHIFT, VirtualKeyCode.OEM_1);
        }

        public override void Rewind()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.OEM_4);
        }

        public override void Stop()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VK_S);
        }

        public override void Next()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VK_N);
        }

        public override void Previous()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VK_S);
        }

        public override void VolumeUp()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.UP);
        }

        public override void VolumeDown()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.DOWN);
        }

        public override void StepBackward()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.LEFT);
        }

        public override void StepForward()
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.RIGHT);
        }
    }
}
