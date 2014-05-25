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
        private class Instruction
        {
            string text;
            Vector2 textPosition;
            int textLineLength = 60;

            Texture2D[] textures;
            Vector2[] texturePositions;

            public Instruction(string text, Vector2 textPosition, Texture2D[] textures, Vector2[] texturePositions)
            {
                this.text = text;
                this.textPosition = textPosition;
                this.textures = textures;
                this.texturePositions = texturePositions;
            }
            public void AdjustText()
            {
                List<char> textCharacters = text.ToList();
                string adjustedText = "";
                string line = "";
                while (textCharacters.Count != 0)
                {
                    if (textCharacters[0] == " "[0])
                        textCharacters.RemoveAt(0);

                    string word = "";
                    while (textCharacters.Count != 0 && textCharacters[0] != " "[0])
                    {
                        word += textCharacters[0];
                        textCharacters.RemoveAt(0);
                    }
                    //while (textCharacters.Count != 0)
                    //{
                    //    if (textCharacters[0] == "\n"[0])
                    //    {
                    //        line += word;
                    //        text += line;
                    //    }
                    //    if (textCharacters[0] == " "[0])
                    //    word += textCharacters[0];
                    //    textCharacters.RemoveAt(0);
                    //}
                    if (word == "to")
                        word = "to";

                    if ((line + word).Length < textLineLength) line += word + " ";
                    else
                    {
                        line += "\n";
                        adjustedText += line;
                        line = "";
                    }

                    /*
                    for (int i = 0; i < textLineLength; i++)
                    {
                        if (textCharacters.Count == 0) break;
                        if (textCharacters[0] == "\n"[0])
                        {
                            text += textCharacters[0];
                            textCharacters.RemoveAt(0);
                            break;
                        }
                        else
                        {
                            text += textCharacters[0];
                            textCharacters.RemoveAt(0);
                            if (i == textLineLength - 1) text += "\n";
                        }
                    }
                     * */

                }
            }


            public void Draw(SpriteBatch spriteBatch, SpriteFont font)
            {
                

                spriteBatch.DrawString(font, text, textPosition, Color.Black);
                for (int i = 0; i < texturePositions.Length; i++)
                {
                    spriteBatch.Draw(textures[i], texturePositions[i], Color.White);
                }
            }
        }

        public Input input { get; private set; }
        int selection = 0;

        int players = 1;
        public int GetNumPlayers { get { return players; } }

        Vector2 position;
        SpriteFont font;
        Button[] buttons;
        Instruction[] instructions;
        int currentInstructions = 0;

        #region ButtonTips
        string playerButtonTip =
            "Press A or B to change the number of players.";
        string instructionButtonTip =
            "Press A to cycle through instructions.";
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

            #region Instructions
            string[] instructionTexts = new string[]
            {
                "'Shall we publish it?' is a game about managing a publishing company. Up to four " +
                "companies will compete against eachother over new book offers and sales.\n\n" +
                "As a player you need to judge the demand of the market and adjust your choices accord" +
                "ingly. Will you take up a new book? Will that genre be profitable enough, consider" + 
                "ing the competition? What books do you reprint?",
            };
            Vector2 textPosition = position + new Vector2(0, -400);
            instructions = new Instruction[]
            {
                new Instruction(instructionTexts[0], textPosition,
                    textureManager.bookTexture.ToArray(),
                    new Vector2[]{}),
            };
            #endregion

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
                spriteBatch.DrawString(font, playerButtonTip, buttons[0].GetPosition + new Vector2(0, -40), Color.Black);
            }
            else if (selection == 1)
            {
                spriteBatch.DrawString(font, instructionButtonTip, buttons[0].GetPosition + new Vector2(0, -40), Color.Black);
                instructions[currentInstructions].Draw(spriteBatch, font);
            }
        }
    }
}
