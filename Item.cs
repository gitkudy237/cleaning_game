using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningGame2
{
    internal class Item
    {
        public char name { get; set; } 
        public Point position { get; set; }

        public Item () { }
        public Item (char name, Point position)
        {
            this.name = name;
            this.position = position;
        }
    }
}
