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
        protected struct Selection
        {
            public int x;
            public int y;
            public bool active;

            public Selection(int x = 0, int y = 0, bool active = false)
                :this()
            {
                this.x = x;
                this.y = y;
                this.active = active;
            }
        }

        protected Vector2 position;
        protected float scale = 0.5f;

        protected Selection[] selection;

        //protected bool selection = true;
        //protected int selectionX = 0;
        //protected int selectionY = 0;

        public Tab(Vector2 position, float scale)
        {
            this.position = position;
            this.scale = scale;
            selection = new Selection[4];
        }
        public Tab(Tab tab)
        {
            position = tab.position;
            scale = tab.scale;
            selection = new Selection[4];
        }
        public abstract Tab Clone();
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public abstract void ReceiveInput(Input input, int playerIndex);
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont font);
    }
}
