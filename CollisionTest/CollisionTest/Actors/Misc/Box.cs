using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CollisionTest.Cameras;

namespace CollisionTest.Actors.Misc
{
    class Box : Actor
    {
        Vector3 min, max;
        
        Texture2D dummyTexture;
        Color Colori;

        public Box(Vector3 min, Vector3 max, Game game)
        {
            this.min = min;
            this.max = max;

        }

        public override void Load(GraphicsDevice graphicsDevice)
        {
            dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Camera camera = Game1.currentStage.camera;

            Rectangle dummyRectangle = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));

            Vector2 offset = GetDrawOffset();

            dummyRectangle.X += (int)offset.X;
            dummyRectangle.Y += (int)offset.Y;
            
            spriteBatch.Draw(dummyTexture, dummyRectangle, Colori);
        }
    }
}
