using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg
{
    class Standardization
    {
        Dictionary<string, string> flagList = new Dictionary<string, string>();

        public Standardization()
        {
            // Populate flag abbreviation list
            using (StreamReader file = new StreamReader("Flag List.csv"))
            {
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] text = line.Split(',');

                    if (text.Length >= 2)
                    {
                        flagList.Add(text[0].ToLower(), text[1].ToLower());
                    }
                }
            }
        }

        /// <summary>
        /// Takes an input country and abbreviates it to two letters
        /// </summary>
        /// <param name="country">Input country</param>
        /// <returns>Two letter abbreviation of the coutnry, or string.Empty</returns>
        public string CountryAbbreviation(string country)
        {
            if ( country == null ) return string.Empty;

            string lcCountry = country.ToLower();

            // Assume a two-letter country is already an abbreviation
            if (lcCountry.Length == 2)
            {
                return lcCountry;
            }

            // Look in the dictionary for the country
            if (flagList.ContainsKey(lcCountry))
            {
                return flagList[lcCountry];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
