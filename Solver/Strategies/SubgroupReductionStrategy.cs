using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solver.Strategies
{
    /// <summary>
    /// A more complex version of the basic strategy, subgroup reduction finds pairs/triples/quads of tiles with the same 
    /// possible answers then excludes those answers from the remaining tiles in the group. 
    /// </summary>
    class SubgroupReductionStrategy : BasicStrategy
    {
        public SubgroupReductionStrategy()
        {
            Enabled = true;
            Difficulty = Difficulty.medium;
        }

        public override Grid Execute(List<Tile> tileCollection)
        {
            List<Tile> tileMatch = GetMatchingTiles(tileCollection);
            RemoveSubgroup(tileCollection, tileMatch);
            return Grid;
        }

        //Get tiles with the same set of possible answers
        private List<Tile> GetMatchingTiles(List<Tile> tileCollection)
        {
            List<Tile> relatedTiles = tileCollection.GroupBy(t => t.GetPossibleAnswers(), new IntegerListComparer())
                .Where(x => x.Count() > 1)
                .SelectMany(g => g).ToList();

            return relatedTiles;

        }

        //If the number of matched tiles is equal to the amount of possible answers in those tiles, 
        //exclude them from the larger collection.
        private void RemoveSubgroup(List<Tile> tileCollection, List<Tile> tileMatch)
        {
            foreach (Tile tile in tileMatch)
            {
                int answerCount = tile.GetPossibleAnswers().Count;
                List<Tile> removeTiles = tileMatch.FindAll(t => t.GetPossibleAnswers().SequenceEqual(tile.GetPossibleAnswers()));
                if (answerCount == removeTiles.Count) //this is the important criteria to exclude the group
                {
                    tileCollection.RemoveAll(t => t.GetPossibleAnswers().SequenceEqual(tile.GetPossibleAnswers()));
                    RemovePossibleAnswers(tileCollection, removeTiles);
                }
            }
        }

        /// <summary>
        /// this interface is used to compare Tiles by their list of possible answers. 
        /// </summary>
        class IntegerListComparer : IEqualityComparer<List<int>>
        {
            #region IEqualityComparer<List<int>> Members

            public bool Equals(List<int> x, List<int> y)
            {
                return x.SequenceEqual(y);
            }

            //Needed to fufill the interface contract, but not implemented
            public int GetHashCode(List<int> obj)
            {
                return 0;
            }

            #endregion
        }
    
    }
}
