using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Planspelet
{
    enum Genre
    {
        Drama = 0,
        NonFiction = 1,
        Mystery = 2,
        Fantasy = 3,
        Horror = 4,
    };

    class Book
    {
        public static int numberOfGenres = Enum.GetNames(typeof(Genre)).Length;
        public static int maxStock = 20;
        public static int maxAge = 8;

        Texture2D baseTexture, detailTexture;
        public const int Width = 70;
        public const int Height = 100;

        public int Stock { get; set; }
        public int Owner { get; set; }
        public int SalePrice { get; private set; }
        public int BookAge { get; set; }
        public int PrintSize { get; set; }
        public float Profitablity { get; private set; }
        public float BaseProfitablity { get; private set; }
        public int SellChance { get; private set; }
        public int StorageCost { get; private set; }
        float ageFactor;
        Genre genre;
        public bool eBook;
        public bool inPrint = true;

        int publishingCost;
        public const int maxPublishCost = 8;
        public int PrintCost { get; set; }
        public int totalCost = 0;
        public int totalProfit = 0;

        public Book(Texture2D baseTexture, Texture2D detailTexture, Random rnd, Genre genre)
        {
            Owner = -1;
            PrintSize = 5;
            Profitablity = 1;
            SalePrice = 150;
            ageFactor = 0.05f;
            publishingCost = rnd.Next(2, maxPublishCost) * 100;
            PrintCost = 300;
            StorageCost = 50;
            totalCost += publishingCost;
            this.baseTexture = baseTexture;
            this.detailTexture = detailTexture;
            this.genre = genre;

            SetProfitability(publishingCost);
        }

        public Book(Book book)
        {
            Owner = book.Owner;
            PrintSize = book.PrintSize;
            Profitablity = book.Profitablity;
            SalePrice = book.SalePrice;
            ageFactor = book.ageFactor;
            publishingCost = book.publishingCost;
            PrintCost = book.PrintCost;
            totalCost = book.totalCost;
            baseTexture = book.baseTexture;
            detailTexture = book.detailTexture;
            genre = book.genre;
        }
        public Genre GetGenre()
        {
            return genre;
        }

        public int GetCost()
        {
            if (eBook)
                return (int)(publishingCost * 0.8f);
            else
                return publishingCost;
        }

        public void CalcProfitablity()
        {
            Profitablity = BaseProfitablity * (1 - (ageFactor * BookAge));

            if (eBook)
            {
                Profitablity *= 0.5f;
            }
        }

        public void AgeBook(int age)
        {
            BookAge += age;

            if (BookAge == 10)
            {
                BookAge = 9;
            }
        }

        public int Reprint()
        {
            int tempPrintCost = 0;
            if (Stock + PrintSize > Book.maxStock)
            {
                tempPrintCost = PrintCost * ((Book.maxStock - Stock) / PrintSize);
                Stock = Book.maxStock;
            }
            else
            {
                tempPrintCost = PrintCost;
                Stock += PrintSize;
            }
            totalCost += tempPrintCost;
            return tempPrintCost;
        }

        private void SetProfitability(int publishCost)
        {
            switch (publishCost)
            { 
                case 200:
                    BaseProfitablity = 0.95f;
                    SellChance = 50;
                    break;
                case 300:
                    BaseProfitablity = 1f;
                    SellChance = 60;
                    break;
                case 400:
                    BaseProfitablity = 1f;
                    SellChance = 70;
                    break;
                case 500:
                    BaseProfitablity = 1.1f;
                    SellChance = 80;
                    break;
                case 600:
                    BaseProfitablity = 1.2f;
                    SellChance = 90;
                    break;
                case 700:
                    BaseProfitablity = 1.3f;
                    SellChance = 100;
                    break;
            }


        }

        public void Draw(SpriteBatch spriteBatch, Texture2D eBookTexture, Vector2 position, Color tint, float scale)
        {
            spriteBatch.Draw(baseTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(detailTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            if (eBook) spriteBatch.Draw(eBookTexture, position, new Rectangle(0, 0, eBookTexture.Width, eBookTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tint, float scale)
        {
            spriteBatch.Draw(baseTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(detailTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
