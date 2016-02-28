using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Base class for collision engines

namespace CollisionTest.CollisionEngine
{
    abstract class CollisionEngine
    {
        // Handle collisions for this frame
        public void ProcessCollisions(List<CollisionEntity> entities)
        {
            foreach (CollisionEntity entity in entities)
                entity.Reset();
            
            TestCollisions(entities);

            foreach (CollisionEntity entity in entities)
                entity.React();
        }

        // Check which entities are colliding
        public abstract void TestCollisions(List<CollisionEntity> entities);
    }
}
