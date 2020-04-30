using PayloadManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayloadManager.Components
{
    public static class PayloadProcessor
    {
        private static Dictionary<string,
            Func<Powerplant, Dictionary<string, float>, PowerAssingment>> _assigners;
        public static void _Init()
        {
            _assigners = new Dictionary<string,
                Func<Powerplant, Dictionary<string, float>, PowerAssingment>>();

            _assigners["gasfired"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerAssingment assingment = new PowerAssingment(plant);
                assingment.costPerUnit = fuels["gas(euro/MWh)"] * (1f / plant.efficiency);
                assingment.costPerUnit += fuels["co2(euro/ton)"] * 0.3f;
                return assingment;
            };

            _assigners["turbojet"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerAssingment assingment = new PowerAssingment(plant);
                assingment.costPerUnit = fuels["kerosine(euro/MWh)"] * (1f / plant.efficiency);
                return assingment;
            };

            _assigners["windturbine"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerAssingment assingment = new PowerAssingment(plant);
                assingment.pmax = (int)(plant.pmax * fuels["wind(%)"]);
                return assingment;
            };
        }

        public static AssingmentResult[] Process(Payload payload)
        {
            List<PowerAssingment> powerAssingments = new List<PowerAssingment>();
            foreach (Powerplant p in payload.powerPlants)
                powerAssingments.Add(_assigners[p.type](p, payload.fuels));


            AssingmentResult[] results = new AssingmentResult[powerAssingments.Count];
            for (int i = 0; i < powerAssingments.Count; ++i)
                results[i] = powerAssingments[i].result;

            return results;
        }
    }
}
