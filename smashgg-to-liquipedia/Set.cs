using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia
{
    class Set
    {
        public int id;
        public int entrantID1;
        public int entrantID2;
        public int entrant1wins;
        public int entrant2wins;
        public int entrant1PrereqId;
        public int entrant2PrereqId;
        public int winner;
        public int originalRound;
        public int displayRound;
        public int match;
        public int state;
        public bool isGF;

        public int wPlacement;
        public int lPlacement;

        public int gameId;

        public List<Game> games;

        public List<int> entrant1chars;
        public List<int> entrant2chars;
    }
}
