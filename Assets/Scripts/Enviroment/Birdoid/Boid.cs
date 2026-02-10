using UnityEngine;

namespace Enviroment.Birdoid
{
    public sealed class Boid
    {
        public Vector2 velocity;
        public Vector2 position;
        public GameObject boidGameObject;

        public Boid(Vector2 pos, Vector2 vel, GameObject boid)
        {
            velocity = vel;
            position = pos;
            boidGameObject = boid;
        }
    }
}