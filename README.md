# smashgg-api
Queries the smash.gg api for data to put in Liquipedia pages

## Getting the data
1. Pick the singles or doubles tab (Doubles pools is not supported yet)
2. Copy paste a valid smash.gg URL into the URL textbox.
  * **Singles bracket example:** https://smash.gg/tournament/shine-2016-1/brackets/12013/62944/203287
  * **Singles pools example:** https://smash.gg/tournament/shine-2016-1/brackets/12013/19006
  * **Doubles bracket example:** https://smash.gg/tournament/shine-2016-1/brackets/12014/62936/203272
3. If you're doing pools, click on **Bracket Pools** or **Round Robin** depending on the pool type. **Advance** denotes the number of players that advance to the next phase of the tournament. 
4. Press **Get Bracket** for singles, **Get Doubles** for doubles, or **Get Phase** for pools.

## Pools
Liquipedia code is automatically generated. You can ignore the rest of the buttons.

**Note:** Check the ranking numbers. Because of how smash.gg rankings work, it is VERY likely that there are errors.

## Filling in a Singles or Doubles Bracket
1. Copy a blank template for the desired bracket to be filled out. Winners and losers brackets should be done *seperately*.
  * **Singles example:** http://wiki.teamliquid.net/smash/Template:32DEWBracketA
  * **Doubles example:** http://wiki.teamliquid.net/smash/Template:32DELBracketA
2. Paste this template into the bottom left textbox.
3. The two textboxes in the middle show the Winners and Losers matches respectively. The numbers on the left denote the *round*.
4. Set the **Start** and **End** numbers on the right side of the window. This denotes the starting and ending rounds that will be filled into the template.
5. Set the **Offset** if needed.
  * **Ex.** If you want smash.gg Winner's Round 2 filled into Liquipedia Winner's Round 1, set Offset to -1.
  * **Note:** Chances are good you'll need to use this for Losers because of how smash.gg round numbering works.
6. Click **Fill Bracket** or **Fill Doubles**.

You can check/uncheck the Winners/Losers checkboxes to make the program ignore one or the other when filling out brackets. You usually won't need this.

Fill Unfinished Sets is only meant to be used when there is no data for a bracket. Typically this only happens when a tournament is ongoing.

**Note:** Don't use this program for DEFinalBracket or DEFinalDoublesBracket. Because of how smash.gg rounds differ from Liquipedia's, it's more trouble than it's worth.
