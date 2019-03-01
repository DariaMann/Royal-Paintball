using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Playground
    {
        public static readonly int SIZE = 8;

        public GameObject[,] Players { get; set; }

        public Playground()
        {
            this.PutFigureOnSquare(new Position(0, 0), new Player(Color.WHITE));
        }

        public void PutFigureOnSquare(Position pos, GameObject player)
        {
            player.Position = pos;
            this.Players[pos.X, pos.Y] = player;
        }
       
    }
}
