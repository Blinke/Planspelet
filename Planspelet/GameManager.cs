using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Planspelet
{
    class GameManager
    {
        Input input;
        Player[] players;
        BookManager bookManager;

        Texture2D testBookTexture;

        Archive midArchive;
        PlayerPanel testPlayerPanel;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public GameManager(ContentManager content)
        {
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            testBookTexture = content.Load<Texture2D>("book_template");
            players = new Player[4];
            GameStart();

            input = new Input(players.Length);

            //För att testa bokvisualiseringen:
            midArchive = new Archive(2, 5, new Vector2(100, 100));
            testBookTexture = content.Load<Texture2D>("book");
            for (int i = 0; i < 13; i++)
            {
                midArchive.AddBook(new Book(testBookTexture));
            }

            testPlayerPanel = new PlayerPanel(new Vector2(500,100));
        }

        public void Update(GameTime gameTime)
        {
            input.Update();
            bookManager.Update(gameTime);

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    bookManager.archive.ReceiveInput(input.GetPlayerInput(i));

                    if (bookManager.archive.selectedBook != null)
                    {
                        players[i].AddNewBook(bookManager.archive.selectedBook);
                        bookManager.archive.selectedBook = null;
                        return;
                    }

                    players[i].RecieveInput(input.GetPlayerInput(i));
                    players[i].Update(gameTime);
                }
            }

            //För att testa bokvisualiseringen:
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            int x = 0;
            int y = 0;
            if (currentKeyboardState.IsKeyDown(Keys.W) && previousKeyboardState.IsKeyUp(Keys.W))
                y = -1;
            else if (currentKeyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
                y = 1;
            if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A)) 
                x = -1;
            else if (currentKeyboardState.IsKeyDown(Keys.D) && previousKeyboardState.IsKeyUp(Keys.D)) 
                x = 1;

            if (x != 0 || y != 0) midArchive.MoveSelection(x, y);

            //Book testBook;
            if (midArchive.NumberOfBooks != 0 && currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                testPlayerPanel.AddBook(midArchive.TransferSelectedBook());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            bookManager.Draw(spriteBatch);
            //För att testa bokvisualiseringen
            midArchive.Draw(spriteBatch);
            testPlayerPanel.Draw(spriteBatch);

            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
        }

        private void GameStart()
        {
            input = new Input(players.Length);
            for (int i = 0; i < players.Length; i++)
            {
                Archive playerArchive = new Archive(5, 5, Vector2.Zero);

                //for (int j = 0; j < 10; j++)
                //    playerArchive.AddBook(new Book(testBookTexture));

                players[i] = new Player(playerArchive, i);
            }

            Archive bookArchive = new Archive(2, 3, Vector2.Zero);

            for (int i = 0; i < 9; i++)
                bookArchive.AddBook(new Book(testBookTexture));

            bookManager = new BookManager(bookArchive);

        }
    }
}
