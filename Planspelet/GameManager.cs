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
        Market market;
        GameWindow window;

        SpriteFont font;

        public enum TurnPhase
        {
            BookPicking,
            Browsing,
            Selling
        }

        public static TurnPhase phase;

        public GameManager(ContentManager content, GameWindow window, TextureManager textureManager)
        {
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            this.window = window;
            players = new Player[4];
            bookManager = new BookManager(textureManager);

            market = new Market(textureManager, new Vector2(20, 20), 31, 13);

            GameStart(textureManager);

            font = content.Load<SpriteFont>("SpriteFont1");
        }

        public void Update(GameTime gameTime)
        {
            window.Title = phase.ToString();

            inputManager.Update(gameTime);

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    if (phase != TurnPhase.BookPicking) players[i].RecieveInput(inputManager.GetPlayerInput(i));
                    //players[i].RecieveInput(inputManager.GetPlayerInput(i));

                    switch (phase)
                    {
                        case TurnPhase.BookPicking:
                            if (!players[i].phaseDone)
                                bookManager.ReceiveInput(inputManager.GetPlayerInput(i), players[i]);
                            break;

                        case TurnPhase.Browsing:
                            players[i].Update(gameTime);
                            break;

                        case TurnPhase.Selling:
                            //Call each players selling method or whatever
                            players[i].phaseDone = true;
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
            market.Draw(spriteBatch);
        }

        private void TurnPhaseCheck()
        {
            if (players.All(p => p.phaseDone))
            {
                for (int i = 0; i < players.Length; i++)
                    players[i].phaseDone = false;

                switch (phase)
                { 
                    case TurnPhase.BookPicking:
                        phase = TurnPhase.Browsing;
                        break;

                    case TurnPhase.Browsing:
                        phase = TurnPhase.Selling;
                        break;

                    case TurnPhase.Selling:
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
