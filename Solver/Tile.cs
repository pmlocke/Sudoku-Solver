using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;


namespace Solver
{
    public class Tile
    {
        private List<int> possibleAnswers = new List<int>();
        public int X { get; set; }
        public int Y { get; set; }
        public int Group { get; private set; }

        public EventHandler<TileEventArgs> Answered;

        public Tile(int x, int y, int answer = 0)
        {
            X = x;
            Y = y;
            Group = SetGroup();

            if (answer > 0)
            {
                possibleAnswers.Add(answer);
            }
            else
            {
                int[] allAnswer = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                possibleAnswers.AddRange(allAnswer);
            }
        }

        /// <summary>
        /// Create A Tile with the same properties as an existing tile.
        /// </summary>
        /// <param name="t">Tile</param>
        public Tile(Tile t)
        {
            X = t.X;
            Y = t.Y;
            Group = t.Group;

            if (t.Answer > 0)
            {
                Answer = t.Answer;
            }
            else
            {
                possibleAnswers.AddRange(t.GetPossibleAnswers());
            }
        }

        public bool IsSolved
        {
            get
            {
                if (this.possibleAnswers.Count != 1) { return false; }
                return true;
            }
        }

        /// <summary>
        /// Removes a list of answers from the Tile's possible answer list.
        /// If this results in the tile having 1 answer, the tile is set to answered,
        /// and the Answered event is raised.
        /// </summary>
        /// <param name="answered"></param>
        public void RemoveAnswers(List<Tile> answered)
        {
            foreach(Tile tile in answered)
            {
                if (!this.IsSolved)
                {
                    if (tile.IsSolved)
                    {
                        this.possibleAnswers.Remove(tile.Answer);
                    }
                    else
                    {
                        foreach (int answer in tile.possibleAnswers)
                        {
                            this.possibleAnswers.Remove(answer);
                        }
                    }
                    if (this.IsSolved) { this.Answer = this.possibleAnswers[0]; }
                }
            }
        }

        /// <summary>
        /// Returns the solution for the tile or 0 for an unsolved tile
        /// Raises the Answered event when set.
        /// </summary>
        public int Answer
        {
            get{
                if (IsSolved)
                {
                    return possibleAnswers[0];
                }
                return 0;
            }

            set{
                possibleAnswers.Clear();
                possibleAnswers.Add(value);
                if (Answered != null)
                {
                    Answered(this,new TileEventArgs());
                }
            }
        }

        public List<int> GetPossibleAnswers()
        {
            return possibleAnswers;
        }

        public string XYString()
        {
            return this.X.ToString() + this.Y.ToString();
        }

        private int SetGroup()
        {
            int yGroup, xGroup;
            yGroup = Y - (Y % 3);
            xGroup = (int)Math.Floor(Convert.ToDouble(X) / 3);
            return xGroup + yGroup;
        }

        internal void SetPossibleAnswers(List<int> answers)
        {
            this.possibleAnswers = answers;
        }

    }

    public class TileEventArgs : EventArgs
    {
        public TileEventArgs()
        {
            //Not Implementnted
        }
    }
}
