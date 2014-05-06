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
        public bool phaseDone;
        GamePadState gPadState, prevgPadState;
        Archive archive;
        int playerID;

        public Player(Archive archive, int playerID)
        {
            this.archive = archive;
            this.playerID = playerID;

            this.archive.SetPosition(GetPosition(playerID));
        }

        public void Update(GameTime gameTime)
        {
            archive.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            archive.Draw(spriteBatch);
        }

        public void RecieveInput(GamePadState newgPadState)
        {
            prevgPadState = gPadState;
            gPadState = newgPadState;

            if (GameManager.phase == GameManager.TurnPhase.Browsing && gPadState.Buttons.Y == ButtonState.Pressed)
                phaseDone = true;

            archive.ReceiveInput(gPadState);
        }

        private Vector2 GetPosition(int ID)
        { 
            Vector2 playerPosition = Vector2.Zero;

            switch (ID)
            {
                case 0:
                    playerPosition = new Vector2(50, 50);
                    break;
                case 1:
                    playerPosition = new Vector2(800, 50);
                    break;
                case 2:
                    playerPosition = new Vector2(50, 450);
                    break;
                case 3:
                    playerPosition = new Vector2(800, 450);
                    break;
            }


            return playerPosition;
        }

        public void AddBook(Book book)
        {
            archive.AddBook(book);
        }

    }

}
