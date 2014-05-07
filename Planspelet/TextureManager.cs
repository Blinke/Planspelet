using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class TextureManager
    {
        public Texture2D eBookTexture;
        public Texture2D pBookTexture;

        public void LoadTextures(ContentManager content)
        {
            eBookTexture = content.Load<Texture2D>("e-book");
            pBookTexture = content.Load<Texture2D>("physical");
        }
    }
}
