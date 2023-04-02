# smashgg-to-liquipedia
Queries the start.gg (formerly smash.gg) api for data to put in Liquipedia pages

## Getting the data
Before starting, ensure you have a valid start.gg api key. These keys are only valid for one year. Enter it into the "Auth" window.

1. Copy/paste a valid start.gg tournament URL into the URL textbox. The exact subpage doesn't matter as long as the tournament slug clearly shows up in the URL.
  * **Example 1:** https://www.start.gg/tournament/genesis-9-1/details
  * **Example 2:** https://www.start.gg/tournament/genesis-9-1/event/melee-doubles/brackets/1276361/1956087
2. Click "Get Tournament". Events are populated in the Tournament Explorer.
  * **Note:** If the tournament is too large, you may need to enter an event URL instead and use the "Get Event" button
3. Find the event you want data for. Expand it by clicking the + sign.

## Option 1: Generate Pools/Group Table Code
1. Drill into the three and select a Phase or a Wave in the Tournament Explorer using the checkbox.
2. On the right hand side, select "Bracket Pools" or "Round Robin" depending on the format of the pools.
3. Set Advance Winners and Advance Losers depending on how many players will proceed to the next phase of the tournament.
4. Click on "Get Data". Let the program run.

**Note:** Check the ranking numbers. Because of how smash.gg rankings work, it is VERY likely that there are errors.

## Option 2: Generate Bracket Code
1. Select a PhaseGroup in the Tournament Explorer using the checkbox. In start.gg PhaseGroups are the lowest level object. If you have drilled down to the lowest level of the tree, you have likely selected a PhaseGroup.
2. Click on "Get Data". Let the program run.
3. Copy a blank template for the desired bracket to be filled out. Winners and losers brackets should be done *seperately*.
  * **Winners example:** http://wiki.teamliquid.net/smash/Template:32DEWBracketA
  * **Losers example:** http://wiki.teamliquid.net/smash/Template:32DELBracketSmwA
4. Paste these templates into the two indicated textboxes.
5. Click "Fill Bracket"

## Other Options

You can uncheck the **Output Winners**/**Output Losers** checkboxes to make the program ignore one or the other when filling out brackets. You usually won't need to do this.

**Match Details** will fill in match details for each set if they exist on smash.gg. Existing match data will NOT be overwritten.
  * **Note** Match details are not guaranteed to be complete (eg. missing stock or stage info) nor is it guaranteed to be correct due to input errors. Check VODs if time allows.
  
**Guess Finals** attempts to fill in a DEFinalBracket or 4DE4SBracket based on available information. This is not guaranteed to be accurate, so double check the results.

Clicking **Prize Pool Table** will generate a prize pool table based on the retrieved bracket. The number in the box to the right indicates the number of placements you want the table to have.

**Get AKA DB** will retrieve tag information from Liquipedia. This will help standardize names and flags according to start.gg player ID numbers.

The **Data** tab contains settings for advanced users.
  * **Start** and **End** indicates which rounds will be used from queried data. This should almost never be touched.
  * **Offset** will shift rounds left (if negative) and right (if positive) in to the destination Liquipedia template.
  * **Lock** will lock the start/end/offset fields between queries.
  * **Shift** allows you to move sets up and down in to the destination Liquipedia bracket
