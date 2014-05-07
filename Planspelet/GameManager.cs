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

        Input input;
        Player[] players;
        BookManager bookManager;
        GameWindow window;

        Texture2D testBookTexture;

        Archive midArchive;
        PlayerPanel testPlayerPanel;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public enum TurnPhase
        {
            BookPicking,
            Browsing
        }

        public static TurnPhase phase;

        public GameManager(ContentManager content, GameWindow window)
        {
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            this.window = window;
            testBookTexture = content.Load<Texture2D>("book_template");
            players = new Player[1];
            bookManager = new BookManager(new Archive(3, 3, Vector2.Zero), content);

            GameStart();


            //För att testa bokvisualiseringen:
            //midArchive = new Archive(2, 5, new Vector2(100, 100));
            //testBookTexture = content.Load<Texture2D>("book");
            //for (int i = 0; i < 13; i++)
            //{
            //    midArchive.AddBook(new Book(testBookTexture));
            //}

            //testPlayerPanel = new PlayerPanel(new Vector2(500,100));
        }

        public void Update(GameTime gameTime)
        {
            window.Title = currentTurn.ToString();

            input.Update();
            bookManager.Update(gameTime);

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    switch (phase)
                    {
                        case TurnPhase.BookPicking:
                            bookManager.archive.ReceiveInput(input.GetPlayerInput(i));
                            if (bookManager.archive.selectedBook != null && !players[i].phaseDone)
                            {
                                players[i].AddBook(bookManager.archive.TransferSelectedBook());
                                bookManager.archive.selectedBook = null;
                                players[i].phaseDone = true;
                            }
                            break;

                        case TurnPhase.Browsing:
                            players[i].RecieveInput(input.GetPlayerInput(i));
                            players[i].Update(gameTime);
                            break;
                    }
                }
            }

            TurnPhaseCheck();

            //För att testa bokvisualiseringen:
            //previousKeyboardState = currentKeyboardState;
            //currentKeyboardState = Keyboard.GetState();

            //int x = 0;
            //int y = 0;
            //if (currentKeyboardState.IsKeyDown(Keys.W) && previousKeyboardState.IsKeyUp(Keys.W))
            //    y = -1;
            //else if (currentKeyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
            //    y = 1;
            //if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A)) 
            //    x = -1;
            //else if (currentKeyboardState.IsKeyDown(Keys.D) && previousKeyboardState.IsKeyUp(Keys.D)) 
            //    x = 1;

            //if (x != 0 || y != 0) midArchive.MoveSelection(x, y);

            //Book testBook;
            //if (midArchive.NumberOfBooks != 0 && currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            //{
            //    testPlayerPanel.AddBook(midArchive.TransferSelectedBook());
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (phase == TurnPhase.BookPicking)
            {
                bookManager.Draw(spriteBatch);    
            }
            //För att testa bokvisualiseringen
            //midArchive.Draw(spriteBatch);
            //testPlayerPanel.Draw(spriteBatch);

            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
        }

        private void TurnPhaseCheck()
        {
            bool doneSelecting = true;

            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].phaseDone)
                {
                    doneSelecting = false;
                    break;
                }
            }

            if (doneSelecting)
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

        private void GameStart()
        {
            currentTurn = 1;
            phase = TurnPhase.BookPicking;
            input = new Input(players.Length);

            for (int i = 0; i < players.Length; i++)
            {
                Archive playerArchive = new Archive(2, 5, Vector2.Zero);

                players[i] = new Player(playerArchive, i);
            }

            bookManager.archive.ClearArchive();

            bookManager.GenerateBooks();
        }
    }
}
