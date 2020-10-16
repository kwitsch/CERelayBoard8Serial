using NUnit.Framework;
using CERelayBoard8Serial;

namespace CERelayBoard8Serial.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            var test1 = CERB8S_Factory.Instance.GetController("test");
            var test2 = new CERB8S_Controller();
        }
    }
}