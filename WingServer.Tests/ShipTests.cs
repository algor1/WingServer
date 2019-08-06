using System;
using NUnit.Framework;
using WingServer;

namespace WingServer.Tests
{
    [TestFixture]
    public class ShipTests
    {
        [Test]
        public void Move_Velocity1_directionX_CorrectDestination()
        {
            

            Ship sut = new Ship(serverTick,new ShipData());
            sut.Data.Velocity = 1;
            serverTick?.Invoke(this,new EventArgs());
           
        }

        private event EventHandler serverTick;

    }
}
