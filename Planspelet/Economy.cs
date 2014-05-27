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

        public void SellAllBooks(Market market, Player[] players)
        {
            List<Book>[] printedBooks, eBooks;
            printedBooks = new List<Book>[players.Length];
            eBooks = new List<Book>[players.Length];

            //Adds each player's books for sale to a list
            for (int i = 0; i < players.Length; i++)
                AddBooksToSell(printedBooks, eBooks, players[i]);

            int numberOfGenres = Book.numberOfGenres;

            //Goes through each genre and sells books from every player
            for (int i = 0; i < numberOfGenres; i++)
                SellFromGenre((Genre)i, market, printedBooks, eBooks, players, rand);
        }

        private void SellFromGenre(Genre genre, Market market, List<Book>[] printedBooks, List<Book>[] eBooks, Player[] players, Random rand)
        {
            List<Book> printedOfGenre = new List<Book>();
            List<Book> digitalOfGenre = new List<Book>();

            //Gets the demands of the specific genre from the Market class
            int printedGenreDemand = market.GetDemand(genre, false);
            int digitalGenreDemand = market.GetDemand(genre, true);

            //Collects all the books of the specific genre from all players
            for (int i = 0; i < printedBooks.Length; i++)
                printedOfGenre.AddRange(printedBooks[i].Where(b => b.GetGenre() == genre));
            for (int i = 0; i < eBooks.Length; i++)
                digitalOfGenre.AddRange(eBooks[i].Where(b => b.GetGenre() == genre));

            foreach (Book book in printedOfGenre)
                book.CalcProfitablity();

            //Sell digital books
            for (int i = 0; i < digitalGenreDemand; i++)
            {
                if (digitalOfGenre.Count == 0)
                    break;

                int index = rand.Next(0, digitalOfGenre.Count);

                SellBook(genre, index, true, digitalOfGenre, players, market, ref digitalGenreDemand);
            }

            //Leftover demand for ebooks is sold printed books
            for (int i = 0; i < digitalGenreDemand; i++)
            {
                if (printedOfGenre.Count == 0)
                    break;

                int index = rand.Next(0, printedOfGenre.Count);

                SellBook(genre, index, true, printedOfGenre, players, market, ref digitalGenreDemand);
            }

            //Sell printed books
            for (int i = 0; i < printedGenreDemand; i++)
            {
                if (printedOfGenre.Count == 0)
                    break;

                int index = rand.Next(0, printedOfGenre.Count);

                SellBook(genre, index, false, printedOfGenre, players, market, ref printedGenreDemand);
            }
        }

        private void SellBook(Genre genre, int index, bool eBook, List<Book> books, Player[] players, Market market, ref int demand)
        {
            int sellChance = rand.Next(0, 101);

            if (books[index].SellChance >= sellChance)
            {
                if (!players[books[index].Owner].BookSold(books[index]))
                    return;

                market.RemoveDemand(genre, eBook, 1, books[index].Owner);
                demand -= 1;

                if (books[index].Stock <= 0 && !eBook)
                    books.RemoveAt(index); 
            }
        }

        private void AddBooksToSell(List<Book>[] printedBooks, List<Book>[] eBooks, Player player)
        {
            printedBooks[player.playerID] = player.GetBooksForSale(false);
            eBooks[player.playerID] = player.GetBooksForSale(true);
        }
    }
}
