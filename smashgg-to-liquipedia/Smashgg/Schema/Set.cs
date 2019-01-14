using smashgg_to_liquipedia.Smashgg.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion

        public int entrant1wins;
        public int entrant2wins;
        public int entrant1PrereqId;
        public int entrant2PrereqId;
        public int winnerId;
        public int originalRound;
        public int displayRound;
        public int match;
        
        public bool isGF;

        public int wPlacement;
        public int lPlacement;

        public int wProgressingPhaseGroupId;
        public int lProgressingPhaseGroupId;

        public int gameId;

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
    }
}
