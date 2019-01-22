using smashgg_to_liquipedia.Smashgg.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace smashgg_to_liquipedia
{
    public class Set
    {
        #region GraphQL fields
        public int id;
        public int round { get; set; }
        public int state { get; set; }
        public bool hasPlaceholder { get; set; }
        public string displayScore { get; set; }
        public List<Game> games { get; set; }
        public List<SetSlot> slots { get; set; }
        public int? winnerId;
        #endregion

        public int match;

        public int entrant1wins = 0;
        public int entrant2wins = 0;

        public int wPlacement;
        public int lPlacement;

        public Tournament.ActivityState State
        {
            get
            {
                switch (state)
                {
                    case 1:
                        return Tournament.ActivityState.Active;
                    case 2:
                        return Tournament.ActivityState.Called;
                    case 3:
                        return Tournament.ActivityState.Completed;
                    default:
                        return Tournament.ActivityState.Invalid;
                }
            }
        }

        public void ParseScore()
        {
            if (displayScore == string.Empty) return;

            Regex rx = new Regex(@".* ([0-9]) - .*([0-9])$");
            MatchCollection matches = rx.Matches(displayScore);

            if (matches.Count == 2)
            {
                int.TryParse(matches[0].Groups[0].Value, out entrant1wins);
                int.TryParse(matches[0].Groups[1].Value, out entrant2wins);
            }
        }
    }
}
