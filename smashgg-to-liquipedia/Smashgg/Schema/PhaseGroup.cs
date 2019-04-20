using smashgg_to_liquipedia.Smashgg.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Smashgg.Schema
{
    public class PhaseGroup
    {
        // GraphQL fields
        public int id { get; set; }
        public string displayIdentifier { get; set; }
        public int state { get; set; }
        public int phaseId { get; set; }
        public int? waveId { get; set; }
        public List<Seed> seeds { get; set; }
        public List<Set> sets { get; set; }
        public SetConnection paginatedSets { get; set; }
        public SeedConnection paginatedSeeds { get; set; }

        // Internal fields
        public enum IdentiferType { WaveNumber, NumberOnly, Other };
        private int number;
        public IdentiferType identifierType;
        private string wavePrefix;

        public PhaseGroup()
        {
            wavePrefix = string.Empty;
            number = 0;
            identifierType = IdentiferType.Other;
        }

        // When this is set, try to split the value into a letter (wave) and number (bracket number in wave)
        public string DisplayIdentifier
        {
            get
            {
                return displayIdentifier;
            }
            set
            {
                displayIdentifier = value;

                int temp;
                if (int.TryParse(displayIdentifier, out temp))
                {
                    identifierType = IdentiferType.NumberOnly;
                }
                else
                {
                    // Attempt to split the identifier into waves
                    if (displayIdentifier.Length > 1)
                    {
                        // The wave should be a bunch of letters
                        int waveLength = 0;
                        while (waveLength < displayIdentifier.Length)
                        {
                            // If the next character in the identifier is a letter, add it to the wave name
                            if (char.IsLetter(displayIdentifier, waveLength))
                            {
                                waveLength++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Ensure the rest of the identifier is a number
                        identifierType = IdentiferType.WaveNumber;
                        for (int i = waveLength; i < displayIdentifier.Length; i++)
                        {
                            if (!char.IsDigit(displayIdentifier, i))
                            {
                                identifierType = IdentiferType.Other;
                            }
                        }

                        // If all conditions are met, fill in the wave and number variables
                        if (identifierType == IdentiferType.WaveNumber)
                        {
                            wavePrefix = displayIdentifier.Substring(0, waveLength);
                            number = int.Parse(displayIdentifier.Substring(waveLength, displayIdentifier.Length - waveLength));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Figures out the wave letter for the phasegroup (if it exists)
        /// </summary>
        public void GenerateWave()
        {
            int temp;
            if (int.TryParse(displayIdentifier, out temp))
            {
                identifierType = IdentiferType.NumberOnly;
            }
            else
            {
                // Attempt to split the identifier into waves
                if (displayIdentifier.Length > 1)
                {
                    // The wave should be a bunch of letters
                    int waveLength = 0;
                    while (waveLength<displayIdentifier.Length)
                    {
                        // If the next character in the identifier is a letter, add it to the wave name
                        if (char.IsLetter(displayIdentifier, waveLength))
                        {
                            waveLength++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Ensure the rest of the identifier is a number
                    identifierType = IdentiferType.WaveNumber;
                    for (int i = waveLength; i<displayIdentifier.Length; i++)
                    {
                        if (!char.IsDigit(displayIdentifier, i))
                        {
                            identifierType = IdentiferType.Other;
                        }
                    }

                    // If all conditions are met, fill in the wave and number variables
                    if (identifierType == IdentiferType.WaveNumber)
                    {
                        wavePrefix = displayIdentifier.Substring(0, waveLength);
                        number = int.Parse(displayIdentifier.Substring(waveLength, displayIdentifier.Length - waveLength));
                    }
                }
            }
        }

        public string Wave
        {
            get
            {
                return wavePrefix;
            }
        }

        public int Number
        {
            get { return number; }
        }

        public Tournament.ActivityState State
        {
            get
            {
                switch (state)
                {
                    case 1:
                        return Tournament.ActivityState.Created;
                    case 2:
                        return Tournament.ActivityState.Active;
                    case 3:
                        return Tournament.ActivityState.Completed;
                    default:
                        return Tournament.ActivityState.Invalid;
                }
            }
        }
    }
}
