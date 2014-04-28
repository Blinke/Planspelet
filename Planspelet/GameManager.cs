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

        Archive testArchive;
        Texture2D testBookTexture;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public GameManager(ContentManager content)
        {
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            players = new Player[4];
            input = new Input(players.Length);
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
            }

            //För att testa bokvisualiseringen:
            testArchive = new Archive(2, 5, new Vector2(100, 100));
            testBookTexture = content.Load<Texture2D>("book");
            for (int i = 0; i < 13; i++)
            {
                testArchive.AddBook(new Book(testBookTexture));
            }
        }

        public void Update(GameTime gameTime)
        {
            input.Update();

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
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

            if (x != 0 || y != 0) testArchive.MoveSelection(x, y);

            Book testBook;
            if (testArchive.NumberOfBooks != 0 && currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                testBook = testArchive.TransferSelectedBook();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //För att testa bokvisualiseringen
            testArchive.Draw(spriteBatch);

            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
        }

    }
}
