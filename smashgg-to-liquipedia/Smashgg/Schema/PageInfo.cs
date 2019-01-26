using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class PageInfo
    {
        public int total { get; set; }
        public int totalPages { get; set; }
        public int page { get; set; }
        public int perPage { get; set; }
        public string sortBy { get; set; }
    }
}
