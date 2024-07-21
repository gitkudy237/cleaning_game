using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningGame2
{
    internal class Robot
    {
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }
        public Point CurrentPosition { get; set; }
        public char Name { get; set; }
        
        public Robot() { }
        public Robot(Point tl, Point br, Point cp, char name)
        {
            TopLeft = tl;
            BottomRight = br;
            CurrentPosition = cp;
            Name = name;
        }

        //Generate direction
        public int Direction()
        {
            Random direction = new Random();
            return direction.Next(4);
        }

        public void Move(int direction)
        {
            switch (direction)
            {
                case 0: // right
                    CurrentPosition.X += 1;
                    break;
                case 1: // up
                    CurrentPosition.Y -= 1;
                    break;
                case 3: // left
                    CurrentPosition.X -= 1;
                    break;
                default: // down
                    CurrentPosition.Y += 1;
                    break;
            }
        }
    }
}
