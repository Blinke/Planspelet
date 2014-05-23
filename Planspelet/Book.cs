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

        Texture2D baseTexture, detailTexture;
        public const int Width = 70;
        public const int Height = 100;
        //public int Width { get { return baseTexture.Width; } }
        //public int Height { get { return baseTexture.Height; } }
        public int Stock { get; set; }
        public int Owner { get; set; }
        public int SalePrice { get; private set; }
        public int BookAge { get; set; }
        public int PrintSize { get; set; }
        public float Profitablity { get; private set; }
        float ageFactor;
        Genre genre;
        public bool eBook;
        public bool inPrint = true;

        int publishingCost;
        public int PrintCost { get; set; }
        int storageCost;
        public int totalCost = 0;
        public int totalProfit = 0;

        public Book(Texture2D baseTexture, Texture2D detailTexture, Random rnd, Genre genre)//(Texture2D baseTexture, Texture2D detailTexture, Texture2D selectionTexture[], string title)
        {
            Owner = -1;
            PrintSize = 20;
            Profitablity = 1;
            SalePrice = 150;
            ageFactor = 0.95f;
            publishingCost = rnd.Next(2, 8) * 100;
            PrintCost = 300;
            totalCost += publishingCost;
            this.baseTexture = baseTexture;
            this.detailTexture = detailTexture;
            this.genre = genre;
        }

        public Book(Book book)
        {
            baseTexture = book.baseTexture;
            detailTexture = book.detailTexture;
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
            Profitablity = 1 - (ageFactor * BookAge);

            Profitablity = 0.1f;

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

        public void Reprint()
        {
            Stock += PrintSize;
            totalCost += PrintCost;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tint, float scale)
        {
            spriteBatch.Draw(baseTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(detailTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
