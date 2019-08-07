using System;
using NUnit.Framework;
using WingServer;
using UnityEngine;
using MyQuaternions;

namespace WingServer.Tests
{
    [TestFixture]
    public class ShipTests
    {
        [Test]
        [TestCase (1,0,0 , 1,0,0)]
        [TestCase (0,0,1 , 0,0,1)]
        public void Move_Speed1Direction_CorrectDestination(float directionx, float directiony, float directionz, float expectedx , float expectedy, float expectedz)
        {
       
            MyQuaternion rotation = new MyQuaternion();
            Vector3 direction = new Vector3(directionx, directiony, directionz);
            Ship sut = new Ship(new ShipData());
            rotation.SetLookRotation(direction);
            sut.Data.Rotation = rotation;
                
            sut.Data.Speed = 1f;
            sut.Tick();
            Vector3 actual = sut.Data.Position;
            Assert.That(actual.x, Is.EqualTo(expectedx).Within(0.1),"Wrong value in X");
            Assert.That(actual.y, Is.EqualTo(expectedy).Within(0.1),"Wrong value in Y");
            Assert.That(actual.z, Is.EqualTo(expectedz).Within(0.1), "Wrong value in Z");
        }
    }
}
