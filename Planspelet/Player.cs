using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Planspelet
{
    class Player
    {
        GamePadState gPadState, prevgPadState;

        public Player()
        { 
            
        }

        public void RecieveInput(GamePadState newgPadState)
        {
            prevgPadState = gPadState;
            gPadState = newgPadState;
        }

        internal void Update(GameTime gameTime)
        {
            
        }
    }

}
