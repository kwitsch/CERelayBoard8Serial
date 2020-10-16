using System;
using System.Collections.Generic;

namespace CERelayBoard8Serial
{
    public class Factory
    {
        #region Singelton
        private static Lazy<Factory> _Instance;
        public static Factory Instance => _Instance.Value;

        static Factory()
        {
            _Instance = new Lazy<Factory>();
        }
        #endregion

        private readonly Lazy<Dictionary<string, Controller>> _ControllerCache;
        private Factory()
        {
            _ControllerCache = new Lazy<Dictionary<string, Controller>>();
        }

        public Controller GetController(string port)
        {
            if(_ControllerCache.Value.ContainsKey(port))
            {
                return _ControllerCache.Value[port];
            }
            else
            {
                var controller = new Controller(port);
                _ControllerCache.Value.Add(port, controller);
                return controller;
            }
        }
    }
}
