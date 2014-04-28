using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Planspelet
{
    class PlayerPanel
    {
        Archive archive;
        Vector2 position;

        public PlayerPanel(Vector2 position)
        {
            this.position = position;
            archive = new Archive(2, 5, position);
        }

        public void AddBook(Book book)
        {
            archive.AddBook(book);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            archive.Draw(spriteBatch);
        }
    }
}
