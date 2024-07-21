using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningGame2
{
    internal class Game
    {
        public Robot Rob {  get; set; }
        public Point BottomRight { get; set; }
        public int NumObstacles { get; set; }
        public int NumDirts { get; set; }

        public Game() { }
        public Game(Point bottomRight, int numObstacles, int numDirts)
        {
            BottomRight = bottomRight;
            NumObstacles = numObstacles;
            NumDirts = numDirts;
        }
    }
}
