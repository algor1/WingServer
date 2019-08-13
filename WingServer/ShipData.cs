using MyQuaternions;
using UnityEngine;


namespace WingServer
   
{
    public class ShipData
    {
        public Vector3 Position { get; set; }
        public MyQuaternion Rotation { get; set; } = new MyQuaternion(0, 0, 0, 1);
        public float Speed { get; set; }
        public float SpeedMax { get; set; }
        public float Aceleration { get; set; }
        public float RotationSpeed { get; set; }
        public Vector3 RotationTarget { get; set; }
        public float RotationAcceleration { get; set; }
        public float RotationSpeedMax { get; set; }
        public float RotationAccelerationMax { get; set; }
        public float ShieldHp { get; set; }
        public float ArmorHp { get; set; }
    }
}