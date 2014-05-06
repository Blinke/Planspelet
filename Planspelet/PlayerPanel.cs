using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Planspelet
{
    class PlayerPanel
    {
        GamePadState gPadState, prevgPadState;
        Archive archive;
        Vector2 position;

        public PlayerPanel(Vector2 position)
        {
            this.position = position;
            archive = new Archive(2, 5, position);
        }

        public void ReceiveInput(GamePadState newgPadState)
        {
            prevgPadState = gPadState;
            gPadState = newgPadState;

            archive.ReceiveInput(gPadState);
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
