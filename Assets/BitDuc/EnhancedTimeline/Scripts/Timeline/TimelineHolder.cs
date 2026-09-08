using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using BitDuc.EnhancedTimeline.Utilities;

namespace BitDuc.EnhancedTimeline.Timeline
{
    /**
     * Creates GameObjects with PlayableDirectors for each timeline added to the holder, the directors can be used to
     * hold references to scene objects for its associated timeline.
     *
     * When on the same GameObject, the @ref EnhancedTimelinePlayer will use this holder to create PlayableDirectors to
     * play the timelines with the proper scene references.
     */
    public class TimelineHolder : MonoBehaviour
    {
        [SerializeField] PlayableAsset[] timelines;
        [SerializeField] bool deleteUnusedEditorsAutomatically = true;

        Dictionary<PlayableAsset, PlayableDirector> directorsByTimeline;
        Dictionary<PlayableAsset, Pool> poolsByTimeline;

        public GameObject AcquirePlayer(PlayableAsset timeline)
        {
            var gotPool = poolsByTimeline.TryGetValue(timeline, out var pool);
            return gotPool ? pool.Acquire() : null;
        }

        public bool ReleasePlayer(PlayableAsset timeline, GameObject player)
        {
            if (!poolsByTimeline.TryGetValue(timeline, out var pool))
                return false;

            pool.Release(player);
            return true;
        }

        void Awake()
        {
            directorsByTimeline = GetDirectorsByTimeline();
            poolsByTimeline = BuildPoolsByTimeline();
        }

        void OnValidate()
        {
            var currentTimelines = (timelines ?? Array.Empty<PlayableAsset>())
                .Where(timeline => timeline != null)
                .ToHashSet();

            var directorsToDelete = Directors()
                .Where(director => !currentTimelines.Contains(director.playableAsset));

            foreach (var timeline in currentTimelines)
                CreatePlayableDirectorIfNotExists("Edit Timeline: " + timeline.name, timeline);

            directorsByTimeline = GetDirectorsByTimeline();

            if (deleteUnusedEditorsAutomatically)
                StartCoroutine(Destroy(directorsToDelete));
        }

        void CreatePlayableDirectorIfNotExists(string directorName, PlayableAsset timeline)
        {
            if (Directors().Any(director => director.playableAsset == timeline))
                return;

            var directorGameObject = new GameObject(directorName);
            var director = directorGameObject.AddComponent<PlayableDirector>();
            director.playOnAwake = false;
            director.playableAsset = timeline;
            directorGameObject.transform.parent = transform;
        }

        IEnumerable<PlayableDirector> Directors() =>
            transform.GetComponentsInChildren<PlayableDirector>()
                .Where(director => director.name.StartsWith("Edit Timeline: "));

        static IEnumerator Destroy(IEnumerable<PlayableDirector> directorsToDelete)
        {
            yield return new WaitForEndOfFrame();

            foreach (var director in directorsToDelete)
                DestroyImmediate(director.gameObject);
        }

        Dictionary<PlayableAsset, PlayableDirector> GetDirectorsByTimeline() =>
            Directors().ToDictionary(
                director => director.playableAsset,
                director => director
            );

        Dictionary<PlayableAsset, Pool> BuildPoolsByTimeline() =>
            directorsByTimeline.ToDictionary(
                pair => pair.Key,
                pair => new Pool(DirectorPrototype.BuildFrom(pair.Value.gameObject, transform), transform)
            );
    }
}
