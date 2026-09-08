using UnityEngine;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Timeline
{
    public static class DirectorPrototype
    {
        public static GameObject BuildFromScratch(Transform parent) =>
            Configure(new GameObject { transform = { parent = parent } });

        public static GameObject BuildFrom(GameObject other, Transform parent) =>
            Configure(Object.Instantiate(other, parent));

        static GameObject Configure(GameObject prototype)
        {
            prototype.name = "EnhancedTimeline - PlayableDirectorPrototype";
            prototype.hideFlags = HideFlags.HideAndDontSave;
            GetOrAddComponent<TimelineBus>(prototype);
            GetOrAddComponent<PlayableDirector>(prototype);
            return prototype;
        }

        static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            return component ? component : gameObject.AddComponent<T>();
        }
    }
}
