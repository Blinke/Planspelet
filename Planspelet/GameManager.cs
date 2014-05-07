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
        int currentTurn;

        InputManager inputManager;
        Player[] players;
        BookManager bookManager;
        GameWindow window;

        SpriteFont font;

        Texture2D testBookTexture;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public enum TurnPhase
        {
            BookPicking,
            Browsing
        }

        public static TurnPhase phase;

        public GameManager(ContentManager content, GameWindow window, TextureManager textureManager)
        {
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            this.window = window;
            testBookTexture = content.Load<Texture2D>("book_template");
            players = new Player[4];
            bookManager = new BookManager(new Archive(Vector2.Zero, 1f, 3, 3), content);

            GameStart(textureManager);

            font = content.Load<SpriteFont>("SpriteFont1");
        }

        public void Update(GameTime gameTime)
        {
            window.Title = currentTurn.ToString();

            inputManager.Update(gameTime);
            bookManager.Update(gameTime);

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    switch (phase)
                    {
                        case TurnPhase.BookPicking:
                            bookManager.ReceiveInput(inputManager.GetPlayerInput(0));
                            if (bookManager.archive.selectedBook != null && !players[i].phaseDone)
                            {
                                players[i].AddBook(bookManager.archive.TransferSelectedBook());
                                bookManager.archive.selectedBook = null;
                                players[i].phaseDone = true;
                            }
                            break;

                        case TurnPhase.Browsing:
                            players[i].RecieveInput(inputManager.GetPlayerInput(i));
                            players[i].Update(gameTime);
                            break;
                    }
                }
            }

            TurnPhaseCheck();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (phase == TurnPhase.BookPicking)
            {
                bookManager.Draw(spriteBatch, font);    
            }

            foreach (Player player in players)
            {
                player.Draw(spriteBatch, font);
            }
        }

        private void TurnPhaseCheck()
        {
            if (players.Count(p => p.phaseDone) == players.Length)
            {
                for (int i = 0; i < players.Length; i++)
                    players[i].phaseDone = false;

                switch (phase)
                { 
                    case TurnPhase.BookPicking:
                        phase = TurnPhase.Browsing;
                        break;

                    case TurnPhase.Browsing:
                        phase = TurnPhase.BookPicking;
                        NextTurn();
                        break;
                }
            }
        }

        private void NextTurn()
        { 
            currentTurn++;
            phase = TurnPhase.BookPicking;

            bookManager.archive.ClearArchive();
            bookManager.GenerateBooks();

            for (int i = 0; i < players.Length; i++)
                if (players[i] != null)
                    players[i].phaseDone = false;
        }

        private void GameStart(TextureManager textureManager)
        {
            currentTurn = 1;
            phase = TurnPhase.BookPicking;
            inputManager = new InputManager(players.Length);

            for (int i = 0; i < players.Length; i++)
                players[i] = new Player(textureManager, i);

            bookManager.archive.ClearArchive();

            bookManager.GenerateBooks();
        }
    }
}
