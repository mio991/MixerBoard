using System.Runtime.InteropServices;

namespace Windows.Win32.Media.Audio
{
        internal static class IAudioSessionControlExtensions
        {
                /*
                public unsafe static ISimpleAudioVolume? GetSimpleAudioVolume(this IAudioSessionControl control)
                {
                        var ptr = Marshal.GetComInterfaceForObject<IAudioSessionControl, ISimpleAudioVolume>(control).ToPointer();

                        if (ptr != null)
                        {
                                return &(ISimpleAudioVolume*)ptr;
                        }
                        else
                        {
                                return null;
                        }
                }
                */
        }
}
