using System.Collections.Generic;
using System.ComponentModel;

namespace MixerBoard
{
        public class Chanel : INotifyPropertyChanged
        {
                public IReadOnlyList<IAudioSession> Sessions { get; }

                public IAudioSession? Session { get; set; }

                public event PropertyChangedEventHandler? PropertyChanged;

                public Chanel(IReadOnlyList<IAudioSession> sessions)
                {
                        Sessions = sessions;
                }

                public void ChangeVolume(float volume)
                        => Session?.SetVolume(volume);
        }
}
