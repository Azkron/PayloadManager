using PayloadManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayloadManager.Components
{
    public static partial class PayloadSolver
    {
        private static Payload _payload;
        private static Dictionary<string,
            Func<Powerplant, Dictionary<string, float>, PowerplantInfo>> _infoBuilders;
        private static float _minAverageCost = 9999f;
        private static List<Node> _nodes;
        private static Node _lastNode;
        private static int _closestP;
        private static float _bestAverage;
        private static List<PowerplantInfo> _infos;
        public static void _Init()
        {
            _infoBuilders = new Dictionary<string,
                Func<Powerplant, Dictionary<string, float>, PowerplantInfo>>();

            _infoBuilders["gasfired"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerplantInfo info = new PowerplantInfo(plant);
                info.costPerUnit = fuels["gas(euro/MWh)"] * (1f / plant.efficiency);
                info.costPerUnit += fuels["co2(euro/ton)"] * 0.3f;
                return info;
            };

            _infoBuilders["turbojet"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerplantInfo info = new PowerplantInfo(plant);
                info.costPerUnit = fuels["kerosine(euro/MWh)"] * (1f / plant.efficiency);
                return info;
            };

            _infoBuilders["windturbine"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerplantInfo info = new PowerplantInfo(plant);
                info.pmax = (int)(plant.pmax * (fuels["wind(%)"] / 100f));
                return info;
            };
        }

        public static Assingment[] Process(Payload payload)
        {
            _payload = payload;
            _infos = new List<PowerplantInfo>();
            foreach (Powerplant p in payload.powerPlants)
                _infos.Add(_infoBuilders[p.type](p, payload.fuels));

            if(!PowerSurplus())
                return GetMaxAssingments();
            else
            {
                _infos.Sort((p1, p2) => p1.costPerUnit < p2.costPerUnit ? -1 : 1);
                Node node = AstarSearch(_infos);
                return node.GetAssingments();
            }
        }

        private static bool PowerSurplus()
        {
            int totalMaxP = 0;
            foreach (PowerplantInfo info in _infos)
                totalMaxP += info.pmax;
            return totalMaxP > _payload.load;
        }

        private static Assingment[] GetMaxAssingments()
        {
            Assingment[] assingments = new Assingment[_infos.Count];
            for (int i = 0; i < assingments.Length; ++i)
                assingments[i] = new Assingment() { name = _infos[i].name, p = _infos[i].pmax };
            return assingments;
        }

        private static void ResetGraph()
        {
            _closestP = _payload.load + 9999;
            _lastNode = null;
            _bestAverage = 9999f;
            _nodes = new List<Node>();
        }

        private static Node AstarSearch(List<PowerplantInfo> infos)
        {
            ResetGraph();
            _nodes.Add(new Node(infos));
            while (_nodes.Count > 0)
            {
                _nodes.Sort(Node.Comparer);
                Node node = _nodes[0];
                if (node.done)
                {
                    _lastNode = node;
                    break;
                }
                else
                {
                    _nodes.RemoveAt(0);
                    _minAverageCost = node.averageCost;
                    node.Explore();
                }
            }

            // TODO return node based on done, surpass quantity, average
            return _lastNode;
        }


        public struct Assingment
        {
            public string name { get; set; }
            public int p { get; set; }
        }
    }

}
