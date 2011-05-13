using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solver;

namespace Tests
{
    /// <summary>
    /// Tile Tests
    /// </summary>
    [TestClass]
    public class TileTest
    {
        public TileTest()
        {
            //
            // TODO: Add constructor logic here
            //
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
        public void Initialize_Tile()
        {
            Tile tile = new Tile(8,8);
            Assert.AreEqual(tile.X, 8);
            Assert.AreEqual(tile.Y, 8);
            Assert.AreEqual(tile.Group, 8);
            Assert.AreEqual(tile.GetPossibleAnswers().Count, 9);
            
        }

        public void initialize_tile_with_answer()
        {
            Tile tile = new Tile(8, 8, 1);
            Assert.AreEqual(tile.X, 8);
            Assert.AreEqual(tile.Y, 8);
            Assert.AreEqual(tile.Group, 8);
            Assert.AreEqual(tile.GetPossibleAnswers().Count, 1);
            Assert.AreEqual(tile.Answer, 1);
         

        }

        [TestMethod]
        public void Set_Tile_Group()
        {
            Tile tile = new Tile(3, 0);
            Assert.AreEqual(tile.Group, 1);

            tile = new Tile(7, 0);
            Assert.AreEqual(tile.Group, 2);

            tile = new Tile(0, 3);
            Assert.AreEqual(tile.Group, 3);

            tile = new Tile(3, 3);
            Assert.AreEqual(tile.Group, 4);

            tile = new Tile(7, 3);
            Assert.AreEqual(tile.Group, 5);

            tile = new Tile(0, 6);
            Assert.AreEqual(tile.Group, 6);

            tile = new Tile(3, 6);
            Assert.AreEqual(tile.Group, 7);

            tile = new Tile(6, 6);
            Assert.AreEqual(tile.Group, 8);

        }

        [TestMethod]
        public void get_set_tile_answer()
        {
            Tile tile = new Tile(8, 8);
            tile.Answer = 3;
            Assert.AreEqual(3,tile.Answer);

        }

        [TestMethod]
        public void get_tile_answer_negative()
        {
            Tile tile = new Tile(8, 8);
            Assert.AreEqual(0, tile.Answer);
        }

        [TestMethod]
        public void get_tile_possible_answers()
        {
            Tile tile = new Tile(8, 8);
            Assert.AreEqual(9, tile.GetPossibleAnswers().Count);
        }

        [TestMethod]
        public void remove_possible_answers()
        {

            Tile tile = new Tile(8, 8);
            Tile tile2 = new Tile(7, 8, 2);
            Tile tile3 = new Tile(6, 8, 3);

            List<Tile> tileList = new List<Tile>();
            tileList.Add(tile2);
            tileList.Add(tile3);

            Assert.AreEqual(9, tile.GetPossibleAnswers().Count);
            tile.RemoveAnswers(tileList);
            Assert.AreEqual(7, tile.GetPossibleAnswers().Count);
        }

        [TestMethod]
        public void tile_is_solved()
        {
            Tile tile = new Tile(8, 8);
            Assert.AreEqual(false, tile.IsSolved);
            tile.Answer = 1 ;
            Assert.AreEqual(true, tile.IsSolved);
            Assert.AreEqual(1, tile.GetPossibleAnswers().Count);

        }
    }
}
