using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia
{
    class Entrant
    {
        public enum EntrantType { Singles, Doubles, Team }

        private string name = string.Empty;
        EntrantType entrantType;
        private List<Player> players = new List<Player>();
        private int placement;

        public Entrant() { }

        public Entrant(Player player)
        {
            AddPlayer(player);
            placement = 0;
        }

        public Entrant(List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                AddPlayer(player);
            }

            placement = 0;
        }

        public void AddPlayer(Player newPlayer)
        {
            players.Add(newPlayer);
        }

        public List<Player> Players
        {
            get
            {
                return players;
            }
        }

        public int Placement
        {
            get
            {
                return placement;
            }
            set
            {
                placement = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public EntrantType Type
        {
            get
            {
                return entrantType;
            }
            set
            {
                entrantType = value;
            }
        }
    }
}
