using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Solver.Strategies;
using System.Reflection;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]
namespace Solver
{
    public class Solver
    {
        private Grid _grid;
        private List<BasicStrategy> _strategies;

        public List<BasicStrategy> Strategies
        {
            get { return _strategies; }
        }
        public Grid Grid 
        {
            get { 
                return this._grid; 
            }
            set{
                this._grid = value;
                LoadStrategies();
            } 
        }
        public Solver()
        {

        }

        public Solver(Grid grid)
        {
            this.Grid = grid;
        }

        public bool Solve()
        {
            if (this.Grid == null)
            {
                throw new InvalidOperationException("The Solver requires a Grid");
            }

            string gridPrev = Grid.ToString();
            int iteration = 0;

            while (!Grid.IsSolved)
            {
                RunSolveStrategies();
                AddNextStrategyIfBlocked(gridPrev);
                gridPrev = Grid.ToString();
                iteration++;
            }

            return Grid.isValid;
        }

        private void AddNextStrategyIfBlocked(string gridPrev)
        {
           if(gridPrev == Grid.ToString())
           {
               try
               {
                   this.Strategies.OrderBy(s => s.Difficulty).First(s => !s.Enabled).Enabled = true;
               }
               catch(InvalidOperationException)
               {
                   throw new PuzzleTooDifficultException("This grid cannot be solved with the selected strategies");
               }
           }
        }

        /// <summary>
        /// Grab All Strategies from the Solver.Strategies Directory using reflection.
        /// Sidenote: 3 cheers for over-engineering. 
        /// </summary>
        internal void LoadStrategies()
        {
            _strategies = new List<BasicStrategy>();
            System.Reflection.Assembly asm = Assembly.Load("Solver");
            Type[] types = asm.GetTypes().Select(t => t).Where(t => t.Namespace == "Solver.Strategies" 
                && t.BaseType.Name == "BasicStrategy" || t.Name == "BasicStrategy").ToArray();

            foreach (Type t in types)
            {
                BasicStrategy strat = Activator.CreateInstance(t) as BasicStrategy;
                strat.Grid = this.Grid;
                if (strat.Difficulty != Difficulty.basic) { strat.Enabled = false; }
                _strategies.Add(strat);
            }
        }

        private void RunSolveStrategies()
        {
            List<Tile> tileCollection;
            foreach (Grid.FetchMethod Fetch in Grid.FetchMethods)
            {
                for (int index = 0; index < 9; index++)
                {
                    tileCollection = Fetch(index);
                    if (tileCollection.FindAll(t => t.IsSolved).Count == 9)
                    {
                        continue;
                    }

                    foreach (BasicStrategy s in _strategies)
                    {
                        if (s.Enabled)
                        {
                            s.Grid = Grid;
                            s.Execute(tileCollection);
                            if (s.Grid.IsSolved) { Grid = s.Grid; return; }
                        }
                    }
                }
            }
        }
    }

    public class PuzzleTooDifficultException : System.Exception
    {
        public PuzzleTooDifficultException()
        {
        }

        public PuzzleTooDifficultException(string message)
            : base(message)
        {
        }
    }

}
