using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enviroment.Birdoid
{
    public sealed class BoidSystem : MonoBehaviour
    {
        [SerializeField] private GameObject boidVisual;
        [SerializeField] private int boidAmount = 5;
        private List<Boid> _boids = new ();

        private void Start()
        {
            for (int i = 0; i < boidAmount; i++)
            {
                _boids.Add(new (Vector2.one * i, Vector2.one * i, Instantiate(boidVisual)));
            }
        }

        private void Update()
        {
            if (_boids.Count <= 0)
                return;
            
            UpdateBoid();

            foreach (Boid boid in _boids)
            {
                boid.boidGameObject.transform.position = boid.position;
            }
        }

        private void UpdateBoid()
        {
            foreach (Boid boid in _boids)
            {
                Vector2 v1 = Rule1(boid);
                Vector2 v2 = Rule2(boid);
                Vector2 v3 = Rule3(boid);

                boid.velocity += v1 + v2 + v3;
                boid.position += boid.velocity;
                Debug.Log(boid.velocity + " " + boid.position);
            }
        }

        private Vector2 Rule1(Boid targetBoid)
        {
            Vector2 p = Vector2.zero;

            foreach (Boid boid in _boids)
            {
                if (boid != targetBoid)
                {
                    p += boid.position;
                }
            }

            p /= _boids.Count - 1;
            return (p - targetBoid.position) / 100;
        }

        private Vector2 Rule2(Boid targetBoid)
        {
            Vector2 c = Vector2.zero;

            foreach (Boid boid in _boids)
            {
                if (boid != targetBoid
                    && (boid.position - targetBoid.position).magnitude < 100)
                {
                    c -= boid.position - targetBoid.position;
                }
            }

            return c;
        }

        private Vector2 Rule3(Boid targetBoid)
        {
            Vector2 pv = Vector2.zero;
            
            foreach (Boid boid in _boids)
            {
                if (boid != targetBoid)
                {
                    pv += boid.velocity;
                }
            }
            
            pv /= _boids.Count - 1;

            return (pv - targetBoid.velocity) / 8;
        }
    }
}