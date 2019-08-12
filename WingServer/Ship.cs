using UnityEngine;
using MyQuaternions;
using System;
using System.Threading.Tasks;

namespace WingServer
{
    public class Ship
    {
        public ShipData Data { get; set; }
        public RotateState CurrentRotateState { get; set; } = RotateState.Stopped;

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
                    CurrentRotateState = RotateState.Rotating;
                    OnRotationStateChangeCall(RotateState.Rotating);
                }

                if (Data.RotationSpeed <= 0)
                {
                    Data.RotationSpeed = 0;
                    Data.RotationAcceleration = 0;
                    CurrentRotateState = RotateState.Stopped;
                    OnRotationStateChangeCall(RotateState.Stopped);
                }
            }
        }

        public void ChangeRotateState(RotateState rotateState)
        {
            if (rotateState==CurrentRotateState)
            {
                return;
            }
            switch (rotateState)
            {
                case (RotateState.Starting):
                    if (CurrentRotateState != RotateState.Stopped)
                        {
                            ChangeRotateState(RotateState.Stopped);
                        }
                    StartRotating();
                    CurrentRotateState = RotateState.Starting;
                    OnRotationStateChangeCall(RotateState.Starting);
                    break;

                case (RotateState.Rotating):
                    if (CurrentRotateState != RotateState.Starting)
                    {
                        ChangeRotateState(RotateState.Starting);
                    }

                    break;
                case (RotateState.Stopping):
                    if (CurrentRotateState == RotateState.Stopped)
                    {
                        return;
                    }
                    StopRotating();
                    CurrentRotateState = RotateState.Stopping;
                    OnRotationStateChangeCall(RotateState.Stopping);
                    break;
                case (RotateState.Stopped):
                    if (CurrentRotateState != RotateState.Stopping)
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
