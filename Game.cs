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

        public int cleared = 0;
        public char[,] Board { get; set; }

        public Game() { }
        public Game(Point bottomRight, int numObstacles, int numDirts)
        {
            BottomRight = bottomRight;
            NumObstacles = numObstacles;
            NumDirts = numDirts;
            Rob = new(new Point(), bottomRight);
        }

        public void InitBoard()
        {
            Board = new char[BottomRight.Y, BottomRight.X];
            for (int i = 0; i < BottomRight.Y; i++)
                for (int j = 0; j < BottomRight.X; j++)
                    Board[i, j] = ' ';
        }
       
        public void PlaceRobot()
        {
            Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X] = Rob.Name;
        }

        public Point RandomPosition()
        {
            Point p = new();
            Random r = new();
            p.X = r.Next(1, BottomRight.X - 2);
            p.Y = r.Next(2, BottomRight.Y - 1);

            return p;
        }

        public void PlaceItems()
        {
            for (int i = 0; i < NumObstacles; i++)
            {
                Item obs = new()
                {
                    Name = 'O',
                    Position = RandomPosition(),

                };
                while (Board[obs.Position.Y, obs.Position.X] != ' ')
                    obs.Position = RandomPosition();
                Board[obs.Position.Y, obs.Position.X] = obs.Name;

            }

            for (int i = 0; i < NumDirts; i++)
            {
                Item dirt = new()
                {
                    Name = 'D',
                    Position = RandomPosition(),
                };
                while (Board[dirt.Position.Y, dirt.Position.X] != ' ')
                    dirt.Position = RandomPosition();
                Board[dirt.Position.Y, dirt.Position.X] = dirt.Name;
            }
        }

        public void InitGame()
        {
            InitBoard();
            PlaceRobot();
            PlaceItems();
        }

        public void DisplayBoard()
        {
            Console.Clear();
            for (int i = 0; i < BottomRight.Y; i++)
            {
                for (int j = 0; j < BottomRight.X; j++)
                {
                    if (i == 0 || j == 0 || i == Board.GetUpperBound(0) || j == Board.GetUpperBound(1))
                        Board[i, j] = '#';
                    Console.Write("." + Board[i, j]); 
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Cleared: {cleared}\tRemaining: {NumDirts}");
        }

        public bool CanMove(int direction)
        {
            bool canMove = false;

            switch (direction)
            {
                case 0: // checking right
                    if (Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X + 1] == 'D' &&
                        BottomRight.X + 1> Rob.CurrentPosition.X) { canMove = true; break; }
                    if (Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X + 1] == ' ' &&
                        BottomRight.X + 1> Rob.CurrentPosition.X) { canMove = true; break; }
                    break;

                case 1: // checking up
                    if (Board[Rob.CurrentPosition.Y - 1, Rob.CurrentPosition.X] == 'D' &&
                        Rob.CurrentPosition.Y > 1) { canMove = true; break; }
                    if (Board[Rob.CurrentPosition.Y - 1, Rob.CurrentPosition.X] == ' ' &&
                        Rob.CurrentPosition.Y > 1) { canMove = true; break; }
                    break;

                case 2: // checking left
                    if (Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X - 1] == 'D' &&
                        Rob.CurrentPosition.X > 1) { canMove = true; break; }
                    if (Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X - 1] == ' ' &&
                        Rob.CurrentPosition.X > 1) { canMove = true; break; }
                    break;

                case 3: // checking down
                    if (Board[Rob.CurrentPosition.Y + 1, Rob.CurrentPosition.X] == 'D' &&
                        BottomRight.Y > Rob.CurrentPosition.Y) { canMove = true; break; }
                    if (Board[Rob.CurrentPosition.Y + 1, Rob.CurrentPosition.X] == ' ' &&
                        BottomRight.Y > Rob.CurrentPosition.Y) { canMove = true; break; }
                    break;
            }
            return canMove;
        }

        public bool IsStuck()
        {
            bool isStuck = false;
            if(CanMove(0) ==  false && CanMove(1) == false && CanMove(2) == false && CanMove(3) == false)
                isStuck = true;

            return isStuck;
        }

        public void Start()
        {
            while (NumDirts > 0)
            {
                
                
                if (IsStuck())
                {
                    Board[Rob.CurrentPosition.Y + 1, Rob.CurrentPosition.X] = ' ';
                    Board[Rob.CurrentPosition.Y - 1, Rob.CurrentPosition.X] = ' ';
                    Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X + 1] = ' ';
                    Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X - 1] = ' ';
                }

                int direction = Rob.Direction();
                while(CanMove(direction))
                {
                    Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X] = '^';
                    Rob.Move(direction);

                    if (Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X] == 'D')
                    {
                        NumDirts--;
                        cleared++;
                    }

                    Board[Rob.CurrentPosition.Y, Rob.CurrentPosition.X] = Rob.Name;
                    DisplayBoard();
                    Thread.Sleep(1000);
                }


                Console.WriteLine($"direction is {direction} position: ({Rob.CurrentPosition.Y}, {Rob.CurrentPosition.X})");
            }
        }
    }
}
