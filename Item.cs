using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningGame2
{
    internal class Item
    {
        public char Name { get; set; } 
        public Point Position { get; set; }

        public Item () { }
        public Item (char name, Point position)
        {
            Name = name;
            Position = position;
        }
    }
}
