using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solver;
using Solver.Strategies;

namespace Tests
{
    /// <summary>
    /// Summary description for SolverTest
    /// </summary>
    [TestClass]
    public class SolverTest
    {
        private Grid grid;
        private string sGrid = "057840002008360500600000000016008043002435100940100820000000008005083200700024360";
        private Solver.Solver solver;

        public SolverTest()
        {

        }

        [TestInitialize()]
        [TestMethod]
        public void MyTestInitialize()
        {
            this.grid = new Grid(sGrid);
            this.solver = new Solver.Solver();
            Assert.IsNotNull(grid);
        }

        private TestContext testContextInstance;

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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void load_strategies()
        {
            Assert.IsNull(solver.Strategies);
            string easyGrid = "407650030090700000000029000520007840060000001104003020005090084000500003200800000";
            Grid grid = new Grid(easyGrid);
            solver.Grid = grid;
            Assert.AreNotEqual(0, solver.Strategies.Count);
        }

        [TestMethod]
        public void solve_easy()
        {
            string easyGrid = "407650030090700000000029000520007840060000001104003020005090084000500003200800000";
            Grid grid = new Grid(easyGrid);
            solver.Grid = grid;
            Assert.IsTrue(solver.Solve());
      
        }

        [TestMethod]
        public void solve_medium()
        {
            string midGrid = "400008900010030750008509000070000506000000240004000093340900000107005800005000020";
            Grid grid = new Grid(midGrid);
            solver.Grid = grid;
            solver.Strategies.RemoveAll(s => s.Difficulty > Solver.Strategies.Difficulty.medium);
            Assert.IsTrue( solver.Solve());
        }

        [TestMethod]
        public void solve_need_subgroup_reduction()
        {
            string medGrid = "000020100587010200009503040040000502000000000705000010070304800002060957006090000";
            Grid grid = new Grid(medGrid);
            solver.Grid = grid;
            solver.Strategies.RemoveAll(s => s.Difficulty > Solver.Strategies.Difficulty.medium);
            Assert.IsTrue( solver.Solve());
        }

        [TestMethod]
        [ExpectedException(typeof(PuzzleTooDifficultException))]
        public void cannot_solve_puzzle_too_hard()
        {
            string hardGrid = "100060007070054900400800000240500060000000000010002043000005006003920070500070008";
            Grid grid = new Grid(hardGrid);
            solver.Grid = grid;
            solver.Strategies.RemoveAll(s => s.Difficulty > Solver.Strategies.Difficulty.medium);
            solver.Solve();
        }

        [TestMethod]
        public void solve_with_XWing()
        {
            string hardGrid = "100060007070054900400800000240500060000000000010002043000005006003920070500070008";
            Grid grid = new Grid(hardGrid);
            solver.Grid = grid;
            solver.Strategies.RemoveAll(s => s.Difficulty > Solver.Strategies.Difficulty.hard);
            Assert.IsTrue( solver.Solve());
        }

        [TestMethod]
        public void solve_evil_test_no_brute_force()
        {
            string evilGrid = "100060007070054900400800000240500060000000000010002043000005006003920070500070008";
            Grid grid = new Grid(evilGrid);
            solver.Grid = grid;
            solver.Strategies.RemoveAll(s => s.Difficulty > Solver.Strategies.Difficulty.hard);
            Assert.IsTrue( solver.Solve());
        }

        [TestMethod]
        public void solve_evil_with_brute_force_test()
        {
            string evilGrid = "100060007070054900400800000240500060000000000010002043000005006003920070500070008";
            Grid grid = new Grid(evilGrid);
            solver.Grid = grid;
            solver.Strategies.RemoveAll(s => s.Difficulty > Solver.Strategies.Difficulty.basic &&
                s.Difficulty < Solver.Strategies.Difficulty.evil);
            Assert.IsTrue( solver.Solve());
        }

        [TestMethod]
        [ExpectedException(typeof(PuzzleIsInvalidException))]
        public void cannot_solve_invalid_puzzle()
        {
            string evilGrid = "100060007070054900406800000240500060000000500010002043004005006003920070500070008";
            Grid grid = new Grid(evilGrid);
            solver.Grid = grid;
            solver.Strategies.RemoveAll(s => s.Difficulty > Solver.Strategies.Difficulty.basic &&
                s.Difficulty < Solver.Strategies.Difficulty.evil);
            Assert.IsTrue( solver.Solve());
        }
    }
}
