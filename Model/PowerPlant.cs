using System;
using System.Collections.Generic;

namespace PowerAssinger.Model
{
    public class Powerplant
    {
        public string name { get; set; }
        public string type { get; set; }
        public float efficiency { get; set; }
        public int pmin { get; set; }
        public int pmax { get; set; }
    }

    public class PowerplantInfo
    {

        private Powerplant powerplant;
        public string name => powerplant.name;
        public float costPerUnit;
        public int pmin;
        public int pmax;

        public PowerplantInfo(Powerplant powerplant)
        {
            this.powerplant = powerplant;
            pmin = powerplant.pmin;
            pmax = powerplant.pmax;
        }
    }
}
