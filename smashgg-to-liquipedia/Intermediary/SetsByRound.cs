using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia
{
    class SetsByRound
    {
        public SetsByRound(int inputRound, List<Set> inputSets)
        {
            round = inputRound;
            sets = inputSets;
        }

        public SetsByRound() { }

        public int round;
        public List<Set> sets;
    }
}
