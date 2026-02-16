using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enviroment.Birdoid
{
    public sealed class BoidSystemWithAJob : MonoBehaviour
    {
        [SerializeField] private GameObject boidVisual;
        [SerializeField] private int boidAmount = 5;
        [SerializeField] private float cohesion = 100;
        [SerializeField] private float alignment = 8;
        [SerializeField] private float separationRange = 100;
        [SerializeField] private float alignmentRange = 50;
        [SerializeField] private float boidSpeed = 1;
        [SerializeField] private float randomSpawnRange = 5;
        [SerializeField] private Vector2 bounds = Vector2.one * 100;
        [SerializeField] private float turnAwayFromBounds = 10;
        
        private List<GameObject> _boids = new ();
        
        struct BirdoidWithJob : IJob
        {
            public NativeArray<Vector3> positions;
            public NativeArray<Vector3> velocities;
            public float deltaTime;
            
            public void Execute()
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] += velocities[i] * deltaTime;
                }
            }
        }

        private void Awake()
        {
            for (int i = 0; i < boidAmount; i++)
            {
                _boids.Add(Instantiate(boidVisual, transform));
            }
        }

        private void Update()
        {
            NativeArray<Vector3> positions = new (boidAmount, Allocator.TempJob);
            NativeArray<Vector3> velocities = new (boidAmount, Allocator.TempJob);

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new(Random.Range(-randomSpawnRange, randomSpawnRange), Random.Range(-randomSpawnRange, randomSpawnRange));;
                velocities[i] = positions[i];
            }

            BirdoidWithJob job = new ()
            {
                positions = positions,
                velocities = velocities,
                deltaTime = Time.deltaTime
            };

            JobHandle handle = job.Schedule();
            handle.Complete();
            
            // do stuff here
            //Debug.Log(positions[0]);
            for (int i = 0; i < boidAmount; i++)
            {
                if (positions.Length < 0)
                    break;
                
                GameObject boidGameObject = _boids[i];
                boidGameObject.transform.position = positions[i];
            
                Vector3 diff = (positions[i] + velocities[i] - positions[i]).normalized;
                float z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                boidGameObject.transform.rotation = Quaternion.Euler(0f, 0f, z - 90);
            }
            
            positions.Dispose();
            velocities.Dispose();
        }
    }
}