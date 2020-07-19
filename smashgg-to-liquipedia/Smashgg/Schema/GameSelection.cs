using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class GameSelection
    {
        public int id { get; set; }
        public Entrant entrant { get; set; }
        public string selectionType { get; set; }
        public int selectionValue { get; set; }
    }
}
