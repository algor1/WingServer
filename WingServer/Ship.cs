using UnityEngine;
using MyQuaternions;

namespace WingServer
{
    public class Ship
    {
        public ShipData Data { get; set; }

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
    }
}
