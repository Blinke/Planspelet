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
        int playerIndex;

        Vector2 bookOffset = new Vector2(100, 100);
        float bookScale = 1;
        public Book activeBook;
        Texture2D eBookTexture;
        bool done;

        Button eButton;
        Button pButton;

        Vector2 tipOffset = new Vector2(75, 0);
        string pBookTip =
            "Physical books cost to print";
        string eBookTip =
            "E-books have only an initial cost,\n" +
            "but they generate lower demand.";

        public PublishMenu(TextureManager textureManager, Vector2 position, float scale, int playerIndex)
            :base(position, scale)
        {
            this.eButton = new Button(textureManager.eButtonTexture, position, new Vector2(200, 100));
            this.pButton = new Button(textureManager.pButtonTexture, position, new Vector2(100, 100));
            eBookTexture = textureManager.eBookTexture;
            this.playerIndex = playerIndex;
        }
        public PublishMenu(PublishMenu publishMenu)
            :base (publishMenu)
        {
            eButton = publishMenu.eButton;
            pButton = publishMenu.pButton;
            activeBook = new Book(publishMenu.activeBook);
            playerIndex = -1;
        }
        public override Tab Clone()
        {
            return new PublishMenu(this);
        }

        public void Open(Book activeBook)
        {
            this.activeBook = activeBook;
            done = false;
        }

        public override void ReceiveInput(Input input, int playerIndex)
        {
            if (input.Left) selection[playerIndex].x--;
            else if (input.Right) selection[playerIndex].x++;

            if (selection[playerIndex].x < 0) selection[playerIndex].x = 1;
            else if (selection[playerIndex].x > 1) selection[playerIndex].x = 0;

            if (input.ButtonA)
            {
                done = true;
            }
        }

        public bool FinalizeChoice()
        {
            if (!done) return false;

            if (selection[playerIndex].x == 1)
            {
                activeBook.eBook = true;
            }


            return true;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (activeBook != null)
            {
                int x = (int)((Book.Width + 20) * bookScale);
                activeBook.Draw(spriteBatch, position, Color.White, bookScale);
            }

            string tip = " ";
            if (selection[playerIndex].x == 0)
            {
                tip = pBookTip;
                pButton.Draw(spriteBatch, Color.Yellow);
                eButton.Draw(spriteBatch, Color.White);
            }
            else if (selection[playerIndex].x == 1)
            {
                tip = eBookTip;
                pButton.Draw(spriteBatch, Color.White);
                eButton.Draw(spriteBatch, Color.Yellow);
                spriteBatch.Draw(eBookTexture, position, new Rectangle(0, 0, eBookTexture.Width, eBookTexture.Height), Color.White, 0, Vector2.Zero, bookScale, SpriteEffects.None, 0);
            }
            spriteBatch.DrawString(font, tip, position + tipOffset, Color.Black);
        }
    }
}
