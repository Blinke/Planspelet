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

        public GameManager(ContentManager content)
        {
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            testBookTexture = content.Load<Texture2D>("book_template");
            players = new Player[4];
            GameStart();
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            bookManager.Draw(spriteBatch);

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
                Archive playerArchive = new Archive(5, 5);

                //for (int j = 0; j < 10; j++)
                //    playerArchive.AddBook(new Book(testBookTexture));

                players[i] = new Player(playerArchive, i);
            }

            Archive bookArchive = new Archive(2, 3);

            for (int i = 0; i < 9; i++)
                bookArchive.AddBook(new Book(testBookTexture));

            bookManager = new BookManager(bookArchive);

        }
    }
}
