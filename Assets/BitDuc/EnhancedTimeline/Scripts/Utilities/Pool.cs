using System.Collections.Generic;
using UnityEngine;

namespace BitDuc.EnhancedTimeline.Utilities
{
    /// @cond EXCLUDE
    internal class Pool
    {
        readonly GameObject prototype;
        readonly Transform parent;
        readonly Queue<GameObject> instances = new();
        
        public Pool(GameObject prototype, Transform parent)
        {
            this.prototype = prototype;
            this.parent = parent;
        }

        public GameObject Acquire() =>
            instances.TryDequeue(out var result) ? result : CreateInstance();

        public void Release(GameObject instance) =>
            instances.Enqueue(instance);

        GameObject CreateInstance() =>
            Object.Instantiate(prototype, prototype.transform.position, prototype.transform.rotation, parent);
    }
}
