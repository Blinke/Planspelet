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

        public override void ReceiveInput(Input input, int playerIndex)
        {
            if (input.Left) selection[playerIndex].x--;
            else if (input.Right) selection[playerIndex].x++;

            if (selection[playerIndex].x < 0) selection[playerIndex].x = 1;
            else if (selection[playerIndex].x > 1) selection[playerIndex].x = 0;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (activeBook != null)
            {
                int x = (int)((Book.Width + 20) * bookScale);
                activeBook.Draw(spriteBatch, position, Color.White, bookScale);
                activeBook.DrawPublishInfo(spriteBatch, new Vector2(position.X + x, position.Y), scale, font);
            }

            //if (selection[playerIndex].x == 0)
            //{
            //    pButton.Draw(spriteBatch, Color.Yellow);
            //    eButton.Draw(spriteBatch, Color.White);
            //}
            //else if (selection[playerIndex].x == 1)
            //{
            //    pButton.Draw(spriteBatch, Color.White);
            //    eButton.Draw(spriteBatch, Color.Yellow);
            //}
        }
    }
}
