using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Game
    {
        public int winnerId { get; set; }
        public Stage stage { get; set; }
        public int orderNum { get; set; }
        public List<GameSelection> selections { get; set; }


        // Old stuff
        public int winner;

        public int EntrantChar(int entrantId)
        {
            if (selections != null && selections.Count > 0)
            {
                foreach (GameSelection selection in selections)
                {
                    if (selection.entrant.id == entrantId) return selection.selectionValue;
                }
            }

            return 0;
        }


        public int entrant1p1stocks;
        public int entrant1p2stocks;

        public int entrant1p2char;
        public int entrant2p2char;

        public int entrant2p1stocks;
        public int entrant2p2stocks;
    }
}
