using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Media.Audio;
using Windows.Win32.System.Com;

namespace MixerBoard
{
        public interface IAudioSession : IDisposable
        {
                string DisplayName { get; }
                float Volume { get; }

                void SetVolume(float volume);
        }

        internal class AudioSession : IAudioSession, IAudioSessionEvents, INotifyPropertyChanged
        {
                private readonly Guid guid = Guid.NewGuid();
                private readonly IAudioSessionControl audioSessionControl;

                public string DisplayName { get; private set; }
                public float Volume { get; private set; }

                public event PropertyChangedEventHandler? PropertyChanged;
                public event Action<AudioSession>? Disconnected;

                public AudioSession(IAudioSessionControl audioSessionControl)
                {
                        audioSessionControl.GetDisplayName(out var name);
                        DisplayName = name.ToString();

                        audioSessionControl.RegisterAudioSessionNotification(this);
                        this.audioSessionControl = audioSessionControl;                        
                }

                public void Dispose()
                {
                        audioSessionControl.UnregisterAudioSessionNotification(this);
                }

                public void SetVolume(float volume)
                {
                }

                public unsafe void OnDisplayNameChanged(PCWSTR NewDisplayName, Guid* EventContext)
                {
                        DisplayName = NewDisplayName.ToString();
                        PropertyChanged.Raise(this, nameof(DisplayName));
                }

                public unsafe void OnIconPathChanged(PCWSTR NewIconPath, Guid* EventContext) { }

                public unsafe void OnSimpleVolumeChanged(float NewVolume, BOOL NewMute, Guid* EventContext)
                {
                        if (guid != *EventContext)
                        {
                                Volume = NewVolume;
                                PropertyChanged.Raise(this, nameof(Volume));
                        }

                }

                public unsafe void OnChannelVolumeChanged(uint ChannelCount, float* NewChannelVolumeArray, uint ChangedChannel, Guid* EventContext) { }

                public unsafe void OnGroupingParamChanged(Guid* NewGroupingParam, Guid* EventContext) { }

                public void OnStateChanged(AudioSessionState NewState) { }

                public void OnSessionDisconnected(AudioSessionDisconnectReason DisconnectReason)
                        => Disconnected?.Invoke(this);
        }
}
