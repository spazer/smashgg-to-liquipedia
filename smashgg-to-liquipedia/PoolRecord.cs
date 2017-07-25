using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia
{
    class PoolRecord
    {
        private int matchesWin;
        private int matchesLoss;
        private int gamesWin;
        private int gamesLoss;
        public int rank;
        private double gameWinrate;
        private double matchWinrate;
        public bool isinGroup;
        private bool matchesComplete;

        public PoolRecord()
        {
            matchesWin = 0;
            matchesLoss = 0;
            gamesWin = 0;
            gamesLoss = 0;
            rank = 0;
            matchWinrate = -1;
            gameWinrate = -1;
            isinGroup = false;
            matchesComplete = false;
        }

        public void AddGameWins(int count)
        {
            if (count != -99)
            {
                gamesWin += count;
                CalculateGameRatio();
            }
        }

        public void AddGameLosses(int count)
        {
            if (count != -99)
            {
                gamesLoss += count;
                CalculateGameRatio();
            }
        }

        private void CalculateGameRatio()
        {
            if (gamesWin + gamesLoss != 0)
            {
                gameWinrate = (double)gamesWin / (double)(gamesWin + gamesLoss) * 100;
            }
            else
            {
                gameWinrate = -1;
            }
        }

        public void AddMatchWins(int count)
        {
            if (count != -99)
            {
                matchesWin += count;
                CalculateMatchRatio();
            }
        }

        public void AddMatchLosses(int count)
        {
            if (count != -99)
            {
                matchesLoss += count;
                CalculateMatchRatio();
            }
        }

        private void CalculateMatchRatio()
        {
            if (matchesWin + matchesLoss != 0)
            {
                matchWinrate = (double)matchesWin / (double)(matchesWin + matchesLoss) * 100;
            }
            else
            {
                matchWinrate = -1;
            }
        }

        public double GameWinrate
        {
            get { return gameWinrate; }
        }

        public double MatchWinrate
        {
            get { return matchWinrate; }
        }

        public int MatchesWin
        {
            get { return matchesWin; }
        }

        public int MatchesLoss
        {
            get { return matchesLoss; }
        }

        public bool MatchesComplete
        {
            get { return matchesComplete; }
            set { matchesComplete = value; }
        }
    }
}
