using CERelayBoard8Serial.Utils;
using SerialPortLibNetCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CERelayBoard8Serial
{
    public class Controller
    {
        private readonly Lazy<SerialPortInput> _Serial = new Lazy<SerialPortInput>();
        private readonly WaitForSilence _SilenceWaiter = new WaitForSilence();
        public readonly Lazy<Dictionary<ushort,Board>> Boards = new Lazy<Dictionary<ushort,Board>>();
        private readonly string _Port;

        internal Controller(string port)
        {
            _Port = port;
        }


        public async Task Init()
        {
            _Serial.Value.SetPort(_Port,
                                  19200,
                                  RJCP.IO.Ports.StopBits.One,
                                  RJCP.IO.Ports.Parity.None,
                                  DataBits.Eight);
            _Serial.Value.Connect();
            _Serial.Value.MessageReceived += Setup_MessageReceived;
            SendMessage(SendCommand.SETUP, 1, 0);
            await _SilenceWaiter.Wait(1500);
            _Serial.Value.MessageReceived -= Setup_MessageReceived;
        }

        private bool SendMessage(SendCommand command, ushort address, ushort data)
        {
            if (_Serial.Value.IsConnected)
            {
                var message = new byte[4];
                message[0] = (byte)command;
                message[1] = (byte)address;
                message[2] = (byte)data;
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
                    Boards.Value.Add(address, new Board(address));
                }
            }
        }

    
    }
}
