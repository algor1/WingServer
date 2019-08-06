using System;

namespace WingServer
{
    public class Ship
    {
        public ShipData Data { get; set; }

        public Ship (EventHandler serverTick, ShipData shipData)
        {
            Data = shipData;
            serverTick += Tick(this,new EventArgs());
        }

        private void Tick(Object sender,EventArgs e)
        {

        }

    }
}
