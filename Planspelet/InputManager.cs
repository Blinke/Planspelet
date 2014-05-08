using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planspelet
{
    class InputManager
    {
        Input[] inputs;
        
        public InputManager(int numberOfPlayers)
        {
            inputs = new Input[numberOfPlayers];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = new Input();
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i].ProcessInput(GamePad.GetState((PlayerIndex)i));
                inputs[i].Update(gameTime);
            }
        }

        public Input GetPlayerInput(int index)
        {
            //return inputs[0];
            return inputs[index];
        }
    }
}
