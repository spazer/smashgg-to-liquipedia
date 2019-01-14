using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Participant
    {
        public int id { get; set; }
        public string gamerTag { get; set; }
        public Player player { get; set; }
        public int playerId { get; set; }
        public ContactInfo contactInfo { get; set; }
        public bool verified { get; set; }
    }
}
