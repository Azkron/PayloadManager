using System;
using System.Collections.Generic;

namespace PayloadManager.Model
{
    public class Powerplant
    {
        public string name { get; set; }
        public string type { get; set; }
        public float efficiency { get; set; }
        public int pmin { get; set; }
        public int pmax { get; set; }
    }

    public class PowerAssingment
    {

        private Powerplant powerplant;
        public float costPerUnit;
        public int pmin;
        public int pmax;
        public int p;
        public AssingmentResult result => new AssingmentResult() { name = powerplant.name, p = p };

        public PowerAssingment(Powerplant powerplant)
        {
            this.powerplant = powerplant;
        }
    }

    public struct AssingmentResult
    {
        public string name { get; set; }
        public int p { get; set; }
    }
}
