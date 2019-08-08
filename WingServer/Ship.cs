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
            //if (TargetToMove != null)
            //{
            //    MyQuaternion rotationToTarget = MyQuaternion.LookRotation(TargetToMove.Position - p.Position);
            //    //removeing z axis  
            //    rotationToTarget = MyQuaternion.Euler(rotationToTarget.eulerAngles.x, rotationToTarget.eulerAngles.y, zBeforeRotation);

            //    if (p.Rotation != rotationToTarget)
            //    {
            //        p.Rotation = MyQuaternion.RotateTowards(p.Rotation, rotationToTarget, p.RotationSpeed * TickDeltaTime / 1000f);
            //        Console.WriteLine("Ship {0} , rotation {1} target {2} rotation to target {3}", p.Id, p.Rotation.eulerAngles, TargetToMove.Id, rotationToTarget.eulerAngles);
            //    }
            //}

        }
    }
}
