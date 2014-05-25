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
                SellFromGenre((Genre)i, market, playerBooks, players, rand);

            for (int i = 0; i < players.Length; i++)
            {
                foreach (Book book in playerBooks[i])
                    book.AgeBook(1);
            }
        }

        private void SellFromGenre(Genre genre, Market market, List<Book>[] playerBooks, Player[] players, Random rand)
        {
            int genreDemand = 0; // market.GetDemand(genre);
            List<Book> booksOfGenre = new List<Book>();

            for (int i = 0; i < playerBooks.Length; i++)
            {
                booksOfGenre.AddRange(playerBooks[i].Where(b => b.GetGenre() == genre));
            }

            foreach (Book book in booksOfGenre)
                book.CalcProfitablity();

            for (int i = 0; i < genreDemand; i++)
            {
                if (booksOfGenre.Count == 0)
                    break;
                int index = rand.Next(0, booksOfGenre.Count);

                players[booksOfGenre[index].Owner].BookSold(booksOfGenre[index]);

                //market.RemoveDemand(genre, 1, booksOfGenre[index].Owner);
                genreDemand -= 1;

                if (booksOfGenre[index].Stock == 0 && !booksOfGenre[index].eBook)
                    booksOfGenre.RemoveAt(index);
            }
        }

        public void AddBooksToSell(List<Book>[] playerBooks, Player player)
        {
            playerBooks[player.playerID] = player.GetBooksForSale();
        }
    }
}
