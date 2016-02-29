using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionTest.CollisionEngine.OctreeEngine
{
    // Octree division
    class OctreeBox
    {
        int divisions; // Number of divisions to generate below this one
        BoundingBox bounds;
        List<CollisionEntity> entities = new List<CollisionEntity>(); // Entities registered with this box this frame
        int childEntities = 0; // Number of entities registered with this box or with any of the boxes below this one

        // Boxes below this one
        OctreeBox[] subdivs = new OctreeBox[8];

        public OctreeBox(BoundingBox bounds, int depth)
        {
            this.bounds = bounds;
            this.divisions = depth;
        }

        public void Fill()
        {
            if (divisions > 0)
            {
                // split each axis into min/mid/max
                float[] xDiv = { bounds.Min.X, (bounds.Min.X + bounds.Max.X) * (float)0.5, bounds.Max.X };
                float[] yDiv = { bounds.Min.Y, (bounds.Min.Y + bounds.Max.Y) * (float)0.5, bounds.Max.Y };
                float[] zDiv = { bounds.Min.Z, (bounds.Min.Z + bounds.Max.Z) * (float)0.5, bounds.Max.Z };

                int index = 0;

                for (int x = 0; x < 2; x++)
                    for (int y = 0; y < 2; y++)
                        for (int z = 0; z < 2; z++)
                        {
                            subdivs[index] = new OctreeBox(new BoundingBox(new Vector3(xDiv[x], yDiv[y], zDiv[z]), new Vector3(xDiv[x+1], yDiv[y+1], zDiv[z+1])), divisions - 1);
                            index++;
                        }
            }
        }

        // Reset for a new frame
        public void Reset()
        {
            childEntities = 0;
            entities.Clear();

            if (divisions > 0)
            {
                foreach (OctreeBox subdiv in subdivs)
                    subdiv.Reset();
            }
        }

        public bool Contains(CollisionEntity entity)
        {
            bool result = false;

            if (entity is CollisionBox)
                result = (bounds.Contains((entity as CollisionBox).boundingBox) == ContainmentType.Contains);
            else if (entity is CollisionSphere)
                result = bounds.Intersects((entity as CollisionSphere).boundingSphere);

            return result;
        }

        // Try and assign an entity to a subdiv, otherwise assign it to this box
        public bool Assign(CollisionEntity entity)
        {
            bool result = false;

            // If it fits here, check if it can also fit in one of our subdivs, otherwise just leave it here
            if (Contains(entity))
            {
                // Check subdivs first (if any)
                if (divisions > 0)
                {
                    foreach (OctreeBox subdiv in subdivs)
                        if (subdiv.Assign(entity))
                        {
                            // Found one the entity fits in, so we can let that subdiv handle the entity
                            result = true;
                            break;
                        }
                }

                // Could not assign to any subdivs, so leave it here
                if (result == false)
                {
                    entities.Add(entity);
                    result = true;
                }

                // Increment child entity count
                childEntities++;
            }

            return result;
        }

        // Assign to this box regardless of where this entity fits
        public void ForceAssign(CollisionEntity entity)
        {
            entities.Add(entity);
        }

        // Test any entities in this box or its children for collisions with others also in this box or in its children
        public void TestCollisions()
        {
            if (childEntities > 0)
            {
                CollisionEntity[] entityArray = entities.ToArray();

                for (int i = 0; i < entityArray.Length; i++)
                {
                    for (int j = i+1; j < entityArray.Length; j++)
                        if (entityArray[i].Test(entityArray[j]))
                        {
                            entityArray[i].collisions.Add(entityArray[j]);
                            entityArray[j].collisions.Add(entityArray[i]);
                        }

                    if (divisions > 0)
                        foreach (OctreeBox subdiv in subdivs)
                            subdiv.TestCollisions(entityArray[i]);
                }

                if (divisions > 0)
                    foreach (OctreeBox subdiv in subdivs)
                        subdiv.TestCollisions();
            }
        }

        // Test a specific entity for collisions with others in this box or in its children
        private void TestCollisions(CollisionEntity entity)
        {
            if (childEntities > 0)
            {
                foreach (CollisionEntity childEntity in entities)
                    if ( entity.Test(childEntity))
                    {
                        entity.collisions.Add(childEntity);
                        childEntity.collisions.Add(entity);
                    }

                if (divisions > 0)
                    foreach (OctreeBox subdiv in subdivs)
                        subdiv.TestCollisions(entity);
            }
        }

        OctreeBox SubdivMinMinMin
        {
            get { return subdivs[0]; }
            set { subdivs[0] = value; }
        }
        OctreeBox SubdivMinMinMax
        {
            get { return subdivs[1]; }
            set { subdivs[1] = value; }
        }
        OctreeBox SubdivMinMaxMin
        {
            get { return subdivs[2]; }
            set { subdivs[2] = value; }
        }
        OctreeBox SubdivMinMaxMax
        {
            get { return subdivs[3]; }
            set { subdivs[3] = value; }
        }
        OctreeBox SubdivMaxMinMin
        {
            get { return subdivs[4]; }
            set { subdivs[4] = value; }
        }
        OctreeBox SubdivMaxMinMax
        {
            get { return subdivs[5]; }
            set { subdivs[5] = value; }
        }
        OctreeBox SubdivMaxMaxMin
        {
            get { return subdivs[6]; }
            set { subdivs[6] = value; }
        }
        OctreeBox SubdivMaxMaxMax
        {
            get { return subdivs[7]; }
            set { subdivs[7] = value; }
        }
    }
}
