using smashgg_to_liquipedia.Smashgg.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia
{
    public class Entrant
    {
        public enum EntrantType { Singles, Doubles, Team }

#region GraphQL fields
        /// <summary>
        /// Gamertag
        /// </summary>
        public string name { get; set; }
        public int id { get; set; }
        public List<Participant> participants = new List<Participant>();
        public int eventId;
 #endregion

        EntrantType entrantType { get; set; }
    }
}
