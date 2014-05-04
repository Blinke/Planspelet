using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    abstract class Tab
    {
        protected Vector2 position;
        protected float scale = 0.5f;

        public Tab(Vector2 position, float scale)
        {
            this.position = position;
            this.scale = scale;
        }
        public Tab(Tab tab)
        {
            position = tab.position;
            scale = tab.scale;
        }
        public abstract Tab Clone();
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
