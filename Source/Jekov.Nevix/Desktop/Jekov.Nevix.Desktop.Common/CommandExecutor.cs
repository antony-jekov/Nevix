namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Desktop.Common.Contracts;
    using System;
    using System.Threading;

    public class CommandExecutor
    {
        private IPlayer player;

        private const string PLAY_CMD = "play";
        private const string PAUSE_CMD = "pause";
        private const string STOP_CMD = "stop";
        private const string NEXT_CMD = "next";
        private const string PREV_CMD = "prev";
        private const string STEP_F_CMD = "step_f";
        private const string STEP_B_CMD = "step_b";
        private const string VOLUME_UP_CMD = "volume_up";
        private const string SYS_VOLUME_UP_CMD = "sys_volume_up";
        private const string VOLUME_DOWN_CMD = "volume_down";
        private const string SYS_VOLUME_DOWN_CMD = "sys_volume_down";
        private const string MUTE_CMD = "mute";
        private const string POWER_CMD = "power";
        private const string FULL_CMD = "full";
        private const string FF_CMD = "ff";
        private const string RW_CMD = "rw";

        public CommandExecutor(IPlayer player)
        {
            this.player = player;
        }


        private void ExexuteCmd(string cmd)
        {
            switch (cmd)
            {
                case PLAY_CMD:
                    player.Play();
                    break;

                case PAUSE_CMD:
                    player.Pause();
                    break;

                case STOP_CMD:
                    player.Stop();
                    break;

                case NEXT_CMD:
                    player.Next();
                    break;

                case PREV_CMD:
                    player.Previous();
                    break;

                case STEP_B_CMD:
                    player.StepBackward();
                    break;

                case STEP_F_CMD:
                    player.StepForward();
                    break;

                case SYS_VOLUME_DOWN_CMD:
                    player.SystemVolumeDown();
                    break;

                case SYS_VOLUME_UP_CMD:
                    player.SystemVolumeUp();
                    break;

                case VOLUME_DOWN_CMD:
                    player.VolumeDown();
                    break;

                case VOLUME_UP_CMD:
                    player.VolumeUp();
                    break;

                case MUTE_CMD:
                    player.Mute();
                    break;

                case POWER_CMD:
                    player.Power();
                    break;

                case FULL_CMD:
                    player.FullScreen();
                    break;

                case FF_CMD:
                    player.FastForward();
                    break;

                case RW_CMD:
                    player.Rewind();
                    break;

                default:
                    throw new ArgumentException("cmd", "Unknown command type!");
            }
        }

        public void ExecuteCommand(string cmd)
        {
            Thread thread = new Thread(() => ExexuteCmd(cmd));
            thread.Start();
        }
    }
}