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
        [TestCase(1,0,0 , 0,90,0)]
        [TestCase(0,1,0, 270, 0, 0)]
        public void Rotation_RotationSpeed90NewTargetLocation_CorrectRotation(float targetLocationX, float targetLocationY, float targetLocationZ,
                                                                              float expectedRotationX, float expectedRotationY, float expectedRotationZ)
        {
            Vector3 targetLocation = new Vector3( targetLocationX,  targetLocationY, targetLocationZ);

            ShipData shipData = new ShipData();
            shipData.RotationSpeed=90;
            shipData.RotationTarget = targetLocation;

            Ship sut = new Ship(shipData);
            sut.Tick();
            Vector3 actual = sut.Data.Rotation.eulerAngles;
            Assert.That(actual.x, Is.EqualTo(expectedRotationX).Within(0.1), "Wrong value in X");
            Assert.That(actual.y, Is.EqualTo(expectedRotationY).Within(0.1), "Wrong value in Y");
            Assert.That(actual.z, Is.EqualTo(expectedRotationZ).Within(0.1), "Wrong value in Z");
        }
        [Test]
        [TestCase(0,10,10)]
        [TestCase(0, 30, 20)]
        public void RotationAccelerationChangesRotationSpeed_RotationAcceleration_CorrectValue(float startRotationSpeed, float rotationAcceleration, float expectedRotationSpeed)
        {
            ShipData shipData = new ShipData();
            shipData.RotationSpeed = startRotationSpeed;
            shipData.RotationAcceleration = rotationAcceleration;
            shipData.RotationSpeedMax = 20f;

            Ship sut = new Ship(shipData);
            sut.Tick();
            Assert.That(sut.Data.RotationSpeed, Is.EqualTo(expectedRotationSpeed).Within(0.01));
        }
        [Test]
        [TestCase(RotateState.Stopped,RotateState.Starting, RotateState.Starting,0,10)]
        [TestCase(RotateState.Stopped, RotateState.Rotating, RotateState.Starting,0,10)]
        [TestCase(RotateState.Rotating, RotateState.Stopped, RotateState.Stopping,0,-10)]
        [TestCase(RotateState.Starting, RotateState.Stopped, RotateState.Stopping,10,-10)]
        [TestCase(RotateState.Stopped, RotateState.Stopping, RotateState.Stopped,0,0 )]
        public void ChangeRotateState_NewState_CorrectRotateState(RotateState state, RotateState newState, RotateState expectedState,
                                                                    float rotationAcceleration, float expectedRotationAcceleration)
        {
            ShipData shipData = new ShipData();
            shipData.RotationAcceleration = rotationAcceleration;
            shipData.RotationAccelerationMax = 10;
            RotateState activeState;
            Ship sut = new Ship(shipData);
            sut.CurrentRotateState = state;
            sut.ChangeRotateState(newState);
            sut.RotationStateChanged += (s, args) => activeState = args.rotateState;

            Assert.That(sut.Data.RotationAcceleration, Is.EqualTo(expectedRotationAcceleration).Within(0.001), $"Rotate acceleration is wrong actual {sut.Data.RotationAcceleration} exprected {expectedRotationAcceleration} ");
            Assert.That(sut.CurrentRotateState, Is.EqualTo(expectedState),$"Wrong State after changin from {state} to {newState} expected {expectedState}");

        }
        #region hitpoints

        [Test]
        [TestCase(150,0,50)]
        [TestCase(300, 0, 0)]
        [TestCase(50, 50, 100)]
        public void DamegeShildArmor_CorrectHitpointsDamage_CorrectState(float damage, float expectedShieldHp, float expectedArmorHp)
        {
            ShipData shipData = new ShipData();
            shipData.ShieldHp = 100f;
            shipData.ArmorHp = 100f;
            Ship sut = new Ship(shipData);
            sut.Damage(damage);
            Assert.That(sut.Data.ArmorHp, Is.EqualTo(expectedArmorHp),$"wrong Armor hitpoints {sut.Data.ArmorHp} expected {expectedArmorHp}" );
            Assert.That(sut.Data.ShieldHp, Is.EqualTo(expectedShieldHp), $"wrong Armor hitpoints {sut.Data.ShieldHp} expected {expectedShieldHp}");
        }
        [Test]
        public void Damage_Criticaldamage_DestroyedEventRaised()
        {
            ShipData shipData = new ShipData();
            shipData.ShipId = 10;
            shipData.ShieldHp = 100f;
            shipData.ArmorHp = 100f;

            Ship sut = new Ship(shipData);
            int shipId = 0;
            sut.ShipDestroyed += (s, args) => shipId = args.ShipId;
            sut.Damage(250);
            Assert.That(shipId, Is.EqualTo(10));
        }


        #endregion
    }
}
