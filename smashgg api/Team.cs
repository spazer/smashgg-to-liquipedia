using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_api
{
    class Team
    {
        public Player player1;
        public Player player2;

        // Blank constructor
        public Team() { }

        // Overloaded constructor
        public Team(Player p1, Player p2)
        {
            player1 = p1;
            player2 = p2;
        }
    }
}
