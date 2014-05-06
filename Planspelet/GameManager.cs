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

        SpriteFont font;

        Archive midArchive;
        Texture2D testBookTexture;

        PlayerPanel testPlayerPanel;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public GameManager(ContentManager content, TextureManager textureManager)
        {
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            players = new Player[4];
            input = new Input(players.Length);
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
            }

            font = content.Load<SpriteFont>("SpriteFont1");

            //För att testa bokvisualiseringen:
            midArchive = new Archive(new Vector2(100, 100), 0.5f, 2, 5);
            testBookTexture = content.Load<Texture2D>("book");
            for (int i = 0; i < 9; i++)
            {
                midArchive.AddBook(new Book(testBookTexture, "title"));
            }

            testPlayerPanel = new PlayerPanel(textureManager, new Vector2(500,100));
            testPlayerPanel.CopyArchive(midArchive);
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

            bool Up = false;
            bool Down = false;
            bool Left = false;
            bool Right = false;

            if (currentKeyboardState.IsKeyDown(Keys.W) && previousKeyboardState.IsKeyUp(Keys.W))
                Up = true;
            else if (currentKeyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
                Down = true;
            if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
                Left = true;
            else if (currentKeyboardState.IsKeyDown(Keys.D) && previousKeyboardState.IsKeyUp(Keys.D))
                Right = true;

            testPlayerPanel.Update(Up, Down, Left, Right);

            //Book testBook;
            if (midArchive.NumberOfBooks != 0 && currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                testPlayerPanel.AddBook(midArchive.TransferSelectedBook());
            }

            if (currentKeyboardState.IsKeyDown(Keys.O) && previousKeyboardState.IsKeyUp(Keys.O))
            {
                testPlayerPanel.OpenPublishMenu();
            }
            if (currentKeyboardState.IsKeyDown(Keys.L) && previousKeyboardState.IsKeyUp(Keys.L))
            {
                testPlayerPanel.OpenArchive();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //För att testa bokvisualiseringen
            midArchive.Draw(spriteBatch, font);
            testPlayerPanel.Draw(spriteBatch, font);

            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
        }

    }
}
