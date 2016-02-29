using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionTest.Actor
{
    // An entity in the game world
    // Each actor can belong to one stage at a time
    class Actor
    {
        // Position in the stage's actor linked list, saved for fast removal
        LinkedListNode<Actor> linkedListNode;

        // Draw to a given camera
        public void Draw(Camera.Camera camera)
        {

        }
    }
}
