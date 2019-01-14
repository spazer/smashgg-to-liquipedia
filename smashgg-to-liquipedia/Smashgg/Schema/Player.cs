using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia
{
    public class Player
    {
        public string name;
        public string country;
        public int playerID;

        // Blank constructor
        public Player() { }

        // Overloaded constructor
        public Player(int inputID, string inputName, string inputCountry)
        {
            playerID = inputID;
            name = inputName;
            country = inputCountry;
        }
    }
}
