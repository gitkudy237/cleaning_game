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
        public Robot(Point tl, Point br, char name = 'R')
        {
            TopLeft = tl;
            BottomRight = br;
            //Placing the robot in the middle of the board
            CurrentPosition = new Point(br.X / 2, br.Y / 2);
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
                case 2: // left
                    CurrentPosition.X -= 1;
                    break;
                case 3: // down
                    CurrentPosition.Y += 1;
                    break;
            }
        }

        public void Move(List<int> path)
        {
            while (path.Count > 0)
            {
                Move(path.First());
                path.RemoveAt(0);
            }
        }

        public void Move(Point newLocation)
        {
            CurrentPosition = newLocation;
        }
    }
}
