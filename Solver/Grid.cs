using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Solver;
using System.Diagnostics;

namespace Solver
{
    public class Grid : ICloneable
    {
        private List<Tile> myTiles = new List<Tile>();
        public EventHandler<TileEventArgs> NewTileAnswered;
        public delegate List<Tile> FetchMethod(int coordinate);
        private FetchMethod[] _fetchMethods; 

        public FetchMethod[] FetchMethods
        {
            get { return _fetchMethods; }
            set { _fetchMethods = value; }
        }

        
        /// <summary>
        /// Create The Grid
        /// </summary>
        /// <param name="serializedBoard">81 character numeric string, 0 denotes a blank tile </param>
        public Grid(string serializedBoard)
        {
            if (serializedBoard.Length != 81)
            {
           
                throw new Exception("The input is not a valid sudoku puzzle");
            }

            this.FetchMethods = new FetchMethod[] { this.getRow, this.getColumn, this.getGroup };
            int gridIndex = 0;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    int answer = int.Parse(serializedBoard[gridIndex].ToString());
                    Tile tile = new Tile(x, y);
                    tile.Answered += this.TileIsSolved;
                    myTiles.Add(tile);
                    if (answer > 0) { tile.Answer = answer; }
                    gridIndex++;
                }
            }
        }

        public Grid()
        {
           this.FetchMethods = new FetchMethod[] { this.getRow, this.getColumn, this.getGroup };
        }

        /// <summary>
        /// Return a tile based on its coordinates in the 0-based 9x9 grid
        /// </summary>
        /// <param name="x">column 0-8</param>
        /// <param name="y">row 0-8</param>
        /// <returns>Tile</returns>
        public Tile GetTile(int x,int y)
        {
            Tile tile = myTiles.Find(t => (t.X == x && t.Y == y));
            return tile; 
        }

        /// <summary>
        /// Return a 3x3 List of Tiles based on their coordinates in the 0-based 9x9 grid
        /// </summary>
        /// <param name="x">group 0-8</param>
        /// <returns>List of Type Tile</returns>
        public List<Tile> getGroup(int group)
        {
            return myTiles.FindAll(t => t.Group == group);
        }

        /// <summary>
        /// Return a List of Tiles based on their coordinates in the 0-based 9x9 grid
        /// </summary>
        /// <param name="y">Row 0-8</param>
        /// <returns>List of Type Tile</returns>
        public List<Tile> getRow(int y)
        {
            return myTiles.FindAll(t => t.Y == y);
        }

        /// <summary>
        /// Return a List of Tiles based on their coordinates in the 0-based 9x9 grid
        /// </summary>
        /// <param name="x">Col 0-8</param>
        /// <returns>List of Type Tile</returns>
        public List<Tile> getColumn(int x)
        {
            return myTiles.FindAll(t => t.X == x);
        }

        public bool IsSolved {
            get { return !Convert.ToBoolean(myTiles.FindAll(t => t.IsSolved == false).Count); } 
        }

        public override string ToString()
        {
            string gridString = string.Empty;
            foreach (Tile tile in this.myTiles)
            {
                gridString += tile.Answer;
            }

            return gridString;
        }

        public void TileIsSolved(Object sender, TileEventArgs e)
        {
            Debug.Write(((Tile)sender).Answer);
            if (NewTileAnswered != null)
            {
                NewTileAnswered(sender, new TileEventArgs());
            }
        }

        /// <summary>
        /// Determine if the board is in a valid state.
        /// A valid state is one in which no collection of tiles has duplicate answers.
        /// </summary>
        public bool isValid 
        {
            get 
            {
                if (myTiles.FindAll(t => t.GetPossibleAnswers().Count == 0).Count > 0)
                {
                    return false;
                }

                for (int index = 0; index < 9; index++)
                {
                    List<int> row = this.getRow(index).FindAll(t => t.IsSolved).SelectMany(t => t.GetPossibleAnswers()).ToList();
                    List<int> col = this.getColumn(index).FindAll(t => t.IsSolved).SelectMany(t => t.GetPossibleAnswers()).ToList();
                    List<int> group = this.getGroup(index).FindAll(t => t.IsSolved).SelectMany(t => t.GetPossibleAnswers()).ToList();

                    if (row.Distinct().Count() - row.Count != 0 ||
                       col.Distinct().Count() - col.Count != 0 ||
                       group.Distinct().Count() - group.Count != 0)
                    {
                        return false;
                    }

                }
                return true;
            }
        }

        #region ICloneable Members

        public Object Clone()
        {
            Grid gridClone = new Grid();
            foreach (Tile t in myTiles)
            {
                Tile tile = new Tile(t);
                tile.Answered += gridClone.TileIsSolved;
                gridClone.myTiles.Add(tile);
            }
            return gridClone;
        }

        #endregion
    }
}
