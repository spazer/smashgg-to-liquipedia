using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Event
    {
        public enum EventType { Unknown, Singles, Doubles };

        public int id { get; set; }
        public string name { get; set; }
        public int? numEntrants { get; set; }
        public EntrantConnection entrants { get; set; }
        public List<Phase> phases { get; set; }
        public List<PhaseGroup> phaseGroups { get; set; }
        public Videogame videogame { get; set; }
        public int type { get; set; }
        public Tournament.ActivityState state { get; set; }
        public StandingConnection standings { get; set; }

        public EventType Type
        {
            get
            {
                switch (type)
                {
                    case 1:
                        return EventType.Singles;
                    case 5:
                        return EventType.Doubles;
                    default:
                        return EventType.Unknown;
                }
            }
        }
    }
}
