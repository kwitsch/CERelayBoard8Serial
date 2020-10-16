using System;

namespace CERelayBoard8Serial.Utils
{
    public class RequestSendMessageEventArgs : EventArgs
    {
        public SendCommand Command { get; set; } 
        public ushort Address { get; set; }
        public byte Data { get; set; }
    }
}
