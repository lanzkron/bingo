# Bingo card generator
This small program allows generating personalised bingo cards.

Rather than reading out the actual content of a cell on the bingo board you will read out a _definition_ and the players will mark the cell with the answer to said definition.

    dotnet run --title="Flowers bingo" --size=3 --center=Flowers

![Example board](https://user-images.githubusercontent.com/11015162/55152838-d93b6b80-5159-11e9-9497-74e227d80420.png)

## Input file

In order to generate a game you'll need a text file that contains the requeired cells in the game. Each cell is a line in the text file and can be one of the following.

* **Image file name** - the image will be used in the cell
* **Multi line text** - Seperate each line with a `|`
* **Plain text** - just use the text as is
* **Empty lines** - are ignored

You should have about 1.5 times more lines than cells in the board to make the game more interesting.

## Command line options
The files are generated by running the command line

    dotnet run --title="Demo game" --size=3 --count=20

* **title** - the title for each bingo card 
* **file** - text input file to use (defaults to _bingo.txt_)
* **size** - dimensions of the bard (defaults to 4x4)
* **center** - value to put in the center cell of each card (only supported for odd `size` cards)
* **count** - how many cards to generate
* **rtl** - use Right-To-Left text

## Output
The output is an HTML file called `output.html` which, when printed, places a card on each sheet of paper.
