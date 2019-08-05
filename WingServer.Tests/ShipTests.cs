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
            Ship sut = new Ship();
            sut.Data.Velocity = 1;
            
        }

    }
}
