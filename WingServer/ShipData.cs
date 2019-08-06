using UnityEngine;

namespace WingServer
   
{
    public class ShipData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float Speed { get; set; }

        public void SetRotationEuler(Vector3 rotation)
        {
            Rotation = Quaternion.Euler(rotation);
        }
    }
}