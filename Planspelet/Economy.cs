using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planspelet
{
    class Economy
    {
        Random rand;

        public Economy()
        {
            rand = new Random();
        }

        public void SellBooks(Market market, Player[] players)
        {
            List<Book>[] playerBooks;
            playerBooks = new List<Book>[players.Length];

            for (int i = 0; i < players.Length; i++)
                AddBooksToSell(playerBooks, players[i]);

            int numberOfGenres = Book.numberOfGenres;

            for (int i = 0; i < numberOfGenres; i++)
            {
                SellFromGenre((Genre)i, market, playerBooks, players, rand);
            }

        }

        private void SellFromGenre(Genre genre, Market market, List<Book>[] playerBooks, Player[] players, Random rand)
        {
            int genreDemand = market.GetDemand(genre);
            List<Book> numberOfBooksFromGenre = new List<Book>();

            for (int i = 0; i < playerBooks.Length; i++)
            {
                numberOfBooksFromGenre.AddRange(playerBooks[i].Where(b => b.GetGenre() == genre));
            }

            for (int i = 0; i < genreDemand; i++)
            {
                if (numberOfBooksFromGenre.Count == 0)
		            break;

                int sellingIndex = rand.Next(0, numberOfBooksFromGenre.Count);

                players[numberOfBooksFromGenre[sellingIndex].Owner].BooksSold(1, 1);
                numberOfBooksFromGenre.RemoveAt(sellingIndex);
                market.RemoveDemand(genre, 1);
            }
        }

        public void AddBooksToSell(List<Book>[] playerBooks, Player player)
        {
            playerBooks[player.playerID] = player.GetBooksForSale();
        }
    }
}
