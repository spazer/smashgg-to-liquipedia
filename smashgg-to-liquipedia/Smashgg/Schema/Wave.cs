using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class Wave
    {
        public int id { get; set; }
        public string identifier { get; set; }

        public Wave()
        {
            id = 0;
        }
    }
}
