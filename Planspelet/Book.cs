using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Planspelet
{
    class Book
    {
        Texture2D baseTexture, detailTexture, selectionTexture;
        string title;
        public bool isSelected;

        public int Width { get { return baseTexture.Width; } }
        public int Height { get { return baseTexture.Height; } }

        public Book(Texture2D baseTexture, Texture2D detailTexture, Texture2D selectionTexture, string title)
        {
            this.baseTexture = baseTexture;
            this.detailTexture = detailTexture;
            this.selectionTexture = selectionTexture;
            this.title = title;
        }
        public Book(Book book)
        {
            baseTexture = book.baseTexture;
            title = book.title;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tint, float scale)
        {
            spriteBatch.Draw(baseTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(detailTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), tint, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            if (isSelected)
	        {
		        spriteBatch.Draw(selectionTexture, position, new Rectangle(0, 0, baseTexture.Width, baseTexture.Height), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
	        }
        }

        public void DrawPublishInfo(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteFont font)
        {
            spriteBatch.DrawString(font, title, position, Color.White);
            // Draw title and any other information interesting for publishing/printing
        }
    }
}
