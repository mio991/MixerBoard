using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.Media.Audio.Endpoints;
using static Windows.Win32.System.Com.CLSCTX;

namespace Windows.Win32.Media.Audio
{
        public static class MMDeviceExtensions
        {
                public unsafe static IAudioEndpointVolume? GetAudioEndpointVolume(this IMMDevice device)
                {
                        var guid = typeof(IAudioEndpointVolume).GUID;
                        device.Activate(
                                &guid,
                                CLSCTX_REMOTE_SERVER,
                                default,
                                out var obj);

                        if (obj is IAudioEndpointVolume audioEndpointVolume)
                        {
                                return audioEndpointVolume;
                        }
                        else
                        {
                                return null;
                        }
                }

                public unsafe static IAudioSessionManager2? GetAudioSessionManager2(this IMMDevice device)
                {
                        var guid = typeof(IAudioSessionManager2).GUID;
                        device.Activate(&guid,
                                CLSCTX_REMOTE_SERVER,
                                default,
                                out var obj);

                        if (obj is IAudioSessionManager2 sessionManager)
                        {
                                return sessionManager;
                        }
                        else
                        {
                                return null;
                        }
                }
        }
}
