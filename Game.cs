using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningGame2
{
    internal class Game
    {
        private Robot _robot;
        private char[,] _board;
        private int _cleared = 0;
        public Point BottomRight { get; set; }
        public int NumObstacles { get; set; }
        public int NumDirts { get; set; }


        public Game() { }
        public Game(Point bottomRight, int numObstacles, int numDirts)
        {
            BottomRight = bottomRight;
            NumObstacles = numObstacles;
            NumDirts = numDirts;
            _robot = new(new Point(), bottomRight);
        }

        private void InitBoard()
        {
            _board = new char[BottomRight.Y, BottomRight.X];
            for (int i = 0; i < BottomRight.Y; i++)
                for (int j = 0; j < BottomRight.X; j++)
                    _board[i, j] = ' ';
        }
       
        private void PlaceRobot()
        {
            _board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X] = _robot.Name;
        }

        private Point RandomPosition()
        {
            Point point = new();
            Random r = new();
            point.X = r.Next(1, BottomRight.X - 2);
            point.Y = r.Next(2, BottomRight.Y - 1);

            return point;
        }

        private void PlaceItems()
        {
            for (int i = 0; i < NumObstacles; i++)
            {
                Item obstacle = new()
                {
                    Name = 'O',
                    Position = RandomPosition(),

                };
                while (_board[obstacle.Position.Y, obstacle.Position.X] != ' ')
                    obstacle.Position = RandomPosition();
                _board[obstacle.Position.Y, obstacle.Position.X] = obstacle.Name;

            }

            for (int i = 0; i < NumDirts; i++)
            {
                Item dirt = new()
                {
                    Name = 'D',
                    Position = RandomPosition(),
                };
                while (_board[dirt.Position.Y, dirt.Position.X] != ' ')
                    dirt.Position = RandomPosition();
                _board[dirt.Position.Y, dirt.Position.X] = dirt.Name;
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
                    if (i == 0 || j == 0 || i == _board.GetUpperBound(0) || j == _board.GetUpperBound(1))
                        _board[i, j] = '#';
                    Console.Write("." + _board[i, j]); 
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Cleared: {_cleared}\tRemaining: {NumDirts}");
        }

        private bool CanMove(int direction)
        {
            bool canMove = false;

            switch (direction)
            {
                case 0: // checking right
                    if (_board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X + 1] == 'D' ) { canMove = true; break; }
                    if (_board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X + 1] == ' ' ) { canMove = true; break; }
                    break;

                case 1: // checking up
                    if (_board[_robot.CurrentPosition.Y - 1, _robot.CurrentPosition.X] == 'D' ) { canMove = true; break; }
                    if (_board[_robot.CurrentPosition.Y - 1, _robot.CurrentPosition.X] == ' ' ) { canMove = true; break; }
                    break;

                case 2: // checking left
                    if (_board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X - 1] == 'D' ){ canMove = true; break; }
                    if (_board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X - 1] == ' ') { canMove = true; break; }
                    break;

                case 3: // checking down
                    if (_board[_robot.CurrentPosition.Y + 1, _robot.CurrentPosition.X] == 'D' ) { canMove = true; break; }
                    if (_board[_robot.CurrentPosition.Y + 1, _robot.CurrentPosition.X] == ' ') { canMove = true; break; }
                    break;
            }
            return canMove;
        }

        private Point ClosestPoint()
        {
            Point point = new();
            if (_board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X + 1] == 'D') // right
            {
                point.X = _robot.CurrentPosition.X + 1; point.Y = _robot.CurrentPosition.Y;
                return point;
            }
            
            if (_board[_robot.CurrentPosition.Y - 1, _robot.CurrentPosition.X] == 'D') // up
            {
                point.X = _robot.CurrentPosition.X; point.Y = _robot.CurrentPosition.Y - 1;
                return point;
            }
            
            if (_board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X - 1] == 'D') // left
            {
                point.X = _robot.CurrentPosition.X - 1; point.Y = _robot.CurrentPosition.Y;
                return point;
            }
            
            if (_board[_robot.CurrentPosition.Y + 1, _robot.CurrentPosition.X] == 'D') // down
            {
                point.X = _robot.CurrentPosition.X; point.Y = _robot.CurrentPosition.Y + 1;
                return point;
            }

            return null;
        }

        private bool IsStuck()
        {
            bool isStuck = false;
            if(CanMove(0) ==  false && CanMove(1) == false && CanMove(2) == false && CanMove(3) == false)
                isStuck = true;

            return isStuck;
        }

        public void Start()
        {
            Console.WriteLine("Press enter to start the game.");
            Console.ReadLine();
            while (NumDirts > 0)
            {
                if (IsStuck())
                {
                    _board[_robot.CurrentPosition.Y + 1, _robot.CurrentPosition.X] = ' ';
                    _board[_robot.CurrentPosition.Y - 1, _robot.CurrentPosition.X] = ' ';
                    _board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X + 1] = ' ';
                    _board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X - 1] = ' ';
                    DisplayBoard();
                }


                int direction = _robot.Direction();
                while(CanMove(direction))
                {
                    _board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X] = '^';
                    
                    Point closestPoint = ClosestPoint();
                    if (closestPoint != null)
                        _robot.Move(closestPoint);
                    
                    else
                        _robot.Move(direction);

                    if (_board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X] == 'D')
                    {
                        NumDirts--;
                        _cleared++;
                    }

                    _board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X] = _robot.Name;
                    DisplayBoard();
                    Thread.Sleep(1000);
                }
                

            }
        }
    }
}
