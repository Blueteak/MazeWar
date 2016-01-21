#Stage Generation

Stage generation will be achieved using a modified maze generation algorithm.<br />
The maze will be generated in a standard way (creating a single 'solution') where all points of the maze are accessable from every other point. The start and end point will be filled in creating a closed system. Finally holes will be randomly added to the maze wall to create quicker paths from any point in the maze to any other point.

##Creation Algorithms
###Maze Generation
The maze will be generated using a randomized version of Prim's Algorithm
1. Start with a grid full of walls
2. Pick a cell, mark it as part of the maze. Add the walls of the cell to the wall list.
3. While there are walls in the list:
..* Pick a random wall from the list and a random direction. If a cell in that direction isn't in the maze yet:
...* Make the wall a passage and mark the cell on the opposite side as part of the maze
...* Add the neighboring walls of the cell to the wall list
..* Remove the wall from the list

###Filling Start/End
The adding of the start and end points is the last part of the standard Randomized Prim's algorithm for creating a maze. This part will simply be omitted.

###Adding Holes
Holes will be randomly added to the maze by scanning through the entire maze and adding holes if **Random** and **Appropriate**
- **Random**: % Chance if cell wall is **Appropriate** for removal
- **Appropriate**: Cell wall has at least two adjacent walls.

The requirement of at least two adjacent walls means that there will not be wide open spaces, keeping corridors one space large.