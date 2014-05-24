using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Planspelet
{
    class StartMenu
    {
        private class Button
        {
            Texture2D defaultTexture;
            Texture2D textureActive;
            Vector2 position;
            public Vector2 GetPosition { get { return position; } }
            Vector2 textOffset;
            public string label;
            SpriteFont font;
            public bool active;

            public Button(Texture2D texture, Texture2D textureActive, Vector2 position, Vector2 textOffset, string label, SpriteFont font)
            {
                defaultTexture = texture;
                this.textureActive = textureActive;
                this.position = position;
                this.textOffset = textOffset;
                this.label = label;
                this.font = font;
            }

            public void Draw(SpriteBatch spriteBatch, float scale)
            {
                Texture2D texture;
                if (active) texture = textureActive;
                else texture = defaultTexture;
                Rectangle source = new Rectangle(0, 0, defaultTexture.Width, defaultTexture.Height);
                spriteBatch.Draw(texture, position, source, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, label, position + textOffset, Color.Black);
            }
        }

        public Input input { get; private set; }
        int selection = 0;

        int players = 1;
        public int GetNumPlayers { get { return players; } }

        Vector2 position;
        SpriteFont font;
        Button[] buttons;

        #region Texts
        string playerButtonTip =
            "Press A or B to change the number of players.";
        string Instructions =
            "WALL OF TEXT!!!";
        #endregion

        public StartMenu(Vector2 position, TextureManager textureManager, SpriteFont font)
        {
            this.position = position;

            input = new Input();
            this.font = font;
            buttons = new Button[]
            {
                new Button(textureManager.buttonTexture, textureManager.buttonActiveTexture, position + new Vector2(0, 0), new Vector2(20, 20), "Players: ", font),
                new Button(textureManager.buttonTexture, textureManager.buttonActiveTexture, position + new Vector2(300, 0), new Vector2(20, 20), "Instructions", font),
                new Button(textureManager.buttonTexture, textureManager.buttonActiveTexture, position + new Vector2(600, 0), new Vector2(20, 20), "Start Game", font),
            };

        }

        public bool Update(GameTime gameTime)
        {
            input.ProcessInput(GamePad.GetState(PlayerIndex.One));
            input.Update(gameTime);

            buttons[selection].active = false;
            if (input.Right) selection++;
            else if (input.Left) selection--;
            if (selection < 0) selection = 0;
            else if (selection > buttons.Length - 1) selection = buttons.Length - 1;
            buttons[selection].active = true;

            if (selection == 0)
            {
                if (input.ButtonA) { if (players < 4) players++; }
                else if (input.ButtonB) { if (players > 1) players--; }
            }
            else if (selection == 1)
            {
            }
            else if (selection == 2)
            {
                if (input.ButtonA) return true; // Start game
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            buttons[0].label = "Players: " + players.ToString();
            foreach (Button b in buttons)
            {
                b.Draw(spriteBatch, 3f);
            }

            if (selection == 0)
            {
                spriteBatch.DrawString(font, playerButtonTip, buttons[0].GetPosition + new Vector2(0, -60), Color.Black);
            }
            else if (selection == 1)
            {
                spriteBatch.DrawString(font, Instructions, position + new Vector2(200, -300), Color.Black);
            }
        }
    }
}
