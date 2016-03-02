using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionTest.CollisionEngines.Octree
{
    class OctreeEngine : CollisionEngine
    {
        public OctreeBox rootBox;

        public OctreeEngine(BoundingBox bounds, int depth)
        {
            rootBox = new OctreeBox(bounds, depth);

            rootBox.Fill();
        }

        protected override void TestCollisions(LinkedList<CollisionEntity> entities)
        {
            rootBox.Reset();

            foreach(CollisionEntity entity in entities)
            {
                // If an entity doesn't fit in the rootBox, assign it to the rootBox anyway because there's no where else we can put it
                if (!rootBox.Assign(entity))
                {
                    rootBox.ForceAssign(entity);
                }
            }

            rootBox.TestCollisions();
        }
    }
}
