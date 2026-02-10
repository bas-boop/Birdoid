using UnityEngine;

namespace Framework.ObjectPooling
{
    public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
    {
        /// <summary>
        /// Initializes the pooled object with the specified position, rotation, and object pool reference.
        /// </summary>
        /// <param name="position">The position to place the object.</param>
        /// <param name="rotation">The rotation of the object.</param>
        /// <param name="objectPool">The object pool managing this object.</param>
        void Create(
            Vector3 position,
            Quaternion rotation,
            ObjectPool<T> objectPool);

        void Delete();
        
        /// <summary>
        /// Activates the pooled object, making it visible and operational.
        /// </summary>
        void Activate();
        
        /// <summary>
        /// Deactivates the pooled object, making it invisible and non-operational.
        /// </summary>
        void Deactivate();
        
        /// <summary>
        /// Returns the pooled object back to its object pool for reuse.
        /// </summary>
        void ReturnToPool();
    }
}