﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class Player
    {
        public bool phaseDone;
        Input input, prevInput;
        Archive archive;
        PublishMenu publishMenu;
        Tab activeTab;

        int playerID;

        public Player(TextureManager textureManager, int playerID)
        {
            archive = new Archive(GetPosition(playerID), 0.5f, 2, 5);
            this.playerID = playerID;
            publishMenu = new PublishMenu(textureManager, GetPosition(playerID), 0.5f);
            activeTab = archive;
        }

        public void Update(GameTime gameTime)
        {
            archive.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            archive.Draw(spriteBatch, font);
        }

        public void RecieveInput(Input newInput)
        {
            prevInput = input;
            input = newInput;

            if (GameManager.phase == GameManager.TurnPhase.Browsing && input.ButtonY)
                phaseDone = true;

            if (GameManager.phase == GameManager.TurnPhase.BookPicking && input.ButtonA)
            {
                archive.AddBook(archive.TransferSelectedBook());
            }

            archive.ReceiveInput(input);
        }

        private Vector2 GetPosition(int ID)
        { 
            Vector2 playerPosition = Vector2.Zero;

            switch (ID)
            {
                case 0:
                    playerPosition = new Vector2(50, 50);
                    break;
                case 1:
                    playerPosition = new Vector2(800, 50);
                    break;
                case 2:
                    playerPosition = new Vector2(50, 450);
                    break;
                case 3:
                    playerPosition = new Vector2(800, 450);
                    break;
            }


            return playerPosition;
        }

        public void AddBook(Book book)
        {
            archive.AddBook(book);
        }

        public void CopyArchive(Archive archive)
        {
            this.archive.CopyBooks(archive);
        }

        public void OpenPublishMenu()
        {
            activeTab = publishMenu;
            publishMenu.SetActiveBook(archive.GetSelectedBook());
        }

        public void OpenArchive()
        {
            activeTab = archive;
        }
    }

}
