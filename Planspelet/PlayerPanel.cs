//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Content;

//namespace Planspelet
//{
//    class PlayerPanel
//    {
//        GamePadState gPadState, prevgPadState;
//        Vector2 position;

//        Archive archive;
//        PublishMenu publishMenu;

//        Tab activeTab;

//        public PlayerPanel(TextureManager textureManager, Vector2 position)
//        {
//            this.position = position;
//            archive = new Archive(position, 0.5f, 2, 5);
//            publishMenu = new PublishMenu(textureManager, position, 0.5f);
//            activeTab = archive;
//        }

//        public void ReceiveInput(GamePadState newgPadState)
//        {
//            prevgPadState = gPadState;
//            gPadState = newgPadState;

//            archive.ReceiveInput(gPadState);
//        }

//        public void AddBook(Book book)
//        {
//            archive.AddBook(book);
//        }
//        public void CopyArchive(Archive archive)
//        {
//            this.archive.CopyBooks(archive);
//        }

//        public void Update(bool up, bool down, bool left, bool right)
//        {
//            activeTab.ReceiveInput(up, down, left, right);
//        }

//        public void OpenPublishMenu()
//        {
//            activeTab = publishMenu;
//            publishMenu.SetActiveBook(archive.GetSelectedBook());
//        }
//        public void OpenArchive()
//        {
//            activeTab = archive;
//        }

//        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
//        {
//            activeTab.Draw(spriteBatch, font);
//        }
//    }
//}
