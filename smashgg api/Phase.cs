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
        public List<PhaseGroup> id;
        public int phaseId;
        private int waveId;
        private Type phaseType;

        public Phase()
        {
            phaseType = Type.Bracket;
            id = new List<PhaseGroup>();
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
