using System;
using System.Collections.Generic;
using System.Text;

namespace CERelayBoard8Serial
{
    public sealed class CERB8S_Factory
    {
        #region Singelton
        private static readonly Lazy<CERB8S_Factory> _Lazy = new Lazy<CERB8S_Factory>(() => new CERB8S_Factory());

        public static CERB8S_Factory Instance => _Lazy.Value;
        #endregion

        private CERB8S_Factory()
        {
        }

        public CERB8S_Controller GetController(string port)
        {
            return new CERB8S_Controller(port);
        }
    }
}
