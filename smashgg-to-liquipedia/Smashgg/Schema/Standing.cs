using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Standing
    {
        #region GraphQL fields
        public int id { get; set; }
        public Entrant entrant { get; set; }
        public int standing { get; set; }
        #endregion
    }
}
