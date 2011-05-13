using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solver;
using Solver.Strategies;

namespace Tests
{
    [TestClass]
    public class StrategyTest
    {

        [TestMethod]
        public void Get_answered()
        {
            Tile tile = new Tile(8, 8);
            Tile tile2 = new Tile(7, 8, 2);
            Tile tile3 = new Tile(6, 8, 3);

            List<Tile> tileList = new List<Tile>();
            tileList.Add(tile);
            tileList.Add(tile2);
            tileList.Add(tile3);

            Assert.AreEqual(BasicStrategy.GetAnswered(tileList).Count, 2);
        }

        [TestMethod()]
        public void GetUniqueAnswersTest()
        {
            List<int> allPossibleAnswers = new List<int>() { 1, 1, 3, 3, 4, 4, 5, 2 };
            List<int> expected = new List<int>() { 5, 2 };
            List<int> actual;
            DeductionStrategy_Accessor accessor = new DeductionStrategy_Accessor();
            actual = accessor.FindUniqueAnswers(allPossibleAnswers);
            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod()]
        public void GetMatchingTilesTest()
        {
            Tile tile123 = new Tile(0, 0);
            tile123.SetPossibleAnswers(new List<int>() { 1, 2 });

            Tile tileMatch123 = new Tile(1, 0);
            tileMatch123.SetPossibleAnswers(new List<int>() { 1, 2 });

            Tile tile456 = new Tile(3, 0);
            tile456.SetPossibleAnswers(new List<int>() { 4, 5 });

            Tile tileMatch456 = new Tile(4, 0);
            tileMatch456.SetPossibleAnswers(new List<int>() { 4, 5 });

            Tile tile567 = new Tile(6, 0);
            tile567.SetPossibleAnswers(new List<int>() { 1, 2, 5, 6, 7 });

            List<Tile> allTiles = new List<Tile>() { tile123, tileMatch123, tile456, tileMatch456, tile567 };

            SubgroupReductionStrategy_Accessor accessor = new SubgroupReductionStrategy_Accessor();
            List<Tile> actual = accessor.GetMatchingTiles(allTiles);
            Assert.AreEqual(4, actual.Count);
            accessor.RemoveSubgroup(allTiles, actual);
            Assert.AreEqual(2, tile567.GetPossibleAnswers().Count);
        }
    }
}
