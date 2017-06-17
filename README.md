# smashgg-to-liquipedia
Queries the smash.gg api for data to put in Liquipedia pages

## Getting the data
1. Pick the singles or doubles tab
2. Copy paste a valid smash.gg URL into the URL textbox.
  * **Singles bracket example:** https://smash.gg/tournament/the-big-house-6/events/melee-singles/brackets/76016
  * **Singles pools example:** https://smash.gg/tournament/the-big-house-6/events/melee-singles/brackets/76014
  * **Doubles bracket example:** https://smash.gg/tournament/the-big-house-6/events/melee-doubles/brackets/76019
3. If you're doing pools, click on **Bracket Pools** or **Round Robin** depending on the pool type. **Advance** denotes the number of players that advance to the next phase of the tournament. 
4. Press **Get Bracket** for singles, **Get Doubles** for doubles, or **Get Phase** for pools.

## Pools
Liquipedia code is automatically generated. You can ignore the rest of the buttons.

**Note:** Check the ranking numbers. Because of how smash.gg rankings work, it is VERY likely that there are errors.

## Filling in a Singles or Doubles Bracket
1. Copy a blank template for the desired bracket to be filled out. Winners and losers brackets should be done *seperately*.
  * **Winners example:** http://wiki.teamliquid.net/smash/Template:32DEWBracketA
  * **Losers example:** http://wiki.teamliquid.net/smash/Template:32DELBracketA
2. Paste these templates into the two indicated textboxes.
3. The two textboxes in the middle show the Winners and Losers matches respectively. The numbers on the left denote the *round*.
4. Set the **Start** and **End** numbers if desired. Everything between the start and end rounds will be inserted into the templates.
5. Set the **Offset** if needed.
  * **Ex.** If you want smash.gg Winner's Round 2 filled into Liquipedia Winner's Round 1, set Offset to -1.
6. **Shift** allows you to shift matches down by a value. Negative numbers will shift matches up.
7. You can **Lock** the Start, End, and Offset controls if you want. Otherwise, these controls will be changed whenever data is retrieved from smash.gg. Locking is useful if you want to perform the same actions for a batch of brackets.
8. Click **Fill Bracket** or **Fill Doubles**.

## Other Options

You can uncheck the **Output Winners**/**Output Losers** checkboxes to make the program ignore one or the other when filling out brackets. You usually won't need to do this.

**Match Details** will fill in match details for each set if they exist on smash.gg. Existing match data will NOT be overwritten.
  * **Note** Match details are not guaranteed to be complete (eg. missing stock or stage info) nor is it guaranteed to be correct. Check VODs if time allows.
  
**Fill byes** assumes that any player by himself in a set has a bye. This is crucial for round 1 in most brackets. However, it can mistakenly assign byes to later matchups if the bracket is incomplete, so be careful.
**Fill bye wins** will assume that players with byes win their matches. It will then insert checkmarks into the bracket.

**Guess Final Bracket** attempts to fill in a DEFinalBracket based on available information. Check **SMW** to use DEFinalSmwBracket instead.

Clicking **Prize Pool Table** will generate a prize pool table based on the retrieved bracket. The number in the box to the right indicates the number of places you want the table to have.
