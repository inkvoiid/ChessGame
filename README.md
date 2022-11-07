# And My Rook!
A one computer, two player chess-type game. 
 
A project for my second year doing a Diploma in Applied Infomation Technology

© VOIID Inc 2022

*(No further updates have been planned for this game, as I have am burnt out from working on this for half a year)*
 
## Setup
- [Download the latest version](https://github.com/inkvoiid/ChessGame/releases/latest/download/andmyrook-release-v1.0.zip)
- Extract the contents of the zip somewhere you can find it.
- Run "And My Rook!.exe"
- Blamo
 
## Guide / Controls

### The Basics
To start a new game, click "new game" and then select a save slot. This will take you to your castle screen, from there you can click between player 1 and 2's castles and upgrade each army respectively amd you can click the the top left button to rename your save.

### Castle Screen

**DISCLAIMER - No money system was implemented in this game, upgrading your pieces should be mutually decided between the two players.**

There are five main buttons: Loadout Manager, Upgrade Troops, Train Units, Play Regular Board and Play Large Board.

- Loadout Manager - View each chess piece you have in your army, increase your max X and Y for army to expand the size of your army, enable/disable pieces from participating in games and move their starting X,Y coords on the board.

- Upgrade Troops - The pieces in your army can be upgraded to stronger types and can be given special abilities.
  - Knighted - Can be given to any type other than a knight, allows the piece to also move like a knight.
  - Sidestep - Pawn Only, Allows pawns to move left and right.
  - Backpedal - Pawn Only, Allows pawns to take a step backward.

- Train Units - Train units of a specified type using the dropdowns. **Units will default to the position (-4, -4), which means they won't appear on the board and you will have to manually set it to a vacant spot.

- Play Regular Board - Start a game on an 8*8 chessboard.

- Play Large Board - Start a game on a 14*14 chessboard. (Set your piece's X to -3 for the left most on this board, *sorry*.)

## Playing a game of Chess

To start off, Player 1 is always white and white always starts, that's how it is, just thought I'd let you know.

To move a piece, click and drag it. Tiles should light up white if the piece can move there.

If you decide you don't want to move that piece, make sure you aren't hovering it over a valid move and just let go, it will fling back to where it came from.

Hovering over a piece, you will see the material and type it is, above that is a white health bar, below all that, abilities are displayed.

If you are to move and try to capture a piece, instead of taking it out in one go, the health points of the target and the attack points of the piece moving are taken into account.

If the health points minus the attack points of the target piece is not equal to 0, the target piece will take damage and the piece moving will return to it's spot. This move will also not show up in the move history as it technically didn't move.

The health and attack points are both equal to the material the piece is made from:
- Glass Piece - 1 Attack Point, 1 Health Point
- Ceramic Piece - 2 Attack Points, 2 Health Points
- Stone Piece - 3 Attack Points, 3 Health Points
- Metal Piece - 4 Attack Points, 4 Health Points
- Diamond Piece - 5 Attack Points, 5 Health Points

Once a piece is killed in a game, it stays dead and you will have to train another piece.

When a king is defeated or there is a stalemate, the game ends. Kings can't be don't stay dead and you can only have one per player. 

More examples on how attack/health points work:
- A ceramic Piece will take 3 turns to kill a diamond Piece, at 2 attack damage each turn.
- A glass piece will shatter in one hit from any piece.
- A diamond piece can take out another diamond piece in one hit.

Save files can be found in AppData/LocalLow/VOIID Inc/And My Rook!
