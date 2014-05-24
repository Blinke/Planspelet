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
        Economy economyManager;

        Random rand;


        Texture2D backgroundTexture;

        public enum TurnPhase
        {
            BookPicking,
            Browsing,
            Selling
        }

        public static TurnPhase phase;

        public GameManager(GameWindow window, TextureManager textureManager, int numberOfPlayers)
        {
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            this.window = window;
            rand = new Random();

            players = new Player[numberOfPlayers];
            bookManager = new BookManager(textureManager);

            market = new Market(textureManager, new Vector2(20, 20), 31, 13, rand);
            economyManager = new Economy();

            GameStart(textureManager);

            backgroundTexture = textureManager.backgroundTexture;
        }

        public void Update(GameTime gameTime)
        {
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
                            if (players[i].phaseDone)
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

        public void Draw(SpriteBatch spriteBatch, SpriteFont fontSmall, SpriteFont fontLarge)
        {
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            if (phase == TurnPhase.BookPicking)
            {
                bookManager.Draw(spriteBatch, fontSmall);
            }

            foreach (Player player in players)
            {
                player.Draw(spriteBatch, fontSmall, fontLarge);
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
                        economyManager.SellBooks(market, players);
                        for (int i = 0; i < players.Length; i++)
                            bookManager.archive.DeactivateSelection(players[i].playerID);
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
            {
                players[i].phaseDone = false;
                players[i].RemoveOldBooks();
            }

            market.GenerateDemand(rand);
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
