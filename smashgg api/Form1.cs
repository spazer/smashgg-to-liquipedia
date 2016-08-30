using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace smashgg_api
{
    public partial class Form1 : Form
    {
        static string GG_URL_PHASEGROUP = "https://api.smash.gg/phase_group/";
        static string GG_URL_PHASE = "https://api.smash.gg/phase/";
        static string GG_URL_BRACKET = "?expand[]=sets&expand[]=entrants";
        static string GG_URL_GROUPS = "?expand[]=groups";

        static string GG_ENTRANTS = "\"entrants\":";
        static string GG_SETS = "\"sets\":";

        static string GG_ID = "\"id\":";
        static string GG_GAMERTAG = "\"gamerTag\":";
        static string GG_COUNTRY = "\"country\":";

        Dictionary<string, Player> entrants = new Dictionary<string, Player>();

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBracket_Click(object sender, EventArgs e)
        {
            string webText;
            int endPos = 0;

            WebRequest r = WebRequest.Create(GG_URL_PHASEGROUP + textBoxURL.Text + GG_URL_BRACKET);
            WebResponse resp = r.GetResponse();
            using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
            {
                webText = sr.ReadToEnd();
            }

            string entrants = ExpandOnly(webText, GG_ENTRANTS, 0, out endPos);
            GetEntrants(entrants);

            string sets = ExpandOnly(webText, GG_SETS, 0, out endPos);
        }

        private string ExpandOnly(string input, string title, int startPos, out int endPos)
        {
            endPos = 0;
            int bracketLevel = 0;
            char openBracket;
            char closeBracket;

            // Find the beginning of the desired expand
            if (input.IndexOf(title + "[", startPos) != -1)
            {
                openBracket = '[';
                closeBracket = ']';

                startPos = input.IndexOf(title + "[", startPos) + title.Length + 1;
            }
            else if (input.IndexOf(title + "{", startPos) != -1)
            {
                openBracket = '{';
                closeBracket = '}';

                startPos = input.IndexOf(title + "{", startPos) + title.Length + 1;
            }
            else
            {
                endPos = input.Length;
                return string.Empty;
            }

            endPos = startPos;
            bracketLevel = 1;

            // Iterate through the text until the whole expand is acquired
            do
            {
                int nextOpen = input.IndexOf(openBracket, endPos);
                int nextClose = input.IndexOf(closeBracket, endPos);

                // Bracket level cannot be zero if there are no more close brackets
                if (nextClose == -1)
                {
                    endPos = input.Length;
                    return string.Empty;
                }

                // Increase the bracket level for each additional open bracket. Subtract a level per close bracket.
                if (nextOpen < nextClose && nextOpen != -1)
                {
                    bracketLevel++;
                    endPos = nextOpen + 1;
                }
                else if (nextClose < nextOpen)
                {
                    bracketLevel--;
                    endPos = nextClose + 1;
                }
            } while (bracketLevel > 0);

            // Get whatever's in the brackets
            string output = input.Substring(startPos, endPos - startPos - 1);

            // If all we get are empty brackets, go deeper via recursion. If the end of input is reached, string.empty will be returned.
            if (output == string.Empty)
            {
                output = ExpandOnly(input, title, endPos, out endPos);
            }

            return output;
        }

        private string SplitExpand(string input, int startPos, out int endPos)
        {
            int bracketLevel = 0;
            endPos = startPos;

            do
            {
                int nextOpen = input.IndexOf('{', endPos);
                int nextClose = input.IndexOf('}', endPos);

                // Bracket level cannot be zero if there are no more brackets
                if (nextOpen == -1 || nextClose == -1)
                {
                    endPos = input.Length;
                    return string.Empty;
                }

                // Increase the bracket level for each additional open bracket. Subtract a level per close bracket.
                if (nextOpen < nextClose)
                {
                    bracketLevel++;
                    endPos = nextOpen + 1;
                }
                else if (nextClose < nextOpen)
                {
                    bracketLevel--;
                    endPos = nextClose + 1;
                }
            } while (bracketLevel > 0);

            // Return whatever's in the brackets
            return input.Substring(startPos, endPos - startPos - 1);
        }

        private int GetIntParameter(string input, string param)
        {
            if (input.IndexOf(param) != -1)
            {
                int start = input.IndexOf(param) + param.Length;
                int length = 0;     // Current length of int
                int temp;
                int output = -99;

                do 
                {
                    if (int.TryParse(input.Substring(start, length + 1), out temp))
                    {
                        output = temp;
                        length++;
                    }
                    else
                    {
                        if (length == 0)
                        {
                            return -99;
                        }
                        else
                        {
                            return output;
                        }
                    }
                } while (length < 15);  // Arbitrary limit

                return -99;
            }
            else
            {
                return -99;
            }
        }

        private string GetStringParameter(string input, string param)
        {
            if (input.IndexOf(param) != -1)
            {
                int start = input.IndexOf(param) + param.Length;

                // Error check for things that are not strings
                if(input.Substring(start,1) != "\"")
                {
                    return string.Empty;
                }

                // Find the closing quotation mark
                start = start + 1;
                int end = input.IndexOf("\"", start);

                return input.Substring(start, end - start);
            }
            else
            {
                return string.Empty;
            }
        }

        private void GetEntrants(string input)
        {
            List<string> rawEntrantData = new List<string>();
            int endPos = 0;
            string temp;

            // Divide input into manageable chunks
            do
            {
                temp = SplitExpand(input, endPos, out endPos);
                if (temp != string.Empty)
                {
                    rawEntrantData.Add(temp);
                }
            } while (temp != string.Empty);

            foreach (string entrant in rawEntrantData)
            {
                Player newPlayer = new Player();
                // Get player ID
                int id = GetIntParameter(entrant, GG_ID);

                // Get player tag
                newPlayer.name = GetStringParameter(entrant, GG_GAMERTAG);

                // Get player country
                newPlayer.country = GetStringParameter(entrant, GG_COUNTRY);
            }
        }

        class Player
        {
            public int id;
            public string name;
            public string country;
        }

        class Team
        {

        }

        class Set
        {

        }
    }
}
