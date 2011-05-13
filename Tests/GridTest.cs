using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solver;

namespace Tests
{
    [TestClass]
    public class GridTest
    {
        private TestContext testContextInstance;
        private Grid grid;
        private string sGrid = "057840002008360500600000000016008043002435100940100820000000008005083200700024360";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

         [TestInitialize()]
         [TestMethod]
         public void MyTestInitialize() {
             this.grid = new Grid(sGrid);
             Assert.IsNotNull(grid);
         }

        [TestMethod]
        public void find_tile()
        {
            Assert.AreEqual(5, this.grid.GetTile(1, 0).Answer);
        }

        [TestMethod]
        public void find_group()
        {
            List<Tile> group = grid.getGroup(0);
           
            Assert.AreEqual(9, group.Count);
            Assert.AreEqual(group[0].X, 0);
            Assert.AreEqual(group[0].Y, 0);
            Assert.AreEqual(group[8].X, 2);
            Assert.AreEqual(group[8].Y, 2);
        }

        [TestMethod]
        public void find_row()
        {
            List<Tile> row = grid.getRow(0);
          
            Assert.AreEqual(9, row.Count);
            Assert.AreEqual(row[0].X, 0);
            Assert.AreEqual(row[0].Y, 0);
            Assert.AreEqual(row[8].X, 8);
            Assert.AreEqual(row[8].Y, 0);
        }

        [TestMethod]
        public void find_column()
        {
            List<Tile> col = grid.getColumn(0);

            Assert.AreEqual(9, col.Count);
            Assert.AreEqual(col[0].X, 0);
            Assert.AreEqual(col[0].Y, 0);
            Assert.AreEqual(col[8].X, 0);
            Assert.AreEqual(col[8].Y, 8);
        }

        [TestMethod]
        public void is_solved()
        {
            string solvedGrid = "957849992998369599699999999916998943992435199949199829999999998995983299799924369";
            grid = new Grid(solvedGrid);
            Assert.IsTrue(grid.IsSolved);
        }

        [TestMethod]
        public void is_not_solved()
        {
            Assert.IsFalse(grid.IsSolved);
        }

        [TestMethod]
        public void grid_to_string()
        {
            Assert.AreEqual(sGrid, grid.ToString());
        }

        [TestMethod()]
        public void verify_test()
        {
            string solved = "182369457376254981459817632247538169938146725615792843721485396863921574594673218";
            Grid grid = new Grid(solved);
            Assert.IsTrue(grid.isValid);
        }
    }
}
