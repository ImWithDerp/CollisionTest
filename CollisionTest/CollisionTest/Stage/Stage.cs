using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionTest.Stage
{
    // A stage that contains entities
    class Stage
    {
        // List of actors in this stage
        public LinkedList<Actor.Actor> actors = new LinkedList<Actor.Actor>();

        // Draw to a given camera
        public void Draw(Camera.Camera camera)
        {
            foreach (Actor.Actor actor in actors)
                actor.Draw(camera);
        }
    }
}
