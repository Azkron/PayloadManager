using System;
using System.Collections.Generic;

namespace PowerAssinger.Model
{
    public class PowerRequest
    {
        public int load { get; set; }
        public Dictionary<string, float> fuels { get; set; }
        public Powerplant[] powerPlants { get; set; }
    }
}