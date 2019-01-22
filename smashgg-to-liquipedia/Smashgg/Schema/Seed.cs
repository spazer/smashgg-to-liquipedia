using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Seed
    {
        public int id { get; set; }
        public Entrant entrant { get; set; }
        public List<Player> players { get; set; }
        public int entrantId { get; set; }
        public int seedNum { get; set; }
        public int placement { get; set; }
        public bool isBye { get; set; }
        public List<Standing> standings { get; set; }
    }
}
