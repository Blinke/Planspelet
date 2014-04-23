using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class Player
    {
        GamePadState gPadState, prevgPadState;
        
        public Player()
        {

        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public void RecieveInput(GamePadState newgPadState)
        {
            prevgPadState = gPadState;
            gPadState = newgPadState;
        }

    }

}
