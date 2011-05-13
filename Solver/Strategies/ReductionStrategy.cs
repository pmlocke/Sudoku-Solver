using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solver.Strategies
{
    /// <summary>
    /// The Basic Suduku strategy. Every time a tile is answered, remove that answer from the possible answers of
    /// related tiles.
    /// </summary>
    class ReductionStrategy : BasicStrategy
    {
        public ReductionStrategy()
        {
            this.Enabled = true;
            this.Difficulty = Difficulty.basic;
        }

        public override Grid Execute(List<Tile> tileCollection)
        {
            List<Tile> answered;
            answered = GetAnswered(tileCollection);
            RemovePossibleAnswers(tileCollection, answered);
            return Grid;
        }

        internal static List<Tile> GetAnswered(List<Tile> tiles)
        {
            List<Tile> answeredTiles;
            return answeredTiles = tiles.FindAll(t => t.IsSolved);
        }


    }
}
