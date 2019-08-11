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
                if (Data.RotationSpeed >= Data.RotationSpeedMax)
                {
                    Data.RotationSpeed = Data.RotationSpeedMax;
                    Data.RotationAcceleration = 0;
                    ChangeRotateState(RotateState.Rotating);
                }

                if (Data.RotationSpeed <= 0)
                {
                    Data.RotationSpeed = 0;
                    Data.RotationAcceleration = 0;
                    ChangeRotateState(RotateState.Stopped);
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
                    OnRotationStateChangeCall(RotateState.Starting);
                    break;

                case (RotateState.Rotating):
                    if (_rotateState != RotateState.Starting)
                    {
                        ChangeRotateState(RotateState.Starting);
                    }
                    OnRotationStateChangeCall(RotateState.Rotating);
                    break;
                case (RotateState.Stopping):
                    if (_rotateState != RotateState.Rotating)
                    {
                        ChangeRotateState(RotateState.Rotating);
                    }
                    StopRotating();
                    OnRotationStateChangeCall(RotateState.Stopping);
                    break;
                case (RotateState.Stopped):
                    if (_rotateState != RotateState.Stopping)
                    {
                        ChangeRotateState(RotateState.Stopping);
                    }
                    OnRotationStateChangeCall(RotateState.Stopped);
                    break;
            }
        }
        private void StartRotating()
        {
            Data.RotationAcceleration = Data.RotationAccelerationMax;
        }

        private void StopRotating()
        {
            Data.RotationAcceleration = -Data.RotationAccelerationMax;
        }


        #region events
        public event EventHandler<RotateStateMashineArgs> RotationStateChanged;

        protected virtual void OnRotationStateChange(RotateStateMashineArgs e)
        {
            EventHandler<RotateStateMashineArgs> handler = RotationStateChanged;
        }
        private void OnRotationStateChangeCall(RotateState rotateState)
        {
            RotateStateMashineArgs args = new RotateStateMashineArgs();
            args.rotateState = rotateState;
            OnRotationStateChange(args);
        }
        #endregion

    }
    public enum RotateState { Stopped,Starting,Rotating,Stopping};

    public class RotateStateMashineArgs : EventArgs
    {
        public RotateState rotateState { get; set; }
    }
}
