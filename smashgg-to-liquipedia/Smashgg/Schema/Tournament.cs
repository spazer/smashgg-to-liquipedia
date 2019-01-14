using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Tournament
    {
        public enum ActivityState { Created, Active, Completed, Ready, Invalid, Called, Queued }

        public int id;
        public string slug;

        public List<Event> events;
    }
}
