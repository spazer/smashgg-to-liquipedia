using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_api
{
    class Entrant
    {
        private List<Player> players = new List<Player>();
        private int nameLength;

        public Entrant() { }

        public Entrant(Player player)
        {
            nameLength = 0;
            AddPlayer(player);
        }

        public Entrant(List<Player> playerList)
        {
            nameLength = 0;
            foreach (Player player in playerList)
            {
                AddPlayer(player);
            }
        }

        public void AddPlayer(Player newPlayer)
        {
            players.Add(newPlayer);
            nameLength += newPlayer.name.Length;
        }

        public List<Player> Players
        {
            get
            {
                return players;
            }
        }

        public int NameLength
        {
            get
            {
                return nameLength;
            }
        }
    }
}
