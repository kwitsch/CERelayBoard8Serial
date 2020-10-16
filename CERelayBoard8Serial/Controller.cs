using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SerialPortLibNetCore;
using CERelayBoard8Serial.Utils;

namespace CERelayBoard8Serial
{
    public class Controller
    {
        private readonly Lazy<SerialPortInput> _Serial;
        private readonly WaitForSilence _SilenceWaiter = new WaitForSilence();
        public readonly Lazy<Dictionary<ushort,Board>> Boards = new Lazy<Dictionary<ushort,Board>>();
        private readonly string _Port;
        private bool _Initialized;

        internal Controller(string port)
        {
            _Port = port;
            _Initialized = false;
            _Serial = new Lazy<SerialPortInput>(new SerialPortInput(new LoggerFactory().CreateLogger<SerialPortInput>()));
        }

        public async Task<bool> Init()
        {
            if (!_Initialized)
            {
                _Serial.Value.SetPort(_Port,
                                      19200,
                                      RJCP.IO.Ports.StopBits.One,
                                      RJCP.IO.Ports.Parity.None,
                                      DataBits.Eight);
                if (_Serial.Value.Connect())
                {
                    //setup & collect boards
                    _Serial.Value.MessageReceived += Setup_MessageReceived;
                    SendMessage(SendCommand.SETUP, 1, 0);
                    await _SilenceWaiter.Wait(1500);
                    _Serial.Value.MessageReceived -= Setup_MessageReceived;
                    if (Boards.Value.Count > 0)
                    {
                        //disable all
                        SendMessage(SendCommand.SET_PORT, 0, 0);
                        //register message reciever
                        _Serial.Value.MessageReceived += Serial_MessageReceived;
                        //prevent double initialization
                        _Initialized = true;
                    }
                    else //no boaards detected
                    {
                        if(_Serial.Value.IsConnected)
                        {
                            _Serial.Value.Disconnect();
                        }
                    }
                }
            }
            return _Initialized;
        }

        private bool SendMessage(SendCommand command, ushort address, byte data)
        {
            if (_Serial.Value.IsConnected)
            {
                var message = new byte[4];
                message[0] = (byte)command;
                message[1] = (byte)address;
                message[2] = data;
                message[3] = (byte)((ushort)command ^ address ^ data);

                return _Serial.Value.SendMessage(message);
            }
            return false;
        }


        private void Setup_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            if((RecieveCommand)args.Data[0]==RecieveCommand.SETUP)
            {
                _SilenceWaiter.Reset();
                var address = (ushort)args.Data[1];
                if (!Boards.Value.ContainsKey(address))
                {
                    var b = new Board(address);
                    b.RequestSendMessage += RequestSendMessage;
                    Boards.Value.Add(address, b);
                }
            }
        }

        private void RequestSendMessage(object sender, RequestSendMessageEventArgs e)
        {
            SendMessage(e.Command, e.Address, e.Data);
        }

        private void Serial_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            var command = (RecieveCommand)args.Data[0];
            var address = (ushort)args.Data[1];
            var data = args.Data[2];
            if (Boards.Value.ContainsKey(address))
            {
                if (command == RecieveCommand.GET_PORT)
                {
                    Boards.Value[address].ByteToData(data);
                }
            }
        }

    }
}
