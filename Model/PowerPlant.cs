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

        public PowerplantInfo(Powerplant powerplant, Dictionary<string, float> fuels)
        {
            this.powerplant = powerplant;
            pmin = powerplant.pmin;
            pmax = powerplant.pmax;
            Init(fuels);
        }

        private void Init(Dictionary<string, float> fuels)
        {
            if (powerplant.type == "gasfired")
            {
                costPerUnit = fuels["gas(euro/MWh)"] * (1f / powerplant.efficiency);
                costPerUnit += fuels["co2(euro/ton)"] * 0.3f;
            }
            else if (powerplant.type == "turbojet")
                costPerUnit = fuels["kerosine(euro/MWh)"] * (1f / powerplant.efficiency);
            else if (powerplant.type == "windturbine")
                pmax = (int)(powerplant.pmax * (fuels["wind(%)"] / 100f));
        }
    }
}
