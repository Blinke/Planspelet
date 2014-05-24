using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Planspelet
{
    class TextureManager
    {
        public Texture2D playerBackground;
        public Texture2D eButtonTexture;
        public Texture2D pButtonTexture;
        public Texture2D selectionTexture;
        public Texture2D backgroundTexture;
        public Texture2D[] middleSelection;
        public List<Texture2D> bookTexture, detailTexture;
        public Texture2D eBookTexture;
        public Texture2D lossTexture, profitTexture, outlineTexture;
        public Texture2D doneTexture;

        public void LoadTextures(ContentManager content, GraphicsDevice graphics)
        {
            playerBackground = content.Load<Texture2D>("background");
            eButtonTexture = content.Load<Texture2D>("eButton");
            pButtonTexture = content.Load<Texture2D>("pButton");
            selectionTexture = content.Load<Texture2D>("selection");
            backgroundTexture = content.Load<Texture2D>(@"paper_background");
            doneTexture = content.Load<Texture2D>("ready_mark");
            middleSelection = new Texture2D[4];

            for (int i = 0; i < middleSelection.Length; i++)
            {
                middleSelection[i] = content.Load<Texture2D>(@"selection_p" + (i + 1));
            }

            bookTexture = new List<Texture2D>();
            detailTexture = new List<Texture2D>();

            string[] bookNames, detailNames;

            bookNames = Directory.GetFiles(@"..\..\..\..\PlanspeletContent\Books\");
            detailNames = Directory.GetFiles(@"..\..\..\..\PlanspeletContent\Books\Details");

            LoadBookTextures(bookNames, ref bookTexture, content, "Books\\");
            LoadBookTextures(detailNames, ref detailTexture, content, "Books\\Details\\");
            eBookTexture = content.Load<Texture2D>("eBook");

            lossTexture = content.Load<Texture2D>("loss");
            profitTexture = content.Load<Texture2D>("profit");
            outlineTexture = content.Load<Texture2D>("outline");
        }

        private void LoadBookTextures(string[] textureFiles, ref List<Texture2D> textureList, ContentManager content, string path)
        {
            int breakPoint = textureFiles[0].LastIndexOf(@"\") + 1;
            for (int i = 0; i < textureFiles.Length; i++)
            {
                string tempString = textureFiles[i].Substring(breakPoint, textureFiles[i].Length - breakPoint - 4);
                Texture2D tempTexture = content.Load<Texture2D>(path + tempString);
                textureList.Add(tempTexture);
            }
        }
    }
}
