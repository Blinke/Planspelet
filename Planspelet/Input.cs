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
        GamePadState[] gPadState;

        public Input(int numberOfPlayers)
        {
            gPadState = new GamePadState[numberOfPlayers];
        }

        public void Update()
        {
            for (int i = 0; i < gPadState.Length; i++)
                gPadState[i] = GamePad.GetState((PlayerIndex)i);
        }

        public GamePadState GetPlayerInput(int index)
        {
            return gPadState[0];
            //return gPadState[index];
        }
    }
}
