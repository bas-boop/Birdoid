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

        public void UpdateBoid()
        {
            boidGameObject.transform.position = position;
            
            Vector3 diff = (position + velocity - position).normalized;
            float z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            boidGameObject.transform.rotation = Quaternion.Euler(0f, 0f, z - 90);
        }
    }
}