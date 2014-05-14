﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planspelet
{
    class Archive: Tab
    {
        Texture2D[] selectionTexture;

        List<Book> books;
        int rows;
        int columns;
        public int NumberOfBooks { get { return books.Count; } }
        int numOfShelves = 1;

        const float rowSpacing = Book.Height + 10;
        const float columnSpacing = Book.Width + 10;

        int activeShelf = 0;

        //spriteBatch.Draw(selectionTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

        //double selectionTimer, selectionDelay;

        public Archive(TextureManager textureManager, Vector2 position, float scale, int rows, int columns)
            :base(position, scale)
        {
            books = new List<Book>();
            this.rows = rows;
            this.columns = columns;

            this.position = position;

            selectionTexture = new Texture2D[4];
            selectionTexture[0] = textureManager.selectionTexture;
            selectionTexture[1] = textureManager.selectionTexture;
            selectionTexture[2] = textureManager.selectionTexture;
            selectionTexture[3] = textureManager.selectionTexture;
        }

        //public void Update(GameTime gameTime)
        //{
        //    selectionTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        //}
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

            if (x > 0) selection[playerIndex].x++;
            else if (x < 0) selection[playerIndex].x--;
            if (y > 0) selection[playerIndex].y++;
            else if (y < 0) selection[playerIndex].y--;

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
                if (selection[playerIndex].y > fullRows - 1 && fullRows == rows)
                {
                    selection[playerIndex].y--;
                }
                else if (selection[playerIndex].y > fullRows - 1 && selection[playerIndex].x >= booksOnLastRow - 1)
                {
                    if (selection[playerIndex].y > fullRows || booksOnLastRow == 0)
                    {
                        selection[playerIndex].y--;
                    }
                    else
                    {
                        selection[playerIndex].x = booksOnLastRow - 1;
                    }
                }
                #endregion
            }
            if (y < 0)
            {
                if (selection[playerIndex].y < 0)
                {
                    selection[playerIndex].y++;
                }
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

            return books[selectionIndex];
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

        public void ClearArchive()
        {
            books.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            int counter = 0;
            int booksOnShelf = books.Count - activeShelf * rows * columns;
            if (booksOnShelf > rows * columns) booksOnShelf = rows * columns;

            int startIndex = activeShelf * rows * columns;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (counter < booksOnShelf)
                    {
                        //books[counter + activeShelf * rows * columns].isSelected = false;

                        //if (selection && x == selectionX && y == selection)
                        //    books[counter + activeShelf * rows * columns].isSelected = true;

                        books[startIndex + counter].Draw(spriteBatch, position + new Vector2(columnSpacing * x, rowSpacing * y) * scale, Color.White, scale, false);
                        //else
                        //    books[counter + activeShelf * rows * columns].Draw(spriteBatch, position + new Vector2(columnSpacing * x, rowSpacing * y) * scale, Color.White, scale);
                    }
                    counter++;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (selection[i].active)
                {
                    int x = selection[i].x;
                    int y = selection[i].y;
                    Rectangle source = new Rectangle(0, 0, selectionTexture[i].Width, selectionTexture[i].Height);
                    spriteBatch.Draw(selectionTexture[i], position + new Vector2(columnSpacing * x, rowSpacing * y) * scale, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    //spriteBatch.Draw(selectionTexture[i], position + new Vector2(columnSpacing * x, rowSpacing * y), new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}
