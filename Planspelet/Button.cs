using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Planspelet
{
    class Button
    {
        Texture2D texture;
        Vector2 panelPosition;
        Vector2 offset;

        public Button(Texture2D texture, Vector2 panelPosition, Vector2 offset)
        {
            this.texture = texture;
            this.panelPosition = panelPosition;
            this.offset = offset;
        }
        public void SetPanelPosition(Vector2 position)
        {
            panelPosition = position;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, panelPosition + offset, color);
        }
    }
}
