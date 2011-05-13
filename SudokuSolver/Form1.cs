using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int startX = 10;
            int startY = 15;
           
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    TextBox tile =  new TextBox();
                    tile.Width = 20;
                    tile.Height = 20;
                    int xBump = (int)Math.Floor( (double)x / 3);
                    int yBump = (int)Math.Floor((double)y / 3);
                    tile.Location = new Point(startX + tile.Width * (x + 1) + xBump, startY + tile.Height*(y + 1) + yBump);
                    tile.Name = "tile"+x.ToString() + y.ToString();
                    this.Controls.Add(tile);
                    
                }
            }
        }
    }
}
