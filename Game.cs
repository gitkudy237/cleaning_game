﻿using System;
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
        public char[,] Board { get; set; }

        public Game() { }
        public Game(Point bottomRight, int numObstacles, int numDirts)
        {
            BottomRight = bottomRight;
            NumObstacles = numObstacles;
            NumDirts = numDirts;
            Rob = new Robot()
            {
                TopLeft = new Point(),
                BottomRight = bottomRight,
                //Placing the robot in the middle of the board
                CurrentPosition = new Point(bottomRight.X/2, bottomRight.Y/2),
                Name = 'R'
            };
        }

        public void InitBoard()
        {
            Board = new char[BottomRight.X, BottomRight.Y];
            for (int i = 0; i < BottomRight.X; i++)
                for (int j = 0; j < BottomRight.Y; j++)
                    Board[i, j] = ' ';
        }
         public void PlaceRobot()
        {
            Board[Rob.CurrentPosition.X, Rob.CurrentPosition.Y] = Rob.Name;
        }

        public Point RandomPosition()
        {
            Point p = new Point();
            Random r = new Random();
            p.X = r.Next(BottomRight.X);
            p.Y = r.Next(BottomRight.Y);

            return p;
        }

        public void PlaceItems()
        {
            for (int i = 0; i < NumObstacles; i++)
            {
                Item obs = new Item()
                {
                    Name = 'O',
                    Position = RandomPosition(),

                };
                while (Board[obs.Position.X, obs.Position.Y] != ' ')
                obs.Position = RandomPosition();

            }

            for (int i = 0; i < NumDirts; i++)
            {
                Item dirt = new Item()
                {
                    Name = 'D',
                    Position = RandomPosition(),
                };
                while (Board[dirt.Position.X, dirt.Position.Y] != ' ')
                    dirt.Position = RandomPosition();
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
            for (int i = 0; i < BottomRight.X; i++)
            {
                for (int j = 0; j < BottomRight.Y; j++)
                    Console.WriteLine(Board[i, j]); 
                Console.WriteLine();
            }
        }
    }
}
