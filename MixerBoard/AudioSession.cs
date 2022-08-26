using System;
using System.ComponentModel;
using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Media.Audio;

namespace MixerBoard
{
        internal class AudioSession : IAudioSession, IAudioSessionEvents
        {
                private readonly Guid guid = Guid.NewGuid();
                private readonly IAudioSessionControl audioSessionControl;
                private readonly ISimpleAudioVolume simpleAudioVolume;

                public string DisplayName { get; private set; }
                public float Volume { get; private set; }

                public event PropertyChangedEventHandler? PropertyChanged;
                public event Action<AudioSession>? Disconnected;

                public AudioSession(IAudioSessionControl audioSessionControl)
                {
                        audioSessionControl.GetDisplayName(out var name);
                        DisplayName = name.ToString();

                        if (audioSessionControl is IAudioSessionControl2 control2)
                        {
                                control2.GetProcessId(out var processId);
                                DisplayName = Process.GetProcessById(BitConverter.ToInt32(BitConverter.GetBytes(processId)))
                                        .ProcessName;
                                audioSessionControl.SetDisplayName(DisplayName, guid);
                        }

                        audioSessionControl.RegisterAudioSessionNotification(this);
                        this.audioSessionControl = audioSessionControl;

                        simpleAudioVolume = (ISimpleAudioVolume)audioSessionControl;

                        simpleAudioVolume.GetMasterVolume(out float volume);
                        Volume = volume;
                }

                public void Dispose()
                {
                        audioSessionControl.UnregisterAudioSessionNotification(this);
                }

                public void SetVolume(float volume)
                {
                        simpleAudioVolume.SetMasterVolume(volume, guid);
                }

                public unsafe void OnDisplayNameChanged(PCWSTR NewDisplayName, Guid* EventContext)
                {
                        DisplayName = NewDisplayName.ToString();
                        PropertyChanged.Raise(this, nameof(DisplayName));
                }

                public unsafe void OnIconPathChanged(PCWSTR NewIconPath, Guid* EventContext) { }

                public unsafe void OnSimpleVolumeChanged(float NewVolume, BOOL NewMute, Guid* EventContext)
                {
                        Volume = NewVolume;
                        PropertyChanged.Raise(this, nameof(Volume));
                }

                public unsafe void OnChannelVolumeChanged(uint ChannelCount, float* NewChannelVolumeArray, uint ChangedChannel, Guid* EventContext) { }

                public unsafe void OnGroupingParamChanged(Guid* NewGroupingParam, Guid* EventContext) { }

                public void OnStateChanged(AudioSessionState NewState) { }

                public void OnSessionDisconnected(AudioSessionDisconnectReason DisconnectReason)
                        => Disconnected?.Invoke(this);
        }
}
