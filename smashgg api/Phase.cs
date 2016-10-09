using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_api
{
    enum Type { Pool, Bracket }

    class Phase
    {
        public List<int> id;
        public int phaseId;
        private int waveId;
        private Type phaseType;

        public Phase()
        {
            phaseType = Type.Bracket;
            id = new List<int>();
        }

        public int WaveId
        {
            get
            {
                return waveId;
            }
            set
            {
                waveId = value;
                phaseType = Type.Pool;
            }
        }

        public Type PhaseType
        {
            get
            {
                return phaseType;
            }
        }
    }
}
