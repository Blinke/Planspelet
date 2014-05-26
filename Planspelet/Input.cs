using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planspelet
{
    class Input
    {
        bool firstUpdate = true;
        float sensitivity;
        double selectionTimer, selectionDelay;

        GamePadState currentgPadState, previousgPadState;
        KeyboardState currentKeyboardState, previousKeyboardState;

        public bool Up { get; private set; }
        public bool Down { get; private set; }
        public bool Left { get; private set; }
        public bool Right { get; private set; }
        public bool ButtonA { get; private set; }
        public bool ButtonB { get; private set; }
        public bool ButtonX { get; private set; }
        public bool ButtonY { get; private set; }

        public Input()
        {
            sensitivity = 0.6f;
            selectionDelay = 150;
        }

        public void ProcessInput(GamePadState gPadState, int playerIndex)
        {
            currentKeyboardState = Keyboard.GetState();
            if (firstUpdate)
            {
                previousgPadState = gPadState;
                previousKeyboardState = currentKeyboardState;
                firstUpdate = false;
            }

            Up = false;
            Down = false;
            Left = false;
            Right = false;
            ButtonA = false;
            ButtonB = false;
            ButtonX = false;
            ButtonY = false;


            if (selectionTimer <= 0)
            {
                selectionTimer = selectionDelay;

                if (gPadState.ThumbSticks.Left.Y > sensitivity)
                    Up = true;
                else if (gPadState.ThumbSticks.Left.Y < -sensitivity)
                    Down = true;
                if (gPadState.ThumbSticks.Left.X > sensitivity)
                    Right = true;
                else if (gPadState.ThumbSticks.Left.X < -sensitivity)
                    Left = true;
            }

            if (playerIndex == 1)
            {
                if (currentKeyboardState.IsKeyDown(Keys.W) && previousKeyboardState.IsKeyUp(Keys.W))
                    Up = true;
                else if (currentKeyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
                    Down = true;
                if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
                    Left = true;
                else if (currentKeyboardState.IsKeyDown(Keys.D) && previousKeyboardState.IsKeyUp(Keys.D))
                    Right = true;

                if (currentKeyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyUp(Keys.R))
                    ButtonA = true;
                if (currentKeyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E))
                    ButtonB = true;
                if (currentKeyboardState.IsKeyDown(Keys.F) && previousKeyboardState.IsKeyUp(Keys.F))
                    ButtonY = true; 
            }

            if (gPadState.Buttons.A == ButtonState.Pressed && previousgPadState.Buttons.A == ButtonState.Released)
                ButtonA = true;
            if (gPadState.Buttons.B == ButtonState.Pressed && previousgPadState.Buttons.B == ButtonState.Released)
                ButtonB = true;
            if (gPadState.Buttons.X == ButtonState.Pressed && previousgPadState.Buttons.X == ButtonState.Released)
                ButtonX = true;
            if (gPadState.Buttons.Y == ButtonState.Pressed && previousgPadState.Buttons.Y == ButtonState.Released)
                ButtonY = true;

            previousgPadState = gPadState;
            previousKeyboardState = currentKeyboardState;
        }

        public void Update(GameTime gameTime)
        {
            selectionTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
