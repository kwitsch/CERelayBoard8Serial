using System;
using System.Collections;
using System.Collections.Generic;
using CERelayBoard8Serial.Utils;

namespace CERelayBoard8Serial
{
    public class Board
    {
        private readonly Lazy<Dictionary<ushort, bool>> _Data;
        public readonly ushort Address;
        public event EventHandler<RequestSendMessageEventArgs> RequestSendMessage;
        #region Relay
        public bool R1 
        {
            get { return _Data.Value[1]; }
            set { SetData(1, value); } 
        }
        public bool R2
        {
            get { return _Data.Value[2]; }
            set { SetData(2, value); }
        }
        public bool R3
        {
            get { return _Data.Value[3]; }
            set { SetData(3, value); }
        }
        public bool R4
        {
            get { return _Data.Value[4]; }
            set { SetData(4, value); }
        }
        public bool R5
        {
            get { return _Data.Value[5]; }
            set { SetData(5, value); }
        }
        public bool R6
        {
            get { return _Data.Value[6]; }
            set { SetData(6, value); }
        }
        public bool R7
        {
            get { return _Data.Value[7]; }
            set { SetData(7, value); }
        }
        public bool R8
        {
            get { return _Data.Value[8]; }
            set { SetData(8, value); }
        }
        #endregion

        internal Board(ushort address)
        {
            Address = address;
            _Data = new Lazy<Dictionary<ushort, bool>>(new Dictionary<ushort, bool>
            {
                {1, false },
                {2, false },
                {3, false },
                {4, false },
                {5, false },
                {6, false },
                {7, false },
                {8, false },
            });
        }

        private void SetData(ushort relay, bool state)
        {
            _Data.Value[relay] = state;
            CallRequestSendMessage(SendCommand.SET_PORT, DataToByte());
        }

        private void CallRequestSendMessage(SendCommand command, byte data)
        {
            RequestSendMessage?.Invoke(this, new RequestSendMessageEventArgs
            {
                Command = command,
                Address = Address,
                Data = data,
            });
        }

        #region Converters
        private byte DataToByte()
        {
            var res = new byte[1];
            var d = new BitArray(new bool[]
            {
                _Data.Value[1],
                _Data.Value[2],
                _Data.Value[3],
                _Data.Value[4],
                _Data.Value[5],
                _Data.Value[6],
                _Data.Value[7],
                _Data.Value[8],
            });
            d.CopyTo(res, 0);
            return res[0];
        }

        internal void ByteToData(byte data)
        {
            var d = new BitArray(new byte[] { data });
            _Data.Value[1] = d[0];
            _Data.Value[2] = d[1];
            _Data.Value[3] = d[2];
            _Data.Value[4] = d[3];
            _Data.Value[5] = d[4];
            _Data.Value[6] = d[5];
            _Data.Value[7] = d[6];
            _Data.Value[8] = d[7];
        }
        #endregion
    }
}
