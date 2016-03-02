using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// Base class for entities that can collide with each other

namespace CollisionTest.CollisionEngines
{
    public abstract class CollisionEntity
    {
        // Current position in the game world
        protected Vector3 position;

        // Position in collision engine's list of entities, saved for quick removal
        private LinkedListNode<CollisionEntity> linkedListNode = null;

        // The entities collided with this frame
        public List<CollisionEntity> collisions = new List<CollisionEntity>();

        // Update position and location of bounding objects
        public abstract void UpdatePosition(Vector3 newPosition);

        public Vector3 GetPosition()
        {
            return new Vector3(position.X, position.Y, position.Z);
        }

        // Reset for a new frame
        public void Reset()
        {
            collisions.Clear();
        }

        // Register this entity in a collision list
        public void RegisterEntity(LinkedList<CollisionEntity> list)
        {
            linkedListNode = list.AddLast(this);
        }

        // Remove this entity from its collision list
        public void DeregisterEntity()
        {
            if (linkedListNode != null)
            {
                LinkedList<CollisionEntity> list = linkedListNode.List;
                list.Remove(linkedListNode);
                linkedListNode = null;
            }
        }

        // Test collision with another entity
        public abstract bool Test(CollisionEntity entity);

        // React to collisions
        public abstract void React();
    }

    // Entities using bounding boxes
    public abstract class CollisionBox : CollisionEntity
    {
        public BoundingBox boundingBox;

        public override void UpdatePosition(Vector3 newPosition)
        {
            boundingBox.Min -= position;
            boundingBox.Max -= position;

            position = newPosition;

            boundingBox.Min += position;
            boundingBox.Max += position;
        }

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
    public abstract class CollisionSphere : CollisionEntity
    {
        public BoundingSphere boundingSphere;

        public override void UpdatePosition(Vector3 newPosition)
        {
            boundingSphere.Center -= position;

            position = newPosition;

            boundingSphere.Center += position;
        }

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
