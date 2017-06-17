using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia
{
    class PhaseGroup
    {
        public enum IdentiferType { WaveNumber, NumberOnly, Other };

        private string displayIdentifier;
        private string wave;
        private int number;
        public int id;
        public IdentiferType identifierType;

        public PhaseGroup()
        {
            wave = string.Empty;
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
                            wave = displayIdentifier.Substring(0, waveLength);
                            number = int.Parse(displayIdentifier.Substring(waveLength, displayIdentifier.Length - waveLength));
                        }
                    }
                }
            }
        }

        public string Wave
        {
            get { return wave; }
        }

        public int Number
        {
            get { return number; }
        }
    }
}
