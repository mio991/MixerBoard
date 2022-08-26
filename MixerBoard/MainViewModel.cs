using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using Windows.Win32;
using Windows.Win32.System.Com.StructuredStorage;
using Windows.Win32.Media.Audio.Endpoints;
using Windows.Win32.Media.Audio;

namespace MixerBoard
{
        public class MainViewModel : INotifyPropertyChanged, IDisposable
        {
                private Board? board = null;
                private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

                public string? SelectedPort
                {
                        get => board?.PortName;
                        set => ConnectTo(value);
                }
                public IEnumerable<string> Ports { get; private set; }
                public ObservableCollection<Chanel> Channels { get; } = new ObservableCollection<Chanel>();
                public IReadOnlyList<IAudioSession> Sessions { get; }

                public event PropertyChangedEventHandler? PropertyChanged;

                public MainViewModel()
                {
                        Ports = SerialPort.GetPortNames();
                       
                        var device = API.CreateIMMDeviceEnumerator()
                                ?.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole)
                                ?? throw new Exception("No Device found!");

                        var masterVolume = device.GetAudioEndpointVolume() ?? throw new InvalidOperationException($"Could not create {nameof(IAudioEndpointVolume)}");
                        var manager = device.GetAudioSessionManager2() ?? throw new InvalidOperationException($"Could not create {nameof(IAudioSessionManager2)}");

                        var sessions = manager.GetSessionEnumerator();

                        sessions.GetCount(out int sessionCount);

                        List<IAudioSession> collection = new List<IAudioSession>(sessionCount + 1)
                        {
                                new MasterVolume(masterVolume)
                        };

                        for (int i = 0; i < sessionCount; i++)
                        {
                                sessions.GetSession(i, out var session);
                                collection.Add(new AudioSession(session));
                        }

                        Sessions = collection;
                }                

                private void ConnectTo(string? value)
                {
                        board?.Dispose();

                        if (value == null)
                        {
                                board = null;
                        }
                        else
                        {
                                board = new Board(value);
                                board.Open();
                                board.DataChanged += (data) =>
                                {
                                        dispatcher.Invoke(() =>
                                        {
                                                while (Channels.Count > data.Length)
                                                {
                                                        Channels.RemoveAt(Channels.Count - 1);
                                                }

                                                while (Channels.Count < data.Length)
                                                {
                                                        Channels.Add(new Chanel(Sessions));
                                                }

                                                for (int i = 0; i < data.Length; i++)
                                                {
                                                        Channels[i].ChangeVolume(data[i] / 1023f );
                                                }
                                        });
                                };
                        }
                }

                public void Dispose()
                {
                        board?.Close();
                        board?.Dispose();
                }
        }
}
