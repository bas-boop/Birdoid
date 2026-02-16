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

        [Header("jobs")]
        [SerializeField] private int innerloopBatchCount = 32;
        
        private List<GameObject> _boids = new();

        private NativeArray<Vector2> _positions;
        private NativeArray<Vector2> _velocities;

        private void Start()
        {
            _positions = new (boidAmount, Allocator.Persistent);
            _velocities = new (boidAmount, Allocator.Persistent);
            
            for (int i = 0; i < boidAmount; i++)
            {
                Vector2 randomPos = new (Random.Range(-randomSpawnRange, randomSpawnRange), Random.Range(-randomSpawnRange, randomSpawnRange));
                
                _positions[i] = randomPos;
                _velocities[i] = randomPos.normalized;
                _boids.Add(Instantiate(boidVisual, transform));
            }
        }

        private void Update()
        {
            if (_boids.Count <= 0)
                return;
            
            NativeArray<Vector2> newVelocities = new (boidAmount, Allocator.TempJob);
            
            BoidJob job = new()
            {
                positions = _positions,
                velocities = _velocities,
                newVelocities = newVelocities,
                cohesion = cohesion,
                alignment = alignment,
                separationRange = separationRange,
                alignmentRange = alignmentRange,
                boidSpeed = boidSpeed,
                bounds = bounds,
                turnAwayFromBounds = turnAwayFromBounds,
                deltaTime = Time.deltaTime,
                boidCount = boidAmount
            };
            
            JobHandle handle = job.Schedule(boidAmount, innerloopBatchCount);
            handle.Complete();
            
            for (int i = 0; i < boidAmount; i++)
            {
                _velocities[i] = newVelocities[i];
                _positions[i] += _velocities[i];
                
                GameObject boidGameObject = _boids[i];
                boidGameObject.transform.position = _positions[i];
                
                Vector2 vel = _velocities[i];
                float z = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                boidGameObject.transform.rotation = Quaternion.Euler(0f, 0f, z - 90);
            }
            
            newVelocities.Dispose();
        }

        private void OnDestroy()
        {
            if (_positions.IsCreated)
                _positions.Dispose();
            
            if (_velocities.IsCreated)
                _velocities.Dispose();
        }
    }
}