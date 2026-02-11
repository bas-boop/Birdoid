using System;
using System.Collections.Generic;
using Framework.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enviroment.Birdoid
{
    public sealed class BoidSystem : MonoBehaviour
    {
        [SerializeField] private GameObject boidVisual;
        [SerializeField] private int boidAmount = 5;
        [SerializeField] private float cohesion = 100;
        [SerializeField] private float alignment = 8;
        [SerializeField] private float separationRange = 100;
        [SerializeField] private float alignmentRange = 50;
        [SerializeField] private float boidSpeed = 1;
        [SerializeField] private float randomSpawnRange = 5;
        
        private List<Boid> _boids = new ();

        private void Start()
        {
            for (int i = 0; i < boidAmount; i++)
            {
                Vector2 a = new(Random.Range(-randomSpawnRange, randomSpawnRange), Random.Range(-randomSpawnRange, randomSpawnRange));
                _boids.Add(new (a, a, Instantiate(boidVisual)));
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
                Vector2 v1 = Cohesion(boid);
                Vector2 v2 = Separation(boid);
                Vector2 v3 = Alignment(boid);

                boid.velocity += v1 + v2 + v3;
                boid.velocity = boid.velocity.normalized * (Time.deltaTime * boidSpeed);
                boid.position += boid.velocity;
            }
        }

        private Vector2 Cohesion(Boid targetBoid)
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
            return (p - targetBoid.position) / cohesion;
        }

        private Vector2 Separation(Boid targetBoid)
        {
            Vector2 c = Vector2.zero;

            foreach (Boid boid in _boids)
            {
                if (boid != targetBoid
                    && (boid.position - targetBoid.position).magnitude < separationRange)
                {
                    c -= boid.position - targetBoid.position;
                }
            }

            return c;
        }

        private Vector2 Alignment(Boid targetBoid)
        {
            Vector2 pv = Vector2.zero;
            
            foreach (Boid boid in _boids)
            {
                if (boid != targetBoid
                    && (boid.position - targetBoid.position).magnitude < alignmentRange)
                {
                    pv += boid.velocity;
                }
            }
            
            pv /= _boids.Count - 1;
            return (pv - targetBoid.velocity) / alignment;
        }
    }
}