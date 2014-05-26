using System;
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
        Vector2 offset = new Vector2(0, 30);
        public Tab activeTab { get; private set; }

        Texture2D background;
        Texture2D doneTexture;


        int budget;
        public int salesMade;

        public int playerID { get; private set; }

        public Player(TextureManager textureManager, int playerID)
        {
            position = GetPosition(playerID);
            archive = new Archive(textureManager, position + offset, 0.6f, 2, 5);
            this.playerID = playerID;
            publishMenu = new PublishMenu(textureManager, position + offset, 0.75f, playerID);
            activeTab = archive;

            background = textureManager.playerBackground;
            doneTexture = textureManager.doneTexture;

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
                    if (archive.GetSelectedBook(playerID) != null && !archive.GetSelectedBook(playerID).eBook)
                    {
                        budget -= archive.GetSelectedBook(playerID).Reprint();
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
                    playerPosition = new Vector2(95, 95);
                    break;
                case 1:
                    playerPosition = new Vector2(875, 95);
                    break;
                case 2:
                    playerPosition = new Vector2(95, 430);
                    break;
                case 3:
                    playerPosition = new Vector2(875, 430);
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
            publishMenu.Open(archive.GetLastBook());
        }

        public void OpenArchive()
        {
            activeTab = archive;
        }

        public void RemoveOldBooks()
        {
            archive.RemoveOldBooks(playerID);
        }

        public void AgeBooks()
        {
            budget -= archive.GetStorageCost();
        }

        public List<Book> GetBooksForSale(bool eBook)
        {
            List<Book> tempList = new List<Book>();

            if (!eBook)
                tempList.AddRange(archive.GetBooks().Where(b => b.Stock > 0));
            else
                tempList.AddRange(archive.GetBooks().Where(b => b.eBook));

            return tempList;
        }

        public bool BookSold(Book book)
        {
            if (book.Stock <= 0 && !book.eBook) return false;
            if (!book.eBook)
            {
                book.Stock -= 1;
            }

            salesMade += 1;
            int profit = (int)(book.SalePrice * book.Profitablity);
            book.totalProfit += profit;
            budget += profit;
            return true;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont fontSmall, SpriteFont fontLarge)
        {
            //spriteBatch.Draw(background, position, Color.White);
            activeTab.Draw(spriteBatch, fontSmall);
            spriteBatch.DrawString(fontLarge, "Balance Sheet: " + budget.ToString(), position, Main.FontColor);

            if (phaseDone && GameManager.phase == GameManager.TurnPhase.Browsing)
            {
                spriteBatch.Draw(doneTexture, new Vector2(position.X + 50, position.Y - 10), Color.White);
            }
        }
    }

}
