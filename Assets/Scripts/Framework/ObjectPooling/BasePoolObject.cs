using System;
using UnityEngine;

namespace Framework.ObjectPooling
{
    public class BasePoolObject : MonoBehaviour, IPoolable<BasePoolObject>
    {
        public Action OnReset;
        
        protected ObjectPool<BasePoolObject> p_pool;

        /// <summary>
        /// Creates the object at the specified position and rotation using the provided object pool.
        /// </summary>
        /// <param name="position">Position to create the object.</param>
        /// <param name="rotation">Rotation to apply to the object.</param>
        /// <param name="objectPool">Object pool to which the object belongs.</param>
        public void Create(
            Vector3 position,
            Quaternion rotation,
            ObjectPool<BasePoolObject> objectPool)
        {
            Transform t = transform;
            t.position = position;
            t.rotation = rotation;
            p_pool = objectPool;
        }

        /// <summary>
        /// Deletes the object from the scene and clears its reference to the object pool.
        /// </summary>
        public void Delete()
        {
            p_pool = null;
            Destroy(gameObject);
        }

        /// <summary>
        /// Activates the object, making it active in the scene.
        /// </summary>
        public void Activate()
        {
            gameObject.SetActive(true);
            OnReset?.Invoke();
        }

        /// <summary>
        /// Deactivates the object, making it inactive in the scene.
        /// </summary>
        public void Deactivate() => gameObject.SetActive(false);

        /// <summary>
        /// Returns the object to its object pool.
        /// </summary>
        public void ReturnToPool() => p_pool.ReturnObject(this);
    }
}