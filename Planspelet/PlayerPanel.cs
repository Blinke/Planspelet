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
        Vector2 position;

        Archive archive;


        int activeTab = 0;

        public PlayerPanel(Vector2 position, Archive archive)
        {
            this.position = position;
            this.archive = archive;
            this.archive.SetPosition(position);
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
