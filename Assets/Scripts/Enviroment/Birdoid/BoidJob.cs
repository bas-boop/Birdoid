using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Enviroment.Birdoid
{
    [BurstCompile]
    public struct BoidJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector2> positions;
        [ReadOnly] public NativeArray<Vector2> velocities;
        public NativeArray<Vector2> newVelocities;
        
        public float cohesion;
        public float alignment;
        public float separationRange;
        public float alignmentRange;
        public float boidSpeed;
        public Vector2 bounds;
        public float turnAwayFromBounds;
        public float deltaTime;
        public int boidCount;
        
        public void Execute(int index)
        {
            Vector2 velocity = velocities[index];
            Vector2 position = positions[index];
            
            Vector2 v1 = CalculateCohesion(index, position);
            Vector2 v2 = CalculateSeparation(index, position);
            Vector2 v3 = CalculateAlignment(index, position, velocity);
            
            velocity += v1 + v2 + v3;
            velocity = ApplyBounds(position, velocity);
            velocity = velocity.normalized * (deltaTime * boidSpeed);
            
            newVelocities[index] = velocity;
        }
        
        private Vector2 CalculateCohesion(int targetIndex, Vector2 targetPosition)
        {
            Vector2 p = Vector2.zero;
            
            for (int i = 0; i < boidCount; i++)
            {
                if (i != targetIndex)
                    p += positions[i];
            }
            
            p /= boidCount - 1;
            return (p - targetPosition) / cohesion;
        }
        
        private Vector2 CalculateSeparation(int targetIndex, Vector2 targetPosition)
        {
            Vector2 c = Vector2.zero;
            
            for (int i = 0; i < boidCount; i++)
            {
                if (i == targetIndex)
                    continue;
                
                Vector2 diff = positions[i] - targetPosition;
                    
                if (diff.magnitude < separationRange)
                    c -= diff;
            }
            
            return c;
        }
        
        private Vector2 CalculateAlignment(int targetIndex, Vector2 targetPosition, Vector2 targetVelocity)
        {
            Vector2 pv = Vector2.zero;
            
            for (int i = 0; i < boidCount; i++)
            {
                if (i == targetIndex)
                    continue;
                
                Vector2 diff = positions[i] - targetPosition;
                    
                if (diff.magnitude < alignmentRange)
                    pv += velocities[i];
            }
            
            pv /= boidCount - 1;
            return (pv - targetVelocity) / alignment;
        }
        
        private Vector2 ApplyBounds(Vector2 position, Vector2 velocity)
        {
            if (position.x < -bounds.x)
                velocity.x += turnAwayFromBounds;
            
            if (position.x > bounds.x)
                velocity.x -= turnAwayFromBounds;
            
            if (position.y < -bounds.y)
                velocity.y += turnAwayFromBounds;
            
            if (position.y > bounds.y)
                velocity.y -= turnAwayFromBounds;
            
            return velocity;
        }
    }
}