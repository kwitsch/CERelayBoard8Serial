using System;
using System.Linq;
using System.IO.Ports;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CERelayBoard8Serial.Test
{
    class RelayTest
    {
        private readonly Lazy<string[]> _Ports;
        private readonly Lazy<bool> _HasPorts;

        public RelayTest()
        {
            _Ports = new Lazy<string[]>(SerialPort.GetPortNames());
            _HasPorts = new Lazy<bool>(_Ports.Value.Length > 0);
        }

        [Test]
        public async Task Initalization1()
        {
            var b = Factory.Instance.GetController("not a serial port");
            var res = await b.Init();
            Assert.IsFalse(res);
        }

        [Test]
        public async Task Initalization2()
        {
            if (_HasPorts.Value)
            {
                var res = false;
                foreach(var p in _Ports.Value)
                {
                    var b = Factory.Instance.GetController(_Ports.Value.First());
                    res = await b.Init();
                    if(res)
                    {
                        break;
                    }
                }
                
                Assert.IsTrue(res);
            }
            else
            {
                Assert.Fail("No serial port detected.");
            }
        }
    }
}
