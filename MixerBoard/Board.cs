using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixerBoard
{
        internal class Board : IDisposable
        {
                private SerialPort serialPort;

                public string PortName => serialPort.PortName;

                public event Action<int[]>? DataChanged;

                public Board(string portName)
                {
                        serialPort = new SerialPort(portName);
                        serialPort.DiscardNull = true;
                        serialPort.DataReceived += SerialPort_DataReceived;
                }

                private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
                {
                        string line = serialPort.ReadLine();

                        var data = line.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                .Select(int.Parse)
                                .ToArray();

                        DataChanged?.Invoke(data); 
                }

                public void Open()
                {
                        serialPort.Open();
                }

                public void Close()
                {
                        serialPort.DataReceived -= SerialPort_DataReceived;
                        serialPort.Close();
                }

                public void Dispose()
                {
                        Close();
                        serialPort.Dispose();
                }
        }
}
