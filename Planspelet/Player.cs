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

        Vector2 position;
        Tab activeTab;

        Texture2D background;

        int budget;
        public int salesMade;

        public int playerID { get; private set; }

        public Player(TextureManager textureManager, int playerID)
        {
            position = GetPosition(playerID);
            archive = new Archive(textureManager, position, 0.75f, 2, 5);
            this.playerID = playerID;
            publishMenu = new PublishMenu(textureManager, position, 0.75f, playerID);
            activeTab = archive;

            background = textureManager.playerBackground;

            budget = 0;
        }

        public void Update(GameTime gameTime)
        {
            //archive.Update(gameTime);
        }

        public void RecieveInput(Input newInput)
        {
            prevInput = input;
            input = newInput;

            if (GameManager.phase == GameManager.TurnPhase.Browsing && activeTab is Archive && input.ButtonY)
                phaseDone = true;

            activeTab.ReceiveInput(input, playerID);

            if (activeTab is PublishMenu)
            {
                if (publishMenu.FinalizeChoice())
                {
                    budget -= publishMenu.activeBook.GetCost();
                    OpenArchive();
                }
            }
            else if (activeTab is Archive)
            {
                if (input.ButtonA)
                {
                    if (archive.GetSelectedBook(playerID).Stock == 0 && !archive.GetSelectedBook(playerID).eBook)
                    {
                        archive.GetSelectedBook(playerID).Reprint();
                        budget -= archive.GetSelectedBook(playerID).PrintCost;
                    }
                }
            }
        }

        private Vector2 GetPosition(int ID)
        { 
            Vector2 playerPosition = Vector2.Zero;

            switch (ID)
            {
                case 0:
                    playerPosition = new Vector2(100, 75);
                    break;
                case 1:
                    playerPosition = new Vector2(850, 75);
                    break;
                case 2:
                    playerPosition = new Vector2(100, 450);
                    break;
                case 3:
                    playerPosition = new Vector2(850, 450);
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
            publishMenu.Open(archive.GetSelectedBook(playerID));
        }

        public void OpenArchive()
        {
            activeTab = archive;
        }

        public List<Book> GetBooksForSale()
        {
            List<Book> tempList = new List<Book>();

            tempList.AddRange(archive.GetBooks().Where(b => b.Stock > 0));

            return tempList;
        }

        public void BookSold(Book book)
        {
            if (!book.eBook)
                book.Stock -= 1;

            salesMade += 1;
            int profit = (int)(book.SalePrice * book.Profitablity);
            book.totalProfit += profit;
            budget += profit;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(background, position, Color.White);
            activeTab.Draw(spriteBatch, font);
            spriteBatch.DrawString(font, budget.ToString(), position, Color.White);
            //archive.Draw(spriteBatch, font);
        }
    }

}
