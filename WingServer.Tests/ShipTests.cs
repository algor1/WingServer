using System;
using NUnit.Framework;
using WingServer;
using UnityEngine;

namespace WingServer.Tests
{
    [TestFixture]
    public class ShipTests
    {
        [Test]
        [TestCase (1,0,0 , 1,0,0)]
        [TestCase (0,1,0 , 0,1,0)]
        [TestCase (0,0,1 , 0,0,1)]
        public void Move_Velocity1_directionX_CorrectDestination(float directionx, float directiony, float directionz, float expectedx , float expectedy, float expectedz)
        {
            Vector3 direction = new Vector3(directionx, directiony, directionz);
            
            Ship sut = new Ship(new ShipData());
            sut.Data.Speed = 1f;
            sut.Tick();
            Vector3 actual = sut.Data.Position;
            Assert.That(actual.x, Is.EqualTo(expectedx));
            Assert.That(actual.y, Is.EqualTo(expectedy));
            Assert.That(actual.z, Is.EqualTo(expectedz));
        }
    }
}
