using System.Runtime.Versioning;
using static Windows.Win32.System.Com.CLSCTX;

namespace Windows.Win32.Media.Audio
{
        public static class API
        {
                [SupportedOSPlatform("windows7.0")]
                public unsafe static IMMDeviceEnumerator? CreateIMMDeviceEnumerator()
                {
                        if (PInvoke.CoCreateInstance<IMMDeviceEnumerator>(
                                typeof(MMDeviceEnumerator).GUID,
                                null,
                                CLSCTX_INPROC_SERVER,
                                out var enumerator
                                ).Succeeded)
                        {
                                return enumerator;
                        }
                        else
                        {
                                return null;
                        }
                }
        }
}
