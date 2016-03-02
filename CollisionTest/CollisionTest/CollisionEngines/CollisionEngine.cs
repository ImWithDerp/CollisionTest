using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Base class for collision engines

namespace CollisionTest.CollisionEngines
{
    public abstract class CollisionEngine
    {
        private LinkedList<CollisionEntity> entities = new LinkedList<CollisionEntity>();
        
        // Add a new entity to track
        public void RegisterEntity(CollisionEntity entity)
        {
            entity.RegisterEntity(entities);
        }

        // Stop tracking an entity
        public void DeregisterEntity(CollisionEntity entity)
        {
            entity.DeregisterEntity();
        }

        // Reset to starting state
        public void Reset()
        {
            entities.Clear();
        }

        // Handle collisions for this frame
        public void ProcessCollisions()
        {
            foreach (CollisionEntity entity in entities)
                entity.Reset();
            
            TestCollisions(entities);

            foreach (CollisionEntity entity in entities)
                if (entity.collisions.Count > 0)
                    entity.React();
        }

        // Check which entities are colliding
        protected abstract void TestCollisions(LinkedList<CollisionEntity> entities);
    }
}
