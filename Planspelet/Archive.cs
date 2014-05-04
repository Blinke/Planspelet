using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class Archive: Tab
    {
        List<Book> books;
        int rows;
        int columns;
        public int NumberOfBooks { get { return books.Count; } }
        int numOfShelves;

        float rowSpacing = 110;
        float columnSpacing = 80;

        int activeShelf = 0;
        bool selection = true;
        int selectionX = 0;
        int selectionY = 0;

        public Archive(Vector2 position, float scale, int rows, int columns)
            :base(position, scale)
        {
            books = new List<Book>();
            this.rows = rows;
            this.columns = columns;
            numOfShelves = 0;

            this.position = position;
        }
        public Archive(Archive archive)
            :base(archive)
        {
            books = new List<Book>();
            foreach (Book b in archive.books)
            {
                books.Add(b);
            }
            rows = archive.rows;
            columns = archive.columns;
            numOfShelves = archive.numOfShelves;
            rowSpacing = archive.rowSpacing;
            columnSpacing = archive.columnSpacing;
            activeShelf = 0;
            selection = false;
            selectionX = 0;
            selectionY = 0;

        }
        public override Tab Clone()
        {
            return new Archive(this);
        }

        public void AddBook(Book book)
        {
            if (books.Count > rows * columns * numOfShelves) numOfShelves++;
            books.Add(book);
        }

        /// <summary>
        /// Moves the selection-cursor in the given directions, the method only takes into account whether the parameters are positive, negative or zero. The selection only moves one step.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveSelection(int x, int y)
        {
            if (x > 0) selectionX++;
            else if (x < 0) selectionX--;
            if (y > 0) selectionY++;
            else if (y < 0) selectionY--;

            int booksOnShelf;
            int fullRows;
            int booksOnLastRow;

            booksOnShelf = rows * columns;
            if (activeShelf == numOfShelves - 1) booksOnShelf = books.Count - rows * columns * (numOfShelves - 1);
            fullRows = booksOnShelf / columns;
            if (fullRows == rows) booksOnLastRow = columns;
            else booksOnLastRow = booksOnShelf % columns;

            if (x > 0)
            {
                if (selectionY >= fullRows - 1 && selectionX > booksOnLastRow - 1)
                {
                    if (activeShelf + 1 < numOfShelves)
                        activeShelf++;
                    else
                        activeShelf = 0;

                    selectionX = 0;
                    selectionY = 0;
                }
                else if (selectionX > columns - 1)
                {
                    selectionX = 0;
                    selectionY++;
                }
            }
            else if (x < 0)
            {
                if (selectionX < 0)
                {
                    if (selectionY == 0)
                    {
                        selectionX = booksOnLastRow - 1;
                        selectionY = fullRows;
                    }
                    else
                    {
                        selectionX = columns - 1;
                        selectionY--;
                    }
                }
            }

            if (y > 0)
            {
                if (selectionY > rows - 1)
                selectionY = 0;
                if (selectionY == fullRows && selectionX > booksOnLastRow - 1)
                    selectionX = booksOnLastRow - 1;
            }
            else if (y < 0 && selectionY < 0)
            {
                selectionY = 0;
                selectionX = 0;
            }
        }

        public Book GetSelectedBook()
        {
            int selection = activeShelf * selectionX * selectionY + selectionX + selectionY * columns;
            return books[selection];
        }
        /// <summary>
        /// This method will copy and remove the selected book from the Archive. Make sure to save the copy!
        /// </summary>
        /// <returns></returns>
        public Book TransferSelectedBook()
        {
            int selection = activeShelf * selectionX * selectionY + selectionX + selectionY * columns;
            Book returnBook = books[selection];
            if (selection + 1 == books.Count)
                MoveSelection(-1, 0);
            books.RemoveAt(selection);
            return returnBook;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int counter = 0;
            int booksOnShelf = books.Count - activeShelf * rows * columns;
            if (booksOnShelf > rows * columns) booksOnShelf = rows * columns;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (counter < booksOnShelf)
                    {
                        if (selection && x == selectionX && y == selectionY)
                            books[counter + activeShelf * rows * columns].Draw(spriteBatch, position + new Vector2(columnSpacing * x, rowSpacing * y) * scale, Color.Yellow, scale);
                        else
                            books[counter + activeShelf * rows * columns].Draw(spriteBatch, position + new Vector2(columnSpacing * x, rowSpacing * y) * scale, Color.White, scale);
                    }
                    counter++;
                }
            }
        }
    }
}
