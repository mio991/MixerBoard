using System;
using System.ComponentModel;

namespace MixerBoard
{
        public interface IAudioSession : IDisposable, INotifyPropertyChanged
        {
                string DisplayName { get; }
                float Volume { get; }

                void SetVolume(float volume);
        }
}
