using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planspelet
{
    class GameManager
    {
        Input input;
        Player[] players;

        public GameManager()
        {
            players = new Player[4];
            input = new Input(players.Length);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    players[i].RecieveInput(input.GetPlayerInput(i));
                    players[i].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            
        }

    }
}
