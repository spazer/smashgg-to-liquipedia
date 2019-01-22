using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class SetSlot
    {
        #region GraphQL fields
        public string id { get; set; }
        public Entrant entrant { get; set; }
        public int? seedId { get; set; }
        public string prereqId { get; set; }
        public string prereqType { get; set; }
        public int prereqPlacement { get; set; }
        #endregion
    }
}
