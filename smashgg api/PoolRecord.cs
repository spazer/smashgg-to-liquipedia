using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_api
{
    class PoolRecord
    {
        public int matchesWin;
        public int matchesLoss;
        private int gamesWin;
        private int gamesLoss;
        public int rank;
        private double gameWinrate;
        public bool isinGroup;

        public PoolRecord()
        {
            matchesWin = 0;
            matchesLoss = 0;
            gamesWin = 0;
            gamesLoss = 0;
            rank = 0;
            gameWinrate = 0;
            isinGroup = false;
        }

        public void AddWins(int count)
        {
            if (count != -99)
            {
                gamesWin += count;
                CalculateRatio();
            }
        }

        public void AddLosses(int count)
        {
            if (count != -99)
            {
                gamesLoss += count;
                CalculateRatio();
            }
        }

        private void CalculateRatio()
        {
            if (gamesWin + gamesLoss != 0)
            {
                gameWinrate = (double)gamesWin / (double)(gamesWin + gamesLoss) * 100;
            }
            else
            {
                gameWinrate = 0;
            }
        }

        public double GameWinrate
        {
            get { return gameWinrate; }
        }
    }
}
