namespace Windows.Win32.Media.Audio
{
        public static class IMMDeviceEnumeratorExtensions
        {
                public unsafe static IMMDevice? GetDefaultAudioEndpoint(
                        this IMMDeviceEnumerator enumerator,
                        EDataFlow dataFlow,
                        ERole role)
                {
                        enumerator.GetDefaultAudioEndpoint(dataFlow, role, out var ppEndpoint);
                        return ppEndpoint;
                }
        }
}