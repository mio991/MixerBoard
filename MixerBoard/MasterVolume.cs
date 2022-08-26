using System;
using System.ComponentModel;
using Windows.Win32;
using Windows.Win32.Media.Audio;
using Windows.Win32.Media.Audio.Endpoints;

namespace MixerBoard
{
        internal class MasterVolume : IAudioSession, IAudioEndpointVolumeCallback, INotifyPropertyChanged
        {
                private readonly Guid guid = Guid.NewGuid();
                private readonly IAudioEndpointVolume audioEndpointVolume;

                public string DisplayName => "Master";
                public float Volume { get; private set; }

                public event PropertyChangedEventHandler? PropertyChanged;

                public MasterVolume(IAudioEndpointVolume audioEndpointVolume)
                {
                        this.audioEndpointVolume = audioEndpointVolume;
                        audioEndpointVolume.RegisterControlChangeNotify(this);
                }

                public void Dispose()
                {
                        audioEndpointVolume.UnregisterControlChangeNotify(this);
                }

                public void SetVolume(float volume)
                {
                        audioEndpointVolume.SetMasterVolumeLevelScalar(volume, guid);
                }

                public unsafe void OnNotify(AUDIO_VOLUME_NOTIFICATION_DATA* pNotify)
                {
                        Volume = pNotify->fMasterVolume;
                        PropertyChanged.Raise(this, nameof(Volume));
                }
        }
}
