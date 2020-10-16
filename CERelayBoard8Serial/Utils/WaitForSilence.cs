using System.Threading.Tasks;

namespace CERelayBoard8Serial.Utils
{
    internal class WaitForSilence
    {
        private int _Awaiter = 0;
        private int _BaseAwaiter = 0;
        public async Task Wait(int ms)
        {
            _Awaiter = ms;
            _BaseAwaiter = ms;
            do
            {
                await Task.Delay(1);
                _Awaiter--;
            } 
            while (_Awaiter > 0);
        }
        public void Reset()
        {
            _Awaiter = _BaseAwaiter;
        }
    }
}
