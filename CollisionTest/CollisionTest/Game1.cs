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
using CollisionTest.Stages;
using CollisionTest.CollisionEngines;

namespace CollisionTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Stage currentStage;
        public static CollisionEngine currentCollisionEngine;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            currentStage = new Stage();

            currentStage.camera.position.Y = -0;

            currentCollisionEngine = new CollisionEngines.Octree.OctreeEngine(new BoundingBox(new Vector3(-1024, -1024, -1024), new Vector3(1024, 1024, 1024)), 4);

            Actors.Misc.Box box1 = new Actors.Misc.Box(new Vector3(-100, -100, -100), new Vector3(100, 100, 100));
            box1.position = new Vector3(-200, 0, 0);
            box1.velocity = new Vector3(1, (float)0.4, 0);

            Actors.Misc.Box box2 = new Actors.Misc.Box(new Vector3(-50, -50, -100), new Vector3(50, 50, 50));
            box2.position = new Vector3(200, 0, 0);
            box2.velocity = new Vector3(-1, (float)-0.3, 0);

            Actors.Misc.Box wall1 = new Actors.Misc.Box(new Vector3(-1, -200, -300), new Vector3(1, 200, 300));
            wall1.position = new Vector3(-300, 0, 0);

            Actors.Misc.Box wall2 = new Actors.Misc.Box(new Vector3(-1, -200, -300), new Vector3(1, 200, 300));
            wall2.position = new Vector3(300, 0, 0);

            Actors.Misc.Box wall3 = new Actors.Misc.Box(new Vector3(-300, -1, -300), new Vector3(300, 1, 300));
            wall3.position = new Vector3(0, -200, 0);

            Actors.Misc.Box wall4 = new Actors.Misc.Box(new Vector3(-300, -1, -300), new Vector3(300, 1, 300));
            wall4.position = new Vector3(0, 200, 0);

            currentStage.RegisterActor(box1);
            currentStage.RegisterActor(box2);

            currentStage.RegisterActor(wall1);
            currentStage.RegisterActor(wall2);
            currentStage.RegisterActor(wall3);
            currentStage.RegisterActor(wall4);
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        // This is a texture we can render.

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            currentStage.Load(GraphicsDevice);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState(PlayerIndex.One).GetPressedKeys().Contains(Keys.Escape))
                this.Exit();

            currentStage.Update(gameTime);

            currentCollisionEngine.ProcessCollisions();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Transform view to center around camera position
            Matrix transformMatrix = Matrix.CreateTranslation(new Vector3((float)(graphics.GraphicsDevice.Viewport.Width*0.5), (float)(graphics.GraphicsDevice.Viewport.Height*0.5), 0));

            // Draw the sprite.
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, transformMatrix);

            currentStage.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
