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
            int textLineLength;

            Texture2D[] textures;
            Vector2[] texturePositions;

            public Instruction(string text, int textLineLength, Vector2 textPosition, Texture2D[] textures, Vector2[] texturePositions)
            {
                this.text = text;
                this.textLineLength = textLineLength;
                AdjustText();
                this.textPosition = textPosition;
                this.textures = textures;
                this.texturePositions = texturePositions;
            }
            public Instruction(string text, int textLineLength, Vector2 textPosition) : this(text, textLineLength, textPosition, null, null) { }
            public void AdjustText()
            {
                List<char> textCharacters = text.ToList();
                string adjustedText = "";
                string line = "";
                while (textCharacters.Count != 0)
                {
                    // Fill one line of text.

                    if (textCharacters[0] == " "[0])
                        textCharacters.RemoveAt(0);

                    string word = "";
                    while (textCharacters.Count != 0 && textCharacters[0] != " "[0] && textCharacters[0] != "\n"[0])
                    {
                        // Fill one word of text.
                        word += textCharacters[0];
                        textCharacters.RemoveAt(0);
                    }
                    if (textCharacters.Count == 0)
                    {
                        adjustedText += line + word;
                        break;
                    }
                    if (textCharacters[0] == "\n"[0])
                    {
                        line += word + textCharacters[0];
                        textCharacters.RemoveAt(0);
                        adjustedText += line;
                        line = "";
                    }
                    else if (textCharacters[0] == " "[0])
                    {
                        if ((line + word).Length < textLineLength)
                        {
                            line += word + textCharacters[0];
                            textCharacters.RemoveAt(0);
                        }
                        else
                        {
                            line += word + "\n";
                            adjustedText += line;
                            line = "";
                        }
                    }
                }
                text = adjustedText;
            }

            public void Draw(SpriteBatch spriteBatch, SpriteFont font)
            {
                spriteBatch.DrawString(font, text, textPosition, Color.Black);
                if (textures != null)
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
            "Press A and B to change the number of players.";
        string instructionButtonTip =
            "Press A and B cycle through instructions.";
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
                "Shall we publish it?" +
                "\n\nThis is a game about managing a publishing company. Up to four " +
                "companies will compete against eachother over new book offers and sales.\n\n" +
                "As a player you need to judge the demand of the market and adjust your choices accordingly. " +
                "What books do you reprint? Will you take up a new book? Will that genre be profitable enough, considering the competition?" +
                "\n\nThe right calls will keep your budget healty.",

                "Genres:" +
                "\n\nBooks come in "+ Book.numberOfGenres.ToString() + " different genres, and every genre has a specific color." + 
                "\n\nEvery turn players can take up a new book from an arrange of book offers, a red bar will indicate the cost to accept the book." + 
                "\n\n Books of the same genre sell equally well, despite any differences in cost. Pick up books of a genre you do not have, or additional " +
                "books of a genre that is in high demand.",

                "E-books:" + 
                "\n\nIf you have accepted a book you will be asked whether to publish it as an E-book or not." +
                "Regular books have greater profitability, they sell more often and for a better price. However, they need to be printed (for a print cost)" +
                "and printed books have a storage cost as well." + 
                "\n\nYou need to judge when the demand for E-books is high enough to warrant publishing one over another regular book.",

                "Printing & selling:" + 
                "\n\nWhen all players have finished accepting book offers, they are now free to print books." +
                "\n\nAt the lower left corner of every book is number representing the number of books in stock." + 
                "Selecting a book and clicking 'A' will increase it's stock for a price. When you are satisfied, press 'Y' to continue" +
                "\n\nWhen all players have finished printing books, the books will sell to the market. The market demand is represented as a border of books lining" +
                "the game window. Books of a genre with high demand and low competition will sell in higher numbers." +
                "On each book is a bar representing the total profit and total cost of that particular book.",
            };
            Vector2 textPosition = position + new Vector2(0, -450);
            instructions = new Instruction[]
            {
                new Instruction(instructionTexts[0], 60, textPosition),

                new Instruction(instructionTexts[1], 45, textPosition,
                    textureManager.bookTexture.ToArray(),
                    new Vector2[]{
                        new Vector2(800, 100), new Vector2(800 + Book.Width + 8, 100), new Vector2(800 + Book.Width * 2 + 16, 100),
                        new Vector2(800 + Book.Width*0.5f, 108 + Book.Height), new Vector2(800 + Book.Width*1.5f + 8, 108 + Book.Height)}),

                new Instruction(instructionTexts[2], 50, textPosition,
                    new Texture2D[]{
                        textureManager.bookTexture[0],
                        textureManager.eBookTexture},
                    new Vector2[]{
                        new Vector2(800, 100), new Vector2(800, 100)}),

                new Instruction(instructionTexts[3], 70, textPosition),
            };
            #endregion

        }

        public bool Update(GameTime gameTime)
        {
            input.ProcessInput(GamePad.GetState(PlayerIndex.One), 1);
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
                if (input.ButtonA) { if (currentInstructions < instructions.Length - 1) currentInstructions++; }
                else if (input.ButtonB) { if (currentInstructions > 0) currentInstructions--; }
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
