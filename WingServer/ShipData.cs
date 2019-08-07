using MyQuaternions;
using UnityEngine;


namespace WingServer
   
{
    public class ShipData
    {
        public Vector3 Position { get; set; }
        public MyQuaternion Rotation { get; set; } = new MyQuaternion(0, 0, 0, 1);
        public float Speed { get; set; }


    }
}