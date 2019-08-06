using System;
using UnityEngine;

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
        }

        private void Move()
        {
            if (Data.Speed != 0)
            {
                Data.Position += Data.Rotation * Vector3.forward* Data.Speed;
            }
        }
    }
}
