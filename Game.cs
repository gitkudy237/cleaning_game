using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CleaningGame2
{
    internal class Game
    {
        private Robot _robot;
        private char[,] _board;
        private int _cleared = 0;
        public Point BottomRight { get; init; }
        public int NumObstacles { get; init; }
        public int NumDirts { get; private set; }


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
            Console.SetCursorPosition(0, 0);
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
            Console.WriteLine($"Robot position: ({_robot.CurrentPosition.Y}, {_robot.CurrentPosition.X})");
            Console.WriteLine($"Closest point is ({ClosestDirt().Y}, {ClosestDirt().X})");
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

        // Determines the closest Dirt in the immediate surrouding of the robot
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
        private float Distance(Point point1, Point point2)
        {
            return (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
        } 

        // Determines the closest dirt among all dirts found on the board from the actual robot position.
        private Point ClosestDirt()
        {
            Point closestPoint = new();
            for (int i = 0; i < BottomRight.Y; i++)
            {
                for(int j = 0; j < BottomRight.X; j++)
                {
                    if (_board[j, i] == 'D')
                    {
                        closestPoint.X = i;
                        closestPoint.Y = j; 
                        break;
                    }
                }
            }

            float minDistance = Distance(_robot.CurrentPosition, closestPoint);
            for (int i = 1; i < BottomRight.Y; i++)
            {
                for (int j = 1; j < BottomRight.X; j++)
                {
                    if (_board[i, j] == 'D')
                    {
                        float distance = Distance(_robot.CurrentPosition, new Point(j, i));
                        if (distance < minDistance)
                        {
                            closestPoint = new(j, i);
                            minDistance = distance;
                        }
                    }
                }
            }

            return closestPoint;
        }

        private List<int> PathTo(Point point)
        {
            var path = new List<int>();
            var pathFinder = new Point();
            pathFinder.X = _robot.CurrentPosition.X;
            pathFinder.Y = _robot.CurrentPosition.Y;

            while (pathFinder.X < point.X)
            {
                path.Add(0); // Add right to the path
                pathFinder.X++;
            }
            while (pathFinder.Y > point.Y)
            {
                path.Add(1); // Add up to the path
                pathFinder.Y--;
            }
            while (pathFinder.X > point.X)
            {
                path.Add(2); // Add left to the path
                pathFinder.X--;
            }
            while (pathFinder.Y < point.Y)
            {
                path.Add(3); // Add down to the path
                pathFinder.Y++;
            }

            return path;
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


                //int direction = _robot.Direction();

                var closestDirt = ClosestDirt();
                var path = PathTo(closestDirt);
                
                while (path.Count > 0)
                {
                    _board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X] = ' ';
                    _robot.Move(path.First());
                    path.RemoveAt(0);

                    if (_board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X] == 'D')
                    {
                        NumDirts--;
                        _cleared++;
                    }
                    _board[_robot.CurrentPosition.Y, _robot.CurrentPosition.X] = _robot.Name;
                    path.ForEach(x => Console.Write(x + " "));
                    DisplayBoard();
                    Thread.Sleep(300);
                }
            }
        }
    }
}
