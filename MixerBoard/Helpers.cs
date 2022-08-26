using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MixerBoard
{
        internal static class Helpers
        {
                public static void Raise(this PropertyChangedEventHandler? handler, object sender, [CallerMemberName] string? propertyName = null)
                        => handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
}
