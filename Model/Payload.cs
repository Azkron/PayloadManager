using System;
using System.Collections.Generic;

namespace PayloadManager.Model
{
    public class Payload
    {
        public int load { get; set; }
        public Dictionary<string, float> fuels { get; set; }
        public Powerplant[] powerPlants { get; set; }
    }
}