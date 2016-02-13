# Maze War AI
This document describes the AI added to the system to allow for offline play.

## Pathfinding
Because players are shown a minimap of the entire maze, the AI is given an omniscient view of the Maze. Pathfinding is done using a simple recursive algorithm between any two points in the maze.<br /><br />

The algorithm will be given a starting X and Y position. If the X and Y values are not on a wall, the method will call itself with all adjacent X and Y positions, making sure that it did not already use those X and Y positions before. If the X and Y values are those of the end location, it will save all the previous instances of the method as the correct path.

## Movement

### Sweeping
AI Movement while no players have been recently seen will be randomized. A random point will be chosen from the matrix of maze positions that are not walls. A path will be found and movement will commence along the path, where each block of movement is performed over a certain length of time. After the AI successfully reaches the end point chosen, a new endpoint will be chosen and the process will repeat.

### Player Sighted
If, while sweeping, an opponent comes into view, the end point will change to the last visible location. If the opponent stays in straight line of sight for a given threshold of time, the AI will fire in an attempt to kill them. If the opponent moves out of sight, the AI will take into account the opponent's last visible facing direction in order to attempt to follow.<br /><br />If the AI reaches it's end point without refreshing it's view of the opponent, it will pick a new random end point in the quadrant the player was last seen (where the AI's current position is the basis for the quadrants).

## Difficulty
AI difficulty will be based on 'reflex' and movement speed.