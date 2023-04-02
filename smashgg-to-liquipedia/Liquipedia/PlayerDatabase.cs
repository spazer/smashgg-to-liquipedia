using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia
{
    public class PlayerDatabase
    {
        private static string SMASH_DB_FILENAME = @"SmashAkaDatabase.json";
        private static string FIGHTERS_DB_FILENAME = @"FightersAkaDatabase.json";
        private static string REVID_PATH = @"parse.revid";
        private static string WIKITEXT_PATH = @"parse.wikitext.*";

        private static string TEMPLATE_START = @"{{AltSlot";
        private static string TEMPLATE_END = @"}}";
        private static string TEMPLATE_PLAYER = @"|player=";
        private static string TEMPLATE_FLAG = @"|flag=";
        private static string TEMPLATE_ALTS = @"|alts=";
        private static string TEMPLATE_SMASHGG = @"|smashgg=";

        private readonly IFormMain form;

        public enum DbSource { Smash, Fighters };

        /// <summary>
        /// The key is the smash.gg id
        /// </summary>
        public Dictionary<int, PlayerInfo> players;

        private int revID;

        public PlayerDatabase(DbSource source, IFormMain form)
        {
            this.form = form;
            players = new Dictionary<int, PlayerInfo>();
            ReadDatabaseFromFile(source);
        }

        public int RevID
        {
            get { return revID; }
        }

        public bool ReadDatabaseFromFile(DbSource source)
        {
            // Clear the list
            players.Clear();

            // Set the filename
            string filename = string.Empty;
            if (source == DbSource.Fighters)
            {
                filename = FIGHTERS_DB_FILENAME;
            }
            else if (source == DbSource.Smash)
            {
                filename = SMASH_DB_FILENAME;
            }
            else
            {
                return false;
            }

            // Read AKA database json if available
            string json = string.Empty;
            try
            {
                using (StreamReader file = new StreamReader(filename))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject obj = (JObject)JToken.ReadFrom(reader);

                        revID = obj.SelectToken(REVID_PATH).Value<int>();
                        json = obj.SelectToken(WIKITEXT_PATH).Value<string>();
                        ParseWikitext(json);

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void ParseWikitext(string json)
        {
            string segment = "";
            int i = 0;
            try
            {
                while (json.IndexOf(TEMPLATE_START, i) != -1 && json.IndexOf(TEMPLATE_END, i) != -1)
                {
                    int entrystart = json.IndexOf(TEMPLATE_START, i);
                    int entryend = json.IndexOf(TEMPLATE_END, i) + TEMPLATE_END.Length;

                    segment = json.Substring(entrystart, entryend - entrystart);

                    int playerPos = segment.IndexOf(TEMPLATE_PLAYER);
                    int flagPos = segment.IndexOf(TEMPLATE_FLAG);
                    int altPos = segment.IndexOf(TEMPLATE_ALTS);
                    int smashggPos = segment.IndexOf(TEMPLATE_SMASHGG);

                    string player = string.Empty;
                    string flag = string.Empty;
                    string regex = string.Empty;
                    int smashggID = 0;

                    // Get player
                    if (playerPos != -1 && flagPos != -1)
                    {
                        player = segment.Substring(playerPos + TEMPLATE_PLAYER.Length, flagPos - playerPos - TEMPLATE_PLAYER.Length).Trim();
                    }

                    // Get flag
                    if (flagPos != -1 && altPos != -1)
                    {
                        flag = segment.Substring(flagPos + TEMPLATE_FLAG.Length, altPos - flagPos - TEMPLATE_FLAG.Length).Trim();
                    }

                    // Get alts
                    if (altPos != -1)
                    {
                        regex = segment.Substring(altPos + TEMPLATE_ALTS.Length, smashggPos - altPos - TEMPLATE_ALTS.Length).Trim();
                    }

                    // Get smashgg ID. If no ID exists, skip adding this entry
                    if (smashggPos != -1)
                    {
                        if (!int.TryParse(segment.Substring(smashggPos + TEMPLATE_SMASHGG.Length, segment.Length - smashggPos - TEMPLATE_SMASHGG.Length - TEMPLATE_END.Length).Trim(), out smashggID))
                        {
                            i = entryend; 
                            continue;
                        }
                    }

                    PlayerInfo newPlayer = new PlayerInfo(player, flag, regex, smashggID);
                    players.Add(smashggID, newPlayer);

                    i = entryend;
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                form.Log += "Error during parsing of AKA database: " + ex.ParamName + " = " + ex.ActualValue + "\r\n";
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                form.Log += "Error during parsing of AKA database: Index " + i + "\r\n";

                if (segment != String.Empty)
                {
                    form.Log += segment + "\r\n";
                }
                form.Log += ex.Message + "\r\n";
            }
        }

        public bool SaveDatabase(string json, DbSource source)
        {
            // Save data to file
            try
            {
                // Set the filename
                string filename;
                if (source == DbSource.Fighters)
                {
                    filename = FIGHTERS_DB_FILENAME;
                }
                else if (source == DbSource.Smash)
                {
                    filename = SMASH_DB_FILENAME;
                }
                else
                {
                    return false;
                }

                StreamWriter userinfo = new StreamWriter(filename);
                userinfo.Write(json);
                userinfo.Close();

                return true;
            }
            catch
            {
                Console.WriteLine("Couldn't save json.");
                return false;
            }
        }
    }
}
