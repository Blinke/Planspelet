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
        public Texture2D eBookTexture;
        public Texture2D pBookTexture;
        public Texture2D selectionTexture;
        public Texture2D[] middleSelection;
        public List<Texture2D> bookTexture, detailTexture;

        public void LoadTextures(ContentManager content, GraphicsDevice graphics)
        {
            eBookTexture = content.Load<Texture2D>("e-book");
            pBookTexture = content.Load<Texture2D>("physical");
            selectionTexture = content.Load<Texture2D>("selection");
            middleSelection = new Texture2D[4];

            for (int i = 0; i < middleSelection.Length; i++)
            {
                middleSelection[i] = content.Load<Texture2D>(@"selection_p" + (i + 1));
            }

            bookTexture = new List<Texture2D>();
            detailTexture = new List<Texture2D>();

            string[] bookNames, detailNames;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                bookNames = Directory.GetFiles(@"..\..\..\..\PlanspeletContent\Books\");
                detailNames = Directory.GetFiles(@"..\..\..\..\PlanspeletContent\Books\Details");

                LoadBookTextures(bookNames, ref bookTexture, graphics);
                LoadBookTextures(detailNames, ref detailTexture, graphics);
            }
        }

        private void LoadBookTextures(string[] textureFiles, ref List<Texture2D> textureList, GraphicsDevice graphics)
        {
            BlendState blendColor, blendAlpha;

            blendColor = new BlendState();
            blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;
            blendColor.AlphaDestinationBlend = Blend.Zero;
            blendColor.ColorDestinationBlend = Blend.Zero;
            blendColor.AlphaSourceBlend = Blend.SourceAlpha;
            blendColor.ColorSourceBlend = Blend.SourceAlpha;

            blendAlpha = new BlendState();
            blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;
            blendAlpha.AlphaDestinationBlend = Blend.Zero;
            blendAlpha.ColorDestinationBlend = Blend.Zero;
            blendAlpha.AlphaSourceBlend = Blend.One;
            blendAlpha.ColorSourceBlend = Blend.One;

            for (int i = 0; i < textureFiles.Length; i++)
            {
                FileStream fs = new FileStream(textureFiles[i].ToString(), FileMode.Open);
                textureList.Add(PremultiplyAlpha(Texture2D.FromStream(graphics, fs), graphics, blendColor, blendAlpha));
                fs.Close();
            }
        }

        private Texture2D PremultiplyAlpha(Texture2D file, GraphicsDevice graphics, BlendState blendColor, BlendState blendAlpha)
        {
            RenderTarget2D texture = new RenderTarget2D(graphics, file.Width, file.Height);

            graphics.SetRenderTarget(texture);
            graphics.Clear(Color.Black);

            SpriteBatch sb = new SpriteBatch(graphics);

            sb.Begin(SpriteSortMode.Immediate, blendColor);
            sb.Draw(file, file.Bounds, Color.White);
            sb.End();

            sb.Begin(SpriteSortMode.Immediate, blendAlpha);
            sb.Draw(file, file.Bounds, Color.White);
            sb.End();

            graphics.SetRenderTarget(null);
            return texture;
        }
    }
}
