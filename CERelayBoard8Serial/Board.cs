using System;
using System.Collections.Generic;
using System.Text;

namespace CERelayBoard8Serial
{
    public class Board
    {
        private readonly Lazy<Dictionary<ushort, bool>> _Data;
        public readonly ushort Address;
        
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

        }
    }
}
