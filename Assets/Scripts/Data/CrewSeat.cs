using System.Collections;
using UnityEngine;

namespace ShipMaker.Data
{
    [System.Serializable]
    public class CrewSeat
    {
        public CrewPosition position;
        public int minRequired;
        public int space = 1;

        public CrewSeat()
        {

        }

        public CrewSeat(CrewSeat basedOn)
        {
            this.position = basedOn.position;
            this.minRequired = basedOn.minRequired;
            this.space = basedOn.space;
        }
    }
}