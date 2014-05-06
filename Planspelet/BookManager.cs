﻿using System;
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
        ContentManager content;
        Texture2D testBookTexture;

        public BookManager(Archive archive, ContentManager content)
        {
            this.archive = archive;
            this.archive.SetPosition(new Vector2(500, 250));
            this.content = content;
            testBookTexture = content.Load<Texture2D>("book_template");
        }

        public void Update(GameTime gameTime)
        {
            archive.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            archive.Draw(spriteBatch);  
        }

        public void GenerateBooks()
        {
            Random rnd = new Random();

            for (int i = 0; i < 2 + rnd.Next(2, 7); i++)
                archive.AddBook(new Book(testBookTexture));
        }
    }
}
