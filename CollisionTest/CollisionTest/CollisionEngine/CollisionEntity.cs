using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// Base class for entities that can collide with each other

namespace CollisionTest.CollisionEngine
{
    abstract class CollisionEntity
    {
        // The entities collided with this frame
        public List<CollisionEntity> collisions = new List<CollisionEntity>();

        // Reset for a new frame
        public void Reset()
        {
            collisions.Clear();
        }

        // Test collision with another entity
        public abstract bool Test(CollisionEntity entity);

        // React to collisions
        public abstract void React();
    }

    // Entities using bounding boxes
    abstract class CollisionBox : CollisionEntity
    {
        public BoundingBox boundingBox;

        public override bool Test(CollisionEntity entity)
        {
            bool result = false;

            if (entity is CollisionBox)
                result = boundingBox.Intersects((entity as CollisionBox).boundingBox);
            else if (entity is CollisionSphere)
                result = boundingBox.Intersects((entity as CollisionSphere).boundingSphere);

            return result;
        }
    }

    // Entities using bounding spheres
    abstract class CollisionSphere : CollisionEntity
    {
        public BoundingSphere boundingSphere;

        public override bool Test(CollisionEntity entity)
        {
            bool result = false;

            if (entity is CollisionBox)
                result = boundingSphere.Intersects((entity as CollisionBox).boundingBox);
            else if (entity is CollisionSphere)
                result = boundingSphere.Intersects((entity as CollisionSphere).boundingSphere);

            return result;
        }
    }
}
