using UnityEngine;
using MyQuaternions;
using System;

namespace WingServer
{
    public class Ship
    {
        public ShipData Data { get; set; }
        private RotateState _rotateState= RotateState.Stopped;

        public Ship (ShipData shipData)
        {
            Data = shipData;
        }

        public void Tick()
        {
            Move();
            Accelerate();
            Rotate();
            AccelerateRotation();
        }
        

        private void Move()
        {
            if (Data.Speed != 0)
            {
                Data.Position += Data.Rotation * Vector3.forward* Data.Speed;
            }
        }
        private void Accelerate()
        {
            if (Data.Aceleration != 0)
            {
                Data.Speed += Data.Aceleration;
                if (Data.Speed > Data.SpeedMax)
                {
                    Data.Speed = Data.SpeedMax;
                    Data.Aceleration = 0;
                }

                if (Data.Speed < 0)
                {
                    Data.Speed = 0;
                    Data.Aceleration = 0;
                }
            }
        }
        private void Rotate()
        {
            if (Data.RotationSpeed != 0)
            {
                MyQuaternion rotationToTarget = MyQuaternion.LookRotation(Data.RotationTarget - Data.Position);
                //removeing z axis  
                rotationToTarget = MyQuaternion.Euler(rotationToTarget.eulerAngles.x, rotationToTarget.eulerAngles.y, Data.Rotation.eulerAngles.z);
                Data.Rotation= MyQuaternion.RotateTowards(Data.Rotation, rotationToTarget, Data.RotationSpeed );
            }
        }
        private void AccelerateRotation()
        {
            if (Data.RotationAcceleration != 0)
            {
                Data.RotationSpeed += Data.RotationAcceleration;
                if (Data.RotationSpeed > Data.RotationSpeedMax)
                {
                    Data.RotationSpeed = Data.RotationSpeedMax;
                    Data.RotationAcceleration = 0;
                }

                if (Data.RotationSpeed < 0)
                {
                    Data.RotationSpeed = 0;
                    Data.RotationAcceleration = 0;
                }
            }
        }

        public void ChangeRotateState(RotateState rotateState)
        {
            switch (rotateState)
            {
                case (RotateState.Starting):
                    if (_rotateState != RotateState.Stopped)
                        {
                            ChangeRotateState(RotateState.Stopped);
                        }
                    StartRotating();
                    break;

                case (RotateState.Rotating):
                    if (_rotateState != RotateState.Starting)
                    {
                        ChangeRotateState(RotateState.Starting);
                    }
                    break;
                case (RotateState.Stopping):
                    if (_rotateState != RotateState.Rotating)
                    {
                        ChangeRotateState(RotateState.Rotating);
                    }
                    break;
                case (RotateState.Stopped):
                    if (_rotateState != RotateState.Stopping)
                    {
                        ChangeRotateState(RotateState.Stopping);
                    }
                    break;
            }
        }
        private void StartRotating()
        {
            Data.RotationAcceleration = Data.RotationAccelerationMax;
        }

    }
    public enum RotateState { Stopped,Starting,Rotating,Stopping};
}
