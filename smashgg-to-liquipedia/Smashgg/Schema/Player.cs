using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Player
    {
        public int id;

        // Blank constructor
        public Player() { }

        // Overloaded constructor
        public Player(int inputID, string inputName, string inputCountry)
        {
            id = inputID;
        }
    }
}
