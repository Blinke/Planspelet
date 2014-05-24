using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class Market
    {
        private class Demand
        {
            public Genre genre;
            public bool eBook;
            public Texture2D texture;

            public Demand(Genre genre, bool eBook, Texture2D texture)
            {
                this.genre = genre;
                this.eBook = eBook;
                this.texture = texture;
            }
            public void Draw(SpriteBatch spriteBatch, Texture2D eBookTexture, Vector2 position, float scale)
            {
                Rectangle source = new Rectangle(0, 0, Book.Width, Book.Height);
                spriteBatch.Draw(texture, position, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                if (eBook)
                    spriteBatch.Draw(eBookTexture, position, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }

        #region Members
        public const int minEBookChance = 10;
        public const int maxEBookChance = 30;
        int currentEbookChance = 20;

        Demand[] demand;
        List<int> emptyPos;

        Vector2 position;
        Texture2D[] textures;
        Texture2D eBookTexture;
        float scale = 0.5f;

        int parsingX = 10;
        int parsingY = 6;

        int X_max;
        int Y_max;
        int totalNumber;
        #endregion

        public Market(TextureManager textureManager, Vector2 position, int X_max, int Y_max, Random rand)
        {
            this.position = position;
            textures = new Texture2D[Book.numberOfGenres];
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = textureManager.bookTexture[i];
            }
            eBookTexture = textureManager.eBookTexture;

            this.X_max = X_max;
            this.Y_max = Y_max;
            totalNumber = X_max * 2 + Y_max * 2 - 4;

            demand = new Demand[totalNumber];

            emptyPos = new List<int>();
            Fill(rand);
        }
        private void Fill(Random rand)
        {
            for (int i = 0; i < demand.Length; i++)
            {
                Genre genre = (Genre)(rand.Next(0, Book.numberOfGenres));
                demand[i] = new Demand(genre, false, textures[(int)genre]);
            }
        }
        public void GenerateDemand(Random rand)
        {
            while (emptyPos.Count != 0)
            {
                int randIndex = rand.Next(0, emptyPos.Count - 1);
                Genre genre = (Genre)(rand.Next(0, Book.numberOfGenres));
                bool eBook = false;
                int randEbook = rand.Next(0, 100);
                if (randEbook < currentEbookChance)
                {
                    eBook = true;
                    currentEbookChance -= 5;
                    if (currentEbookChance < minEBookChance) currentEbookChance = minEBookChance;
                }
                else
                {
                    currentEbookChance += 5;
                    if (currentEbookChance > maxEBookChance) currentEbookChance = maxEBookChance;
                }

                demand[emptyPos[randIndex]] = new Demand(genre, eBook, textures[(int)genre]);
                emptyPos.RemoveAt(randIndex);  
            }
        }
        public void RemoveDemand(Genre genre, int number)
        {
            for (int i = 0; i < demand.Length; i++)
            {
                if (number == 0) break;
                if (demand[i] != null && demand[i].genre == genre)
                {
                    demand[i] = null;
                    emptyPos.Add(i);
                    number--;
                }
            }
        }
        public int GetDemand(Genre genre)
        {
            int counter = 0;
            foreach (Demand d in demand)
            {
                if (d != null && d.genre == genre) counter++;
            }

            return counter;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            int counter = 0;
            Rectangle source = new Rectangle(0,0, Book.Width, Book.Height);

            for (int i = 0; i < X_max; i++)
            {
                if (demand[counter] == null) { counter++; continue; }
                demand[counter].Draw(spriteBatch, eBookTexture, position + new Vector2((Book.Width + parsingX) * i, 0) * scale, scale);
                counter++;
            }
            for (int i = 1; i < Y_max; i++)
            {
                if (demand[counter] == null) { counter++; continue; }
                demand[counter].Draw(spriteBatch, eBookTexture, position + new Vector2((Book.Width + parsingX) * (X_max - 1), (Book.Height + parsingY) * i) * scale, scale);
                counter++;
            }
            for (int i = 1; i < X_max; i++)
            {
                if (demand[counter] == null) { counter++; continue; }
                demand[counter].Draw(spriteBatch, eBookTexture, position + new Vector2((Book.Width + parsingX) * (X_max - 1 - i), (Book.Height + parsingY) * (Y_max - 1)) * scale, scale);
                counter++;
            }
            for (int i = 1; i < Y_max - 1; i++)
            {
                if (demand[counter] == null) { counter++; continue; }
                demand[counter].Draw(spriteBatch, eBookTexture, position + new Vector2(0, (Book.Height + parsingY) * (Y_max - 1 - i)) * scale, scale);
                counter++;
            }

        }
    }
}
