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
        [TestCase(1, 0, 0, 1, 0, 0)]
        [TestCase(0, 0, 1, 0, 0, 1)]
        public void Move_Speed1Direction_CorrectDestination(float directionx, float directiony, float directionz, float expectedx, float expectedy, float expectedz)
        {

            MyQuaternion rotation = new MyQuaternion();
            Vector3 direction = new Vector3(directionx, directiony, directionz);
            rotation.SetLookRotation(direction);

            ShipData shipData = new ShipData();
            shipData.Rotation = rotation;
            shipData.Speed = 1f;

            Ship sut = new Ship(shipData);
                       
            sut.Tick();
            Vector3 actual = sut.Data.Position;
            Assert.That(actual.x, Is.EqualTo(expectedx).Within(0.1), "Wrong value in X");
            Assert.That(actual.y, Is.EqualTo(expectedy).Within(0.1), "Wrong value in Y");
            Assert.That(actual.z, Is.EqualTo(expectedz).Within(0.1), "Wrong value in Z");
        }

        [Test]
        [TestCase (0,3,3)]
        [TestCase(0, 20, 10)]
        [TestCase(10, -4, 6)]
        public void Accelerate_SpeedAcceleration_CorrectSpeed(float startSpeed, float aceleration,float expectedSpeed)
            {
            ShipData shipData = new ShipData();
            shipData.Aceleration = aceleration;
            shipData.Speed = startSpeed;
            shipData.SpeedMax = 10;
            Ship sut = new Ship(shipData);
            sut.Tick();

            Assert.That(sut.Data.Speed, Is.EqualTo(expectedSpeed).Within(0.01));
    }

        [Test]
        [TestCase(0,0,1)]
        public void Rotation_RotationSpeed90NewDirection_CorrectRotation(float rtx,float rty, float rtz)
        {
            Vector3 rotationTarget = new Vector3(rtx, rty, rtz);
            ShipData shipData = new ShipData();
            shipData.RotationSpeed=10;
            shipData.RotationTarget = rotationTarget;

            Ship sut = new Ship(shipData);
            sut.Tick();
            Vector3 actual = sut.Data.Rotation.eulerAngles;
            Assert.That(actual.x, Is.EqualTo(rtx).Within(0.1), "Wrong value in X");
            Assert.That(actual.y, Is.EqualTo(rty).Within(0.1), "Wrong value in Y");
            Assert.That(actual.z, Is.EqualTo(rtz).Within(0.1), "Wrong value in Z");
        }
    }
}
