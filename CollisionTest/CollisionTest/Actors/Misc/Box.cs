using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CollisionTest.Cameras;
using CollisionTest.CollisionEngines;

namespace CollisionTest.Actors.Misc
{
    public class Box : Actor
    {
        public Vector3 velocity;

        Vector3 min, max;
        
        Texture2D dummyTexture;
        Color Colori;

        BoxCollider collider;

        bool collided;

        public Box(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;

            collider = new BoxCollider(new BoundingBox(min, max), this);

            Reset();
        }

        public override void Reset()
        {
            this.position = Vector3.Zero;
            this.velocity = Vector3.Zero;
            collided = false;
        }

        public override void RegisterActor(LinkedList<Actor> list)
        {
            Game1.currentCollisionEngine.RegisterEntity(collider);

            base.RegisterActor(list);
        }

        public override void DeregisterActor()
        {
            Game1.currentCollisionEngine.DeregisterEntity(collider);

            base.DeregisterActor();
        }

        public override void Load(GraphicsDevice graphicsDevice)
        {
            dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            Colori = Color.White;
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

        public override void Update(GameTime gameTime)
        {
            if (collided)
            {
                // Average position of colliding entities relative to self
                Vector3 collisionOrigin = Vector3.Zero;

                foreach (CollisionEntity entity in collider.collisions)
                {
                    Vector3 entityPos = entity.GetPosition();
                    collisionOrigin += entityPos;
                }

                collisionOrigin /= collider.collisions.Count();
                collisionOrigin -= collider.GetPosition();

                // Head the other way
                if (collisionOrigin.Length() > 0)
                {
                    collisionOrigin.Normalize();

                    velocity = collisionOrigin * velocity.Length() * -1;
                }
                else
                    velocity *= -1;
            }           

            position.X += velocity.X;
            position.Y += velocity.Y;
            position.Z += velocity.Z;

            collider.UpdatePosition(position);

            collided = false;
        }

        public class BoxCollider : CollisionBox
        {
            Box parentBox;

            public BoxCollider(BoundingBox boundingBox, Box parentBox)
            {
                this.boundingBox = boundingBox;
                this.parentBox = parentBox;
            }

            public override void React()
            {
                parentBox.collided = true;
            }
        }
    }
}
