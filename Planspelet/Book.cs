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
        Texture2D baseTexture;

        public Book(Texture2D baseTexture)
        {
            this.baseTexture = baseTexture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tint)
        {
            spriteBatch.Draw(baseTexture, position, tint);
        }
    }
}
