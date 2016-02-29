using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionTest.Camera
{
    // Produces a view of entities in a stage, displaying them isometrically
    class Camera
    {
        Vector3 position; // Camera's current position in the stage

        double depthOffsetX; // For each unit along the Z axis, what is the distance along the X axis that an entity should be offset by when drawn?
        double depthOffsetY; // For each unit along the Z axis, what is the distance along the Y axis that an entity should be offset by when drawn?
    }
}
