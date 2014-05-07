using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Planspelet
{
    class BookManager
    {
        public Archive archive;
        List<Texture2D> bookTexture, detailTexture;
        Texture2D selectionTexture;

        public BookManager(TextureManager textureManager)
        {
            archive = new Archive(new Vector2(530, 250), 0.75f, 3, 3);
            this.bookTexture = textureManager.bookTexture;
            this.detailTexture = textureManager.detailTexture;
            this.selectionTexture = textureManager.selectionTexture;
        }

        public void Update(GameTime gameTime)
        {
            archive.Update(gameTime);
        }

        public void ReceiveInput(Input input, Player player)
        {
            archive.ReceiveInput(input);

            if (GameManager.phase == GameManager.TurnPhase.BookPicking && input.ButtonA)
            {
                player.AddBook(archive.TransferSelectedBook());
                player.phaseDone = true;
            }

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            archive.Draw(spriteBatch, font);  
        }

        public void GenerateBooks()
        {
            Random rnd = new Random();

            for (int i = 0; i < 9; i++)
            {
                int bookRnd = rnd.Next(0, bookTexture.Count);
                int detailRnd = rnd.Next(0, detailTexture.Count);
                archive.AddBook(new Book(bookTexture[bookRnd], detailTexture[detailRnd], selectionTexture, "bla"));
            }
        }
    }
}
