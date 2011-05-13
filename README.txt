Sudoku Solver 1.0 by Patrick Locke.

The Solver Library can solve sudoku Puzzles, this is not a surprise. What makes it interesting is the following:

+= Self Solving Tiles
	Each tile on the grid will fire an event when it is answered. This event is wired to the basic solver strategy which removes this 
	answer from related tiles. A related tile is one found in a row, column, or 3x3 group corresponding to the solved tile. The event may
	propagate further events, automatically pruning each tile down to the least amount of possible answers.

+= Dynamic Strategies
	Each solver strategy is a self contained algorithm which mirrors how a human would go about solving a sudoku puzzle. The 
	strategies included with this solver are not comprehensive, but they will solve most puzzles. The solving strategies are added at 
	runtime via reflection, so new strategies can be dropped into the library without altering the solver code. The solver strategy 
	collection may also be accessed via the client. 

+= Ramping Intelligence
	The Solver will start with human solving strategies and only add more intensive strategies as needed. For most puzzles the Brute Force 
	Method will not be invoked. When it is invoked, it is used in combination with self-solving tiles to dramatically reduce the running 
	time of the brute force strategy. The brute force strategy can solve all valid puzzles. 

The Library can be exercised and tested though its unit test project. 

What's not included:
A GUI
A hook to access the current state of the puzzle (which would be useful for a GUI)


