#Stage Generation

Stage generation will be achieved using a modified maze generation algorithm.<br />
The maze will be generated in a standard way (creating a single 'solution') where all points of the maze are accessable from every other point. The start and end point will be filled in creating a closed system. Finally holes will be randomly added to the maze wall to create quicker paths from any point in the maze to any other point.

##Creation Algorithms
###Maze Generation
The maze will is generated using Recursive Backtracking
1. Start at point 1,1
2. Randomly choose a wall at that point and carve a passage through to the adjacent cell, but only if the adjacent cell has not been visited yet. This becomes the new current cell.
3. If all adjacent cells have been visited, back up to the last cell that has uncarved walls and repeat.
4. The algorithm ends when the process has backed all the way up to the starting point.

###Filling Start/End
The adding of the start and end points is the last part of the standard Backtracking algorithm for creating a maze. This part is simply omitted.

###Adding Holes
Holes will are randomly added to the maze by scanning through the entire maze and adding holes if **Random** and **Appropriate**
- **Random**: % Chance if cell wall is **Appropriate** for removal
- **Appropriate**: Cell wall has at least two adjacent walls that are in the same row/column.

The requirement of at least two adjacent walls in the same row/column means that there will not be wide open spaces, keeping corridors one space large.