using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planspelet
{
    class Archive : Tab
    {
        Texture2D[] selectionTexture;
        Texture2D lossTexture, profitTexture, outlineTexture;
        Texture2D eBookTexture;

        List<Book> books;
        int rows;
        int columns;
        public int NumberOfBooks { get { return books.Count; } }
        int numOfShelves = 1;

        const float rowSpacing = Book.Height + 10;
        const float columnSpacing = Book.Width + 10;

        int activeShelf = 0;

        public Archive(TextureManager textureManager, Vector2 position, float scale, int rows, int columns)
            : base(position, scale)
        {
            books = new List<Book>();
            this.rows = rows;
            this.columns = columns;

            this.position = position;

            selectionTexture = new Texture2D[4];

            for (int i = 0; i < selectionTexture.Length; i++)
                selectionTexture[i] = textureManager.middleSelection[i];

            eBookTexture = textureManager.eBookTexture;

            lossTexture = textureManager.lossTexture;
            profitTexture = textureManager.profitTexture;
            outlineTexture = textureManager.outlineTexture;
        }

        public Archive(Archive archive)
            : base(archive)
        {
            books = new List<Book>();
            foreach (Book b in archive.books)
            {
                books.Add(b);
            }
            rows = archive.rows;
            columns = archive.columns;
            numOfShelves = archive.numOfShelves;
            //rowSpacing = archive.rowSpacing;
            //columnSpacing = archive.columnSpacing;
            activeShelf = 0;
            //selection = false;
            //selectionX = 0;
            //selectionY = 0;

        }
        public override Tab Clone()
        {
            return new Archive(this);
        }

        public void CopyBooks(Archive archive)
        {
            books = new List<Book>();
            foreach (Book b in archive.books)
            {
                books.Add(b);
            }
        }

        public void AddBook(Book book)
        {
            if (NumberOfBooks + 1 > rows * columns * numOfShelves)
                numOfShelves++;
            if (books.Count > rows * columns * numOfShelves)
                numOfShelves++;

            books.Add(book);
        }

        public override void ReceiveInput(Input input, int playerIndex)
        {
            int x = 0;
            int y = 0;

            if (input.Up)
                y = -1;
            else if (input.Down)
                y = 1;
            if (input.Left)
                x = -1;
            else if (input.Right)
                x = 1;

            if (!selection[playerIndex].active)
                selection[playerIndex].active = true;
            else
                MoveSelection(x, y, playerIndex);
        }

        /// <summary>
        /// Moves the selection-cursor in the given directions, the method only takes into account whether the parameters are positive, negative or zero. The selection only moves one step.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MoveSelection(int x, int y, int playerIndex)
        {
            #region DON'T EVEN THINK ABOUT TOUCHING IT!!!
            if (x == 0 && y == 0)
                return;

            if (x > 0)
                selection[playerIndex].x++;
            else if (x < 0)
                selection[playerIndex].x--;
            if (y > 0)
                selection[playerIndex].y++;
            else if (y < 0)
                selection[playerIndex].y--;

            int booksOnShelf;
            int fullRows;
            int booksOnLastRow;

            booksOnShelf = rows * columns;
            if (activeShelf == numOfShelves - 1)
                booksOnShelf = books.Count - rows * columns * (numOfShelves - 1);

            fullRows = booksOnShelf / columns;
            if (fullRows == rows) booksOnLastRow = columns;
            else booksOnLastRow = booksOnShelf % columns;

            if (x > 0)
            {
                #region moving right
                if ((selection[playerIndex].x > booksOnLastRow - 1 && fullRows == rows && selection[playerIndex].y >= fullRows - 1) ||
                    (selection[playerIndex].x > booksOnLastRow - 1 && selection[playerIndex].y > fullRows - 1))
                {
                    selection[playerIndex].x--;// = 0;
                    //selection[playerIndex].y = 0;
                }
                else if (selection[playerIndex].x > columns - 1)
                {
                    selection[playerIndex].x = 0;
                    selection[playerIndex].y++;
                }
                #endregion
            }
            else if (x < 0)
            {
                #region moving left
                if (selection[playerIndex].x < 0 && selection[playerIndex].y == 0)
                {
                    selection[playerIndex].x++;// = booksOnLastRow - 1;
                    //if (fullRows == rows) selection[playerIndex].y = fullRows - 1;
                    //else selection[playerIndex].y = fullRows;
                }
                else if (selection[playerIndex].x < 0)
                {
                    selection[playerIndex].x = columns - 1;
                    selection[playerIndex].y--;
                }
                #endregion
            }

            if (y > 0)
            {
                #region moving down
                if (selection[playerIndex].y > fullRows - 1)
                {
                    if (fullRows == rows || booksOnLastRow == 0 || selection[playerIndex].y > fullRows)//selection[playerIndex].y > fullRows - 1 && fullRows == rows)
                    {
                        selection[playerIndex].y--;
                    }
                    else if (selection[playerIndex].y > fullRows - 1 && selection[playerIndex].x >= booksOnLastRow - 1)
                    {
                        selection[playerIndex].x = booksOnLastRow - 1;
                    }
                }
                #endregion
            }
            else if (y < 0 && selection[playerIndex].y < 0)
            {
                selection[playerIndex].y++;
            }
            #endregion
        }
        private void AdjustSelections()
        {
            for (int i = 0; i < 4; i++)
            {
                int selectionIndex = activeShelf * rows * columns + selection[i].x + selection[i].y * columns;
                if (selectionIndex + 1 >= books.Count)
                    MoveSelection(-1, 0, i);
            }
        }

        public Book GetSelectedBook(int playerIndex)
        {
            int selectionIndex = activeShelf * selection[playerIndex].x * selection[playerIndex].y + selection[playerIndex].x + selection[playerIndex].y * columns;

            if (books.Count > 0)
            {
                return books[selectionIndex]; // out of bound
            }

            return null;
        }
        public Book GetLastBook()
        {
            return books[books.Count - 1];
        }
        /// <summary>
        /// This method will copy and remove the selected book from the Archive. Make sure to save the copy!
        /// </summary>
        /// <returns></returns>
        public Book TransferSelectedBook(int playerIndex)
        {
            //int selectionIndex = activeShelf * selection[playerIndex].x * selection[playerIndex].y + selection[playerIndex].x + selection[playerIndex].y * columns;
            int selectionIndex = activeShelf * rows * columns + selection[playerIndex].x + selection[playerIndex].y * columns;
            Book returnBook = books[selectionIndex];
            AdjustSelections();
            //if (selectionIndex + 1 == books.Count)
            //    MoveSelection(-1, 0, playerIndex);
            books.RemoveAt(selectionIndex);
            return returnBook;
        }

        public int CountBooksByGenre(Genre genre)
        {
            int count = 0;
            foreach (Book b in books)
            {
                if (b.GetGenre() == genre)
                    count++;
            }
            return count;
        }
        public int CountBooksInPrint(Genre genre)
        {
            int count = 0;
            foreach (Book b in books)
            {
                if (b.GetGenre() == genre && b.inPrint)
                    count++;
            }
            return count;
        }

        public void ClearArchive()
        {
            books.Clear();
        }

        public void RemoveOldBooks()
        {
            books.RemoveAll(b => b.BookAge > 8);
        }

        public int GetStorageCost()
        {
            int storageCost = 0;

            foreach (Book book in books)
            {
                if (!book.eBook)
                {
                    storageCost += book.Stock * book.StorageCost;
                    book.totalCost += book.Stock * book.StorageCost;
                }

                book.AgeBook(1);
            }

            return storageCost;
        }

        public void DeactivateSelection(int playerIndex)
        {
            selection[playerIndex].active = !selection[playerIndex].active;
        }

        public List<Book> GetBooks()
        {
            return books;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (books.Count == 0) return;

            int counter = 0;
            int booksOnShelf = books.Count - activeShelf * rows * columns;
            if (booksOnShelf > rows * columns) booksOnShelf = rows * columns;
            int startIndex = activeShelf * rows * columns;

            Color tint;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (counter < booksOnShelf)
                    {
                        if (books[startIndex + counter].Stock > 0 || books[startIndex + counter].Owner == -1 || books[startIndex + counter].eBook) tint = Color.White;
                        else tint = Color.Gray;
                        books[startIndex + counter].Draw(spriteBatch, eBookTexture, position + new Vector2(columnSpacing * x, rowSpacing * y) * scale, tint, scale);
                        DrawStats(spriteBatch, books[startIndex + counter], position + new Vector2(columnSpacing * x, rowSpacing * y) * scale, font);
                    }
                    counter++;
                }
            }

            for (int i = 0; i < selectionTexture.Length; i++)
            {
                if (selection[i].active)
                {
                    int x = selection[i].x;
                    int y = selection[i].y;
                    Rectangle source = new Rectangle(0, 0, selectionTexture[i].Width, selectionTexture[i].Height);
                    spriteBatch.Draw(selectionTexture[i], position + new Vector2(columnSpacing * x, rowSpacing * y) * scale, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
            }
        }

        private void DrawStats(SpriteBatch spriteBatch, Book book, Vector2 position, SpriteFont font)
        {
            Rectangle source;
            Vector2 offset;

            int totalLength = (int)(Book.Height * scale * 0.9 - 10);
            int lossLength = 0;

            #region No Owner
            if (book.Owner == -1)
            {
                offset = new Vector2(-10, -10);
                totalLength = (int)(book.totalCost / (float)Book.maxPublishCost * totalLength);
                lossLength = totalLength - 2;
                source = new Rectangle(0, 0, 8, totalLength);
                spriteBatch.Draw(outlineTexture, position + new Vector2(Book.Width * scale, Book.Height * scale - totalLength) + offset, source, Color.White);
                source = new Rectangle(0, 0, 6, lossLength);
                spriteBatch.Draw(lossTexture, position + new Vector2(Book.Width * scale + 1, Book.Height * scale - totalLength + 1) + offset, source, Color.White);
            }
            #endregion
            else
            {
                if (book.totalProfit != 0)
                {
                    if (book.totalCost > book.totalProfit)
                    {
                        lossLength = totalLength - (int)((book.totalProfit / (float)(book.totalCost + book.totalProfit)) * totalLength);
                        if (lossLength > totalLength)
                            lossLength = 0;
                    }
                    else
                    {
                        if (book.totalCost == 0) lossLength = 0;
                        else if (book.totalCost == book.totalProfit) lossLength = (int)(totalLength * 0.5f);
                        else
                        {
                            lossLength = (int)((book.totalCost / (float)(book.totalCost + book.totalProfit)) * totalLength);
                        }
                    }
                    lossLength -= 2;
                }
                else lossLength = totalLength - 2;

                offset = new Vector2(-10, 8);
                source = new Rectangle(0, 0, 8, totalLength);
                spriteBatch.Draw(outlineTexture, position + new Vector2(Book.Width * scale, 0) + offset, source, Color.White);
                source = new Rectangle(0, 0, 6, totalLength - 2);
                spriteBatch.Draw(profitTexture, position + new Vector2(Book.Width * scale + 1, 1) + offset, source, Color.White);
                source = new Rectangle(0, 0, 6, lossLength);
                spriteBatch.Draw(lossTexture, position + new Vector2(Book.Width * scale + 1, 1) + offset, source, Color.White);

                if(!book.eBook) spriteBatch.DrawString(font, book.Stock.ToString(), position + new Vector2(8, Book.Height - 36) * scale, Color.White);
            }
            //spriteBatch.DrawString(font, book.GetGenre().ToString(), position + new Vector2(Book.Width / 2, Book.Height / 2) * scale, Color.White);
        }
    }
}
