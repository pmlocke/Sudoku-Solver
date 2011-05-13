using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solver.Strategies
{
    public enum Difficulty { basic = 0, easy , medium , hard , evil }

    public class BasicStrategy
    {
        private Grid _grid;

        public Grid Grid
        {
            get { return _grid; }
            set { _grid = value; 
                  value.NewTileAnswered += TileIsSolved; 
                }
        }
        public bool Enabled { get; set; }
        public Difficulty Difficulty { get; set; }

         public BasicStrategy()
        {
            this.Enabled = true;
            this.Difficulty = Difficulty.basic;
        }

        public virtual Grid Execute(List<Tile> tileCollection)
        {
            ReduceAnswers(tileCollection);
            return Grid;
        }

        internal static List<Tile> GetAnswered(List<Tile> tiles)
        {
            List<Tile> answeredTiles;
            return answeredTiles = tiles.FindAll(t => t.IsSolved);
        }

        protected static void RemovePossibleAnswers(List<Tile> tileCollection, List<Tile> answered)
        {
            foreach (Tile tile in tileCollection)
            {
                tile.RemoveAnswers(answered);
            }
        }

        protected void ReduceAnswers(List<Tile> tileCollection)
        {
            List<Tile> answered;
            answered = GetAnswered(tileCollection);
            RemovePossibleAnswers(tileCollection, answered);
        }

        public void TileIsSolved(Object sender, TileEventArgs e)
        {
            Tile tile = sender as Tile;
            this.ReduceAnswers(Grid.getRow(tile.Y));
            this.ReduceAnswers(Grid.getColumn(tile.X));
            this.ReduceAnswers(Grid.getGroup(tile.Group));
        }
    }
}
