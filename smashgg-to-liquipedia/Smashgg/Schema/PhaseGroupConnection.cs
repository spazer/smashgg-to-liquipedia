using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class PhaseGroupConnection
    {
        public PageInfo pageInfo { get; set; }
        public List<PhaseGroup> nodes { get; set; }
    }
}
