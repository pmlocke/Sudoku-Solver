using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solver.Strategies
{
    /// <summary>
    /// A recursive guess-and-check strategy with backtracking
    /// </summary>
    class BruteForceStrategy : BasicStrategy
    {
        private Stack<Grid> gridHistory;
        private Stack<Dictionary<string, List<int>>> invalidAnswers;
        private Stack<Tile> prevTile = null;

        public BruteForceStrategy()
        {
            this.gridHistory = new Stack<Grid>();
            this.invalidAnswers = new Stack<Dictionary<string, List<int>>>();
            Enabled = true;
            this.Difficulty = Difficulty.evil;
        }

        public override Grid Execute(List<Tile> tileCollection)
        {
            gridHistory.Push(this.Grid);
            this.Grid = BruteForceSolve();
            return this.Grid;
        }

        private Grid BruteForceSolve()
        {
            Grid currentGrid = gridHistory.Peek().Clone() as Grid;
            Grid solvedGrid = currentGrid;
            this.Grid = currentGrid;
            prevTile = prevTile ?? new Stack<Tile>();

            Dictionary<string, List<int>> currentInvalid = GetInvalidAnswers();
            Tile unsolvedTile = FindFirstUnsolvedTile(currentGrid);
            string key = unsolvedTile.XYString();
            List<int> exceptions = (currentInvalid.ContainsKey(key)) ? currentInvalid[key] : new List<int>();

            try
            {
                GuessNextAnswer(unsolvedTile, exceptions);
            }
            catch (PuzzleIsInvalidException)
            {
                Tile PreviousTile = BacktrackToPreviousGuess();
                InvalidatePreviousGuess(currentInvalid, PreviousTile);

                //retry the last iteration with an updated invalid list.
                return solvedGrid = BruteForceSolve();
            }

            if (currentGrid.isValid)
            {
                UpdateGridHistory(currentGrid, unsolvedTile);
            }
            else
            {
                InvalidateCurrentGuess(currentInvalid, unsolvedTile, exceptions);
            }

            if (!currentGrid.IsSolved || !currentGrid.isValid)
            {
                solvedGrid =  BruteForceSolve();
            }

            return solvedGrid;
        }

        private static void InvalidateCurrentGuess(Dictionary<string, List<int>> currentInvalid, Tile guessedTile, List<int> exceptions)
        {
            if (currentInvalid.ContainsKey(guessedTile.XYString()))
            {
                exceptions.Add(guessedTile.Answer);
            }
            else
            {
                exceptions.Add(guessedTile.Answer);
                currentInvalid.Add(guessedTile.XYString(), exceptions);
            }
        }

        private void InvalidatePreviousGuess(Dictionary<string, List<int>> currentInvalid,Tile PreviousTile)
        {
            string prevKey = PreviousTile.XYString();
            if (invalidAnswers.Count > 0)
            {
                currentInvalid = invalidAnswers.Peek();
            }
            else
            {
                invalidAnswers.Push(currentInvalid);
            }

            if (currentInvalid.ContainsKey(prevKey))
            {
                currentInvalid[prevKey].Add(PreviousTile.Answer);
            }
            else
            {
                List<int> newException = new List<int>();
                newException.Add(PreviousTile.Answer);
                currentInvalid.Add(prevKey, newException);
            }
        }

        /// <summary>
        /// Push the current state onto the BruteForce stack
        /// </summary>
        private void UpdateGridHistory(Grid currentGrid, Tile guessTile)
        {
            prevTile.Push(new Tile(guessTile));
            gridHistory.Push(currentGrid);
            invalidAnswers.Push(new Dictionary<string, List<int>>());
        }

        /// <summary>
        /// Remove the current state from the Grid and retrieve the last Tile to be answered.
        /// </summary>
        /// <returns>Previous Tile</returns>
        private Tile BacktrackToPreviousGuess()
        {
            Tile PreviousTile;
            try
            {
                gridHistory.Pop();
                invalidAnswers.Pop();
                PreviousTile = prevTile.Pop();
            }
            catch (Exception)
            {
                throw new PuzzleIsInvalidException("The current puzzle cannot be solved by any means. It may not be a valid sudoku puzzle.");
            }
            return PreviousTile;
        }

        /// <summary>
        /// Guess an answer from a tile's Possible answers, excluding answers which were found to invalidate the Grid.
        /// Throws a PuzzleIsInvalidException when no Possible Answers remain.
        /// </summary>
        /// <param name="tileCollection">The first row of tiles which contain no answer </param>
        /// <param name="exceptions">the List of answers which are not possible for this tile this iteration</param>
        private static void GuessNextAnswer(Tile unsolvedTile, List<int> exceptions)
        {
            try
            {
                int guess = unsolvedTile.GetPossibleAnswers().Except(exceptions).Min();
                unsolvedTile.Answer = guess;
            }
            catch(InvalidOperationException){
                throw new PuzzleIsInvalidException();
            }
        }

        /// <summary>
        /// Retrieve the first instance of an unsolved tile in this iteration
        /// </summary>
        /// <param name="currentGrid"></param>
        /// <returns>A list of Tiles</returns>
        private static Tile FindFirstUnsolvedTile(Grid currentGrid)
        {
            List<Tile> tileCollection = new List<Tile>();

            //find the first unsolved Tile
            for (int y = 0; y < 9; y++)
            {
                tileCollection = currentGrid.getRow(y).FindAll(t => !t.IsSolved);
                if (tileCollection.Count > 0) { break; }
            }
            return tileCollection[0];
        }

        /// <summary>
        /// Get this iteration's dictionary of invalid guesses and initialize it if it does not exist
        /// </summary>
        /// <param name="invalidAnswers"></param>
        /// <returns></returns>
        private Dictionary<string, List<int>> GetInvalidAnswers()
        {
            Dictionary<string, List<int>> currentInvalid = new Dictionary<string, List<int>>();
            if (invalidAnswers.Count > 0)
            {
                currentInvalid = invalidAnswers.Peek();
            }
            else
            {
                invalidAnswers.Push(currentInvalid);
            }
            return currentInvalid;
        }

    }

    public class PuzzleIsInvalidException : System.InvalidOperationException
    {

        public PuzzleIsInvalidException()
        {
        }

        public PuzzleIsInvalidException(string message)
            : base(message)
        {
        }
    }
}
