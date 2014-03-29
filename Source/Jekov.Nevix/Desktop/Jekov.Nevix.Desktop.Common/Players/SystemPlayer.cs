using Jekov.Nevix.Desktop.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace Jekov.Nevix.Desktop.Common.Players
{
    public class SystemPlayer : IPlayer
    {
        protected InputSimulator input;

        public SystemPlayer()
        {
            input = new InputSimulator();
        }

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

        public virtual void VolumeUp()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VOLUME_UP);
        }

        public void SystemVolumeUp()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VOLUME_UP);
        }

        public virtual void VolumeDown()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VOLUME_DOWN);
        }

        public void SystemVolumeDown()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VOLUME_DOWN);
        }

        public void Mute()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.VOLUME_MUTE);
        }

        public virtual void Next()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.MEDIA_NEXT_TRACK);
        }

        public virtual void Previous()
        {
            input.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PREV_TRACK);
        }

        public virtual void StepForward()
        {
            
        }

        public virtual void StepBackward()
        {
            
        }

        public virtual void FastForward()
        {
            
        }

        public virtual void Rewind()
        {
            
        }

        public virtual void OpenFile(string location)
        {
            Process.Start(location);
        }

        public virtual void Power()
        {
            Process.Start("shutdown", "/s /t 0");
        }

        public virtual void FullScreen()
        {
            
        }
    }
}
