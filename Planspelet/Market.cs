using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class Market
    {
        private struct Demand
        {
            Genre genre;

            public Demand(Genre genre)
                :this()
            {
                this.genre = genre;
            }
        }

        List<Demand> demand;

        Vector2 position;
        int X_max;
        int Y_max;


        public Market(Vector2 position, int X_max, int Y_max)
        {
            this.position = position;
            this.X_max = X_max;
            this.Y_max = Y_max;
        }

        public void AddBook(Book book)
        {
            
        }
    }
}
