using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    enum Type { Pool, Bracket }

    public class Phase
    {
        // GraphQL fields
        public int id { get; set; }
        public int numSeeds { get; set; }
        public string name { get; set; }
        public int groupCount { get; set; }
        public PhaseGroupConnection phasegroups { get; set; }

        // Internal fields
        public SortedDictionary<string, List<PhaseGroup>> waves = new SortedDictionary<string, List<PhaseGroup>>();
    }
}
