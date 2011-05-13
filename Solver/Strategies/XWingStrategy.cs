using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solver.Strategies
{
    /// <summary>
    /// A complex strategy that compares related tiles on different rows or columns. 
    /// For a detailed explination visit: 
    /// http://www.sudokuwiki.org/x_wing_strategy
    /// </summary>
    class XWingStrategy : BasicStrategy
    {
        private enum Alignment { row, column };

        public XWingStrategy()
        {
            Enabled = true;
            Difficulty = Difficulty.hard;
        }

        public override Grid Execute(List<Tile> tileCollection)
        {
            List<Tile> matchedPair = new List<Tile>();
            for (int answer = 1; answer <= 9; answer++)
            {
                //Find exactly 2 tiles with the same possible answer 
                List<Tile> candidateTiles = tileCollection.FindAll(t => t.GetPossibleAnswers().Contains(answer) && !t.IsSolved);
                if (candidateTiles.Count != 2) { continue; }

                //Find a corresponding row or column with exactly 2 tiles containing the same possible answers
                List<Tile> nextCollection = new List<Tile>();
                Alignment rowOrCol = GetAlignment(candidateTiles);
          
                for (int xy = GetXY(candidateTiles[0], rowOrCol, true) + 1; xy < 9; xy++)
                {
                    nextCollection = (rowOrCol == Alignment.row) ? Grid.getRow(xy) : Grid.getColumn(xy);
                    matchedPair = nextCollection.FindAll(t => t.GetPossibleAnswers().Contains(answer) && !t.IsSolved);

                    if (!IsAnXWing(matchedPair, candidateTiles, rowOrCol))
                    { continue; }

                    ExcludeXWing(matchedPair, candidateTiles, rowOrCol, answer);
                }
            }
            return Grid;
        }

        //a valid x-wing has 2 possible values in 4 Tiles in a Box Formation
        private static bool IsAnXWing(List<Tile> matchedPair, List<Tile> candidateTiles, Alignment rowOrCol)
        {
            return (matchedPair.Count == 2 
                   && GetXY(matchedPair[0], rowOrCol) == GetXY(candidateTiles[0], rowOrCol)
                   && GetXY(matchedPair[1], rowOrCol) == GetXY(candidateTiles[1], rowOrCol));
        }

        //Remove the matched pair from the corresponding row/column of the x-wing
        private void ExcludeXWing(List<Tile> matchedPair, List<Tile> candidateTiles, Alignment rowOrCol, int answer)
        {
            List<Tile> removedAnswer = new List<Tile>();
            removedAnswer.Add(new Tile(0, 0, answer)); 
            List<Tile> removeFrom = new List<Tile>();
            int i = 0;
            foreach (Tile tile in matchedPair)
            {
                int rc = GetXY(tile, rowOrCol);
                removeFrom = (rowOrCol == Alignment.row) ? Grid.getColumn(rc) : Grid.getRow(rc);
                removeFrom.Remove(matchedPair[i]); 
                removeFrom.Remove(candidateTiles[i]); 
                RemovePossibleAnswers(removeFrom, removedAnswer); 
                i++;
            }
        }

        private static Alignment GetAlignment(List<Tile> candidateTiles)
        {
            if (candidateTiles[0].Y == candidateTiles[1].Y) { return Alignment.row; }
            return Alignment.column;
        }

        private static int GetXY(Tile t, Alignment alignmant, bool isReverse = false)
        {
            if (alignmant == Alignment.row && isReverse == false)
            {
                return t.X;
            }
            if (alignmant == Alignment.row && isReverse)
            {
                return t.Y;
            }
            if (alignmant == Alignment.column && isReverse == false)
            {
                return t.Y;
            }
                return t.X;
        }

    }


}
