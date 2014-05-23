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
        }

        Demand[] demand;
        List<int> emptyPos;

        Vector2 position;
        Texture2D[] textures;
        float scale = 0.5f;

        int parsingX = 10;
        int parsingY = 6;

        int X_max;
        int Y_max;
        int totalNumber;


        public Market(TextureManager textureManager, Vector2 position, int X_max, int Y_max, Random rand)
        {
            this.position = position;
            textures = new Texture2D[Book.numberOfGenres];
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = textureManager.bookTexture[i];
            }

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
            if (emptyPos.Count == 0) return;
            int randIndex = rand.Next(0, emptyPos.Count - 1);
            emptyPos.Remove(randIndex);
            Genre genre = (Genre)(rand.Next(0, Book.numberOfGenres));
            demand[randIndex] = new Demand(genre, false, textures[(int)genre]);

            
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
                spriteBatch.Draw(demand[counter].texture, position + new Vector2((Book.Width + parsingX) * i, 0) * scale, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                counter++;
            }
            for (int i = 1; i < Y_max; i++)
            {
                if (demand[counter] == null) { counter++; continue; }
                spriteBatch.Draw(demand[counter].texture, position + new Vector2((Book.Width + parsingX) * (X_max - 1), (Book.Height + parsingY) * i) * scale, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                counter++;
            }
            for (int i = 1; i < X_max; i++)
            {
                if (demand[counter] == null) { counter++; continue; }
                spriteBatch.Draw(demand[counter].texture, position + new Vector2((Book.Width + parsingX) * (X_max - 1 - i), (Book.Height + parsingY) * (Y_max - 1)) * scale, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                counter++;
            }
            for (int i = 1; i < Y_max - 1; i++)
            {
                if (demand[counter] == null) { counter++; continue; }
                
                spriteBatch.Draw(demand[counter].texture, position + new Vector2(0, (Book.Height + parsingY) * (Y_max - 1 - i)) * scale, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                counter++;
            }

        }
    }
}
