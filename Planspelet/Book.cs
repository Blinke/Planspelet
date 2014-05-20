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
        Mystert = 2,
    };

    class Book
    {
        Texture2D baseTexture, detailTexture;
        //string title; // really needed?
        Genre genre;
        public bool eBook;

        public const int Width = 70;
        public const int Height = 100;
        //public int Width { get { return baseTexture.Width; } }
        //public int Height { get { return baseTexture.Height; } }

        int totalCost;
        int totalProfit;

        public Book(Texture2D baseTexture, Texture2D detailTexture, string title)//(Texture2D baseTexture, Texture2D detailTexture, Texture2D selectionTexture[], string title)
        {
            this.baseTexture = baseTexture;
            this.detailTexture = detailTexture;
            //this.selectionTexture = selectionTexture;
            //this.title = title;
        }

        public Book(Book book)
        {
            baseTexture = book.baseTexture;
            //title = book.title;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tint, float scale, bool drawStatistics)
        {
            spriteBatch.Draw(baseTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(detailTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            //if (isSelected)
            //{
            //    spriteBatch.Draw(selectionTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            //}

            if (drawStatistics)
            {
            }
        }

        public void DrawPublishInfo(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteFont font)
        {
            //spriteBatch.DrawString(font, title, position, Color.White);
            // Draw title and any other information interesting for publishing/printing
        }
    }
}
