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
        float sensitivity;
        double selectionTimer, selectionDelay;

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
            selectionDelay = 200;
        }

        public void GetInput(GamePadState gPadState)
        {
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            ButtonA = false;
            ButtonB = false;
            ButtonX = false;
            ButtonY = false;

            if (selectionTimer <= 0 && gPadState.ThumbSticks.Left.Length() > sensitivity)
            {
                selectionTimer = selectionDelay;

                if (gPadState.ThumbSticks.Left.X > sensitivity)
                    Left = true;
                if (gPadState.ThumbSticks.Left.X < -sensitivity)
                    Right = true;
                if (gPadState.ThumbSticks.Left.Y > sensitivity)
                    Up = true;
                if (gPadState.ThumbSticks.Left.Y < -sensitivity)
                    Down = true;
            }

            if (gPadState.Buttons.A == ButtonState.Pressed)
                ButtonA = true;
            if (gPadState.Buttons.B == ButtonState.Pressed)
                ButtonB = true;
            if (gPadState.Buttons.X == ButtonState.Pressed)
                ButtonX = true;
            if (gPadState.Buttons.Y == ButtonState.Pressed)
                ButtonY = true;
        }

        public void Update(GameTime gameTime)
        {
            selectionTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
