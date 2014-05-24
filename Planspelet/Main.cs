using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Planspelet
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        StartMenu startMenu;
        GameManager gameManager;
        TextureManager textureManager;
        SpriteBatch spriteBatch;

        SpriteFont fontSmall;
        SpriteFont fontLarge;

        public enum GameState
        {
            StartScreen,
            Play
        }

        public static GameState gameState;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            fontSmall = Content.Load<SpriteFont>("fontSmall");
            fontLarge = Content.Load<SpriteFont>("fontLarge");

            textureManager = new TextureManager();
            textureManager.LoadTextures(Content, graphics.GraphicsDevice);
            startMenu = new StartMenu(textureManager, fontLarge);
            //gameManager = new GameManager(Window, textureManager, 1);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameState = GameState.StartScreen;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (gameState)
            {
                case GameState.StartScreen:
                    if (startMenu.Update(gameTime))
                    {
                        gameManager = new GameManager(Window, textureManager, startMenu.GetNumPlayers);
                        gameState = GameState.Play;
                    }
                    break;
                case GameState.Play:
                    gameManager.Update(gameTime);
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.StartScreen:
                    startMenu.Draw(spriteBatch);
                    break;
                case GameState.Play:
                    gameManager.Draw(spriteBatch, fontSmall, fontLarge);   
                    break;
                default:
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
