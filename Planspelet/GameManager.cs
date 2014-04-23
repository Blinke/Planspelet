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
            //Should get the number of players from the start screen or something, can send that as an argument for the GameManager
            players = new Player[4];
            input = new Input(players.Length);
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
            }
        }

        public void Update(GameTime gameTime)
        {
            input.Update();

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
            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
        }

    }
}
