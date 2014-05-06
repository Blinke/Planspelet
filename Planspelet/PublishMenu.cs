using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class PublishMenu: Tab
    {
        Vector2 bookOffset = new Vector2(100, 100);
        float bookScale = 1;
        Book activeBook;

        Button eButton;
        Button pButton;

        public PublishMenu(TextureManager textureManager, Vector2 position, float scale)
            :base(position, scale)
        {
            this.eButton = new Button(textureManager.eBookTexture, position, new Vector2(200, 100));
            this.pButton = new Button(textureManager.pBookTexture, position, new Vector2(100, 100));
        }
        public PublishMenu(PublishMenu publishMenu)
            :base (publishMenu)
        {
            eButton = publishMenu.eButton;
            pButton = publishMenu.pButton;
            activeBook = new Book(publishMenu.activeBook);
        }
        public override Tab Clone()
        {
            return new PublishMenu(this);
        }

        public void SetActiveBook(Book book)
        {
            activeBook = book;
        }

        public override void Update(bool up, bool down, bool left, bool right)
        {
            if (left) selectionX--;
            else if (right) selectionX++;

            if (selectionX < 0) selectionX = 1;
            else if (selectionX > 1) selectionX = 0;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (activeBook != null)
            {
                int x = (int)((activeBook.Width + 20) * bookScale);
                activeBook.Draw(spriteBatch, position, Color.White, bookScale);
                activeBook.DrawPublishInfo(spriteBatch, new Vector2(position.X + x, position.Y), scale, font);
            }

            if (selectionX == 0)
            {
                pButton.Draw(spriteBatch, Color.Yellow);
                eButton.Draw(spriteBatch, Color.White);
            }
            else if (selectionX == 1)
            {
                pButton.Draw(spriteBatch, Color.White);
                eButton.Draw(spriteBatch, Color.Yellow);
            }
        }
    }
}
