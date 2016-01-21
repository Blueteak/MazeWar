#Stage Generation

Stage generation will be achieved using a modified maze generation algorithm.<br />
The maze will be generated in a standard way (creating a single 'solution') where all points of the maze are accessable from every other point. The start and end point will be filled in creating a closed system. Finally holes will be randomly added to the maze wall to create quicker paths from any point in the maze to any other point.

A Map is stored as a 2D Array of Cells, where Cells have the following data:<br />

| Data     	| Type      	|
|:----: 	| :----:		|
| isWall	| boolean		|
| neighbors	| List<Cell>	|
| x	pos		| int			|
| y pos		| int			|


##Creation Algorithms
###Maze Generation
![alt tag](https://upload.wikimedia.org/wikipedia/en/transcoded/7/7d/Depth-First_Search_Animation.ogv/Depth-First_Search_Animation.ogv.360p.webm)<br />
The maze will is generated using Recursive Backtracking (Depth first search)
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

##Results

![alt tag](http://i.imgur.com/3Ei33x7.png)<br />
0% Chance to break walls


![alt tag](http://i.imgur.com/Nag19Pg.png)<br />
50% Chance to break walls


![alt tag](http://i.imgur.com/dR3YfhL.png)<br />
100% Chance to break walls