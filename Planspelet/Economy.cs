using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planspelet
{
    class Economy
    {
        public Economy()
        {

        }

        public void SellBooks(Market market, Player[] players)
        {
            List<Book>[] playerBooks;
            playerBooks = new List<Book>[players.Length];

            for (int i = 0; i < players.Length; i++)
                AddBooksToSell(playerBooks, players[i]);
            


        }

        private void AddBooksToSell(List<Book>[] playerBooks, Player player)
        {
            playerBooks[player.playerID] = player.GetBooksForSale();
        }
    }
}
