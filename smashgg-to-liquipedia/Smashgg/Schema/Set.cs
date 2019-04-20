using smashgg_to_liquipedia.Smashgg.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Set
    {
        #region GraphQL fields
        public string id;
        public int round { get; set; }
        public int state { get; set; }
        public bool hasPlaceholder { get; set; }
        private string displayScore;
        public List<Game> games { get; set; }
        public List<SetSlot> slots { get; set; }
        public int? winnerId;
        public int wPlacement;
        public int lPlacement;
        #endregion

        public int match;

        public int entrant1wins = 0;
        public int entrant2wins = 0;

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

        [JsonProperty("displayScore")]
        public string DisplayScore
        {
            get
            {
                return displayScore;
            }

            // Parse the string for scores
            set
            {
                displayScore = value;
                if (value == string.Empty || value == null)
                {
                    return;
                }
                else if (value == "DQ")
                {
                    return;
                }
                else
                {
                    Regex rx = new Regex(@".* ([0-9]) - .*([0-9])$");
                    MatchCollection matches = rx.Matches(value);

                    if (matches.Count == 1 && matches[0].Groups.Count == 3)
                    {
                        int.TryParse(matches[0].Groups[1].Value, out entrant1wins);
                        int.TryParse(matches[0].Groups[2].Value, out entrant2wins);
                    }
                }
            }
        }
    }
}
