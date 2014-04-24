using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planspelet
{
    class Archive
    {
        List<Book> books;
        int rows;
        int columns;
        int numOfBooks;
        int numOfShelves;

        Vector2 position;
        float rowSpacing = 110;
        float columnSpacing = 80;

        int activeShelf = 0;
        int selectionX = 0;
        int selectionY = 0;

        double selectionTimer, selectionDelay;


        public Archive(int rows, int columns, Vector2 position)
        {
            books = new List<Book>();
            this.rows = rows;
            this.columns = columns;
            numOfBooks = 0;
            numOfShelves = 0;

            selectionDelay = 200;

            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
            selectionTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void AddBook(Book book)
        {
            if (numOfBooks + 1 > rows * columns * numOfShelves) numOfShelves++;
            books.Add(book);
            numOfBooks++;
        }

        /// <summary>
        /// Moves the selection-cursor in the given directions, the method only takes into account whether the parameters are positive, negative or zero. The selection only moves one step.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveSelection(float x, float y)
        {
            if (x > 0) selectionX++;
            else if (x < 0) selectionX--;
            if (y > 0) selectionY++;
            else if (y < 0) selectionY--;

            int booksOnShelf = numOfBooks - rows * columns * (numOfShelves - 1);
            int booksOnLastRow = booksOnShelf % columns;
            int fullRows = booksOnShelf / columns;

            if (x > 0)
            {
                if (selectionY == fullRows && selectionX > booksOnLastRow - 1)
                {
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
                //This was moving the selection to the first book whenever you pressed up at the top row
                selectionY = numOfShelves;
                if (selectionX > booksOnLastRow - 1)
                    selectionX = booksOnLastRow - 1;
            }
        }

        public void ReceiveInput(GamePadState gPadState)
        {
            if (selectionTimer <= 0 && gPadState.ThumbSticks.Left != Vector2.Zero)
            {
                MoveSelection(gPadState.ThumbSticks.Left.X, -gPadState.ThumbSticks.Left.Y);
                selectionTimer = selectionDelay;
            }
        }

        public Book ReturnSelection()
        {
            int selection = activeShelf * selectionX * selectionY + selectionX + selectionY * columns;
            return books[selection];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int counter = 0;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (counter < books.Count)
                    {
                        if (x == selectionX && y == selectionY)
                            books[counter].Draw(spriteBatch, position + new Vector2(columnSpacing * x, rowSpacing * y), Color.Yellow);
                        else
                            books[counter].Draw(spriteBatch, position + new Vector2(columnSpacing * x, rowSpacing * y), Color.White);
                    }
                    counter++;
                }
            }
        }
    }
}
