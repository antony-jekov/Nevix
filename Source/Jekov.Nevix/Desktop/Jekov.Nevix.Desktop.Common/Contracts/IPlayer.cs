namespace Jekov.Nevix.Desktop.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPlayer
    {
        void Play();
        void Pause();
        void Stop();
        void VolumeUp();
        void SystemVolumeUp();
        void VolumeDown();
        void SystemVolumeDown();
        void Mute();
        void Next();
        void Previous();
        void StepForward();
        void StepBackward();
        void FastForward();
        void Rewind();
        void OpenFile(string location);
        void Power();
        void FullScreen();
    }
}
