using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    public class MapSequence
    {
        public Item[,] Map { get; set; }
        public int StartSquare { get; set; }
        public int EndSquare { get; set; }                //The start square and end square refer to the column that the start/end square is in (either 0, 1 or 2).
        public int YPosition { get; set; }    
        public MapSequence(Item[,] mapSequence, int startSquare, int endSquare, int yPos) 
        {
            Map = mapSequence;
            StartSquare = startSquare;
            EndSquare = endSquare;
            YPosition = yPos;
        }
    }
}
