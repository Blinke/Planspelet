using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class BookManager
    {
        public Archive archive;

        public BookManager(Archive archive)
        {
            this.archive = archive;
            this.archive.SetPosition(new Vector2(500, 250));
        }

        public void Update(GameTime gameTime)
        {
            archive.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            archive.Draw(spriteBatch);  
        }
    }
}
