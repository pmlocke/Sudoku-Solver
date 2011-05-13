using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solver.Strategies
{
    /// <summary>
    /// This strategy looks at a row/col/3x3 of tiles and determines if any of their possible answers are unique to that set. 
    /// If so, that tile is forced to be that answer.
    /// </summary>
    class DeductionStrategy : BasicStrategy
    {

        public DeductionStrategy()
        {
            this.Enabled = true;
            this.Difficulty = Difficulty.easy;
        }

        public override Grid Execute(List<Tile> tileCollection)
        {
           List<int> allPossibleAnswers = GetAllPossibleAnswers(tileCollection);
           List<int> uniqueAnswers = FindUniqueAnswers(allPossibleAnswers);
           SolveForUniqueAnswers(tileCollection, uniqueAnswers);
           return Grid;
        }

        private static void SolveForUniqueAnswers(List<Tile> tileCollection, List<int> uniqueAnswers)
        {
            foreach (int answer in uniqueAnswers)
            {
                tileCollection = tileCollection.FindAll(t => !t.IsSolved);
                if (tileCollection.Count > 0)
                {
                    Tile tile = tileCollection.Single(t => t.GetPossibleAnswers().Contains(answer));
                    tile.Answer = answer;
                }
            }
        }

        private static List<int> GetAllPossibleAnswers(List<Tile> tileCollection)
        {
            List<int> answerTotal = new List<int>();
            foreach (Tile tile in tileCollection)
            {
                if (!tile.IsSolved)
                {
                    answerTotal = answerTotal.Concat(tile.GetPossibleAnswers()).ToList();
                }
            } 
            return answerTotal;
        }

        private List<int> FindUniqueAnswers(List<int> allPossibleAnswers)
        {
            List<int> uniqueAnswers = (from answers in allPossibleAnswers
                                       group answers by answers into g
                                       let count = g.Count()
                                       where g.Count() == 1
                                       select (g.Key)).ToList();
            return uniqueAnswers;
        }
    }
}


   

       