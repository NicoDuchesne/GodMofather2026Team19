using System;
using System.Collections.Generic;
using System.Linq;
using BitDuc.EnhancedTimeline.Observable;
using BitDuc.EnhancedTimeline.Utilities;
using R3;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace BitDuc.EnhancedTimeline.Timeline
{
    /**
     * A timeline player that can play, pause, and stop playable timelines from
     * @ref BitDuc.EnhancedTimeline.PlayableSerialization.PlayableReference "PlayableReference" and **PlayableAsset**
     * objects.
     * It can play multiple timelines concurrently. It features simple bindings from objects to tracks, allowing simpler
     * handling of several timelines. Allows listening to events from a single timeline being played, or all on the same
     * observable.
     */
    public class EnhancedTimelinePlayer : MonoBehaviour, TimelinePlayer
    {
        public IEnumerable<PlayableAsset> CurrentlyPlaying =>
            currentlyPlaying.Select(director => director.playableAsset);

        [Serializable]
        public struct TrackBinding
        {
            public string trackName;
            public GameObject bound;
        }

        /**
         * Controls how the timeline updates every frame. It is equivalent to the **PlayableDirector.updateMethod**.
         */
        public DirectorUpdateMode updateMethod = DirectorUpdateMode.GameTime;

        /**
         * **GameObjects** used to search for **Components** to bind timeline tracks to.
         */
        public List<GameObject> bindings;

        /**
         * NamedBindings used to bind the named track to a component on the corresponding **GameObject**.
         */
        public List<TrackBinding> trackBindings;

        [Header("Debug")]
        [SerializeField]
        bool focusPlayableDirector = true;

        readonly Subject<TimelineEvent> events = new();
        readonly HashSet<PlayableDirector> currentlyPlaying = new();
        readonly HashSet<LoopHandler> loopHandlers = new();

        Pool playersPool;
        TimelineHolder timelineHolder;

        void Awake()
        {
            var prototype = DirectorPrototype.BuildFromScratch(transform);
            playersPool = new Pool(prototype.gameObject, transform);
            timelineHolder = GetComponent<TimelineHolder>();
        }

        public Observable<TimelineEvent> Play(PlayableAsset timeline)
        {
            var (player, bus) = AcquirePlayer(timeline);
            Bind(player, timeline, bus);

            var onStop = Play(timeline, player);
            var loopHandler = new LoopHandler(player);
            AddToCurrentlyPlaying(player, onStop, loopHandler);

            var timelineEvents = EmitEventsUntil(bus, onStop).Share();
            timelineEvents.Subscribe(loopHandler.Handle);

#if UNITY_EDITOR
            if (focusPlayableDirector)
                SeekBarCursorFix.Play(
                    gameObject,
                    player.gameObject,
                    timelineEvents.AsUnitObservable()
                );
#endif

            return timelineEvents;
        }

        public void Pause(PlayableAsset timeline)
        {
            var directors = currentlyPlaying
                .Where(director => director.playableAsset == timeline);

            foreach (var director in directors)
                director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        public void Resume(PlayableAsset timeline)
        {
            var directors = currentlyPlaying
                .Where(director => director.playableAsset == timeline);

            foreach (var director in directors)
                director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        public void PauseAll()
        {
            foreach (var director in currentlyPlaying)
                director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        public void ResumeAll()
        {
            foreach (var director in currentlyPlaying)
                director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        public void Stop(PlayableAsset timeline)
        { 
            var directors = currentlyPlaying
                .Where(director => director.playableAsset == timeline)
                .ToArray();

            foreach(var director in directors)
                director.Stop();
        }

        public void BreakLoop(PlayableAsset timeline)
        {
            var toBreak = loopHandlers
                .Where(loopHandler => loopHandler.Timeline == timeline)
                .ToArray();

            foreach (var handler in toBreak)
                handler.Break();
        }

        public Observable<T> Listen<T>() where T: TimelineEvent =>
            events.Where(message => message is T)
                .Select(message => (T)message);

        Observable<PlayableDirector> Play(PlayableAsset timeline, PlayableDirector player)
        {
            player.Play(timeline);
            return OnPlayableDirectorStopped(player).Take(1).Share();
        }

        void AddToCurrentlyPlaying(
            PlayableDirector player,
            Observable<PlayableDirector> onStop,
            LoopHandler loopHandler
        )
        {
            currentlyPlaying.Add(player);
            loopHandlers.Add(loopHandler);
            onStop.Subscribe(_ => ReleasePlayer(player, loopHandler));
        }

        (PlayableDirector, TimelineBus) AcquirePlayer(PlayableAsset timeline)
        {
            var playerFromHolder = timelineHolder ? timelineHolder.AcquirePlayer(timeline) : null;
            var player = playerFromHolder ? playerFromHolder : playersPool.Acquire();
            var director = player.GetComponent<PlayableDirector>();
            var bus = player.GetComponent<TimelineBus>();
            director.timeUpdateMode = updateMethod;
            player.hideFlags = HideFlags.DontSave;
            return (director, bus);
        }

        void ReleasePlayer(PlayableDirector player, LoopHandler loopHandler)
        {
            if (!timelineHolder || !timelineHolder.ReleasePlayer(player.playableAsset, player.gameObject))
                playersPool.Release(player.gameObject);

            currentlyPlaying.Remove(player);
            loopHandlers.Remove(loopHandler);
        }

        Observable<TimelineEvent> EmitEventsUntil(TimelineBus timelineEvents, Observable<PlayableDirector> onStop) =>
            timelineEvents
                .Listen<TimelineEvent>()
                .Do(PublishToBus)
                .TakeUntil(onStop)
                .DefaultIfEmpty();

        void PublishToBus(TimelineEvent @event) =>
            events.OnNext(@event);

        void Bind(PlayableDirector player, PlayableAsset timeline, TimelineBus bus)
        {
            foreach (var output in timeline.outputs)
            {
                var binding =
                    output.outputTargetType == typeof(GameObject) ? FindGameObjectBinding(output) :
                    output.outputTargetType == typeof(TimelineBus) ? bus :
                    output.outputTargetType == null ? null :
                    FindComponentBinding(output);

                if (binding != null)
                    player.SetGenericBinding(output.sourceObject, binding);
            }
        }

        GameObject FindGameObjectBinding(PlayableBinding output) =>
            FindNamedGameObjectBinding(output) ??
            bindings.FirstOrDefault();

        GameObject FindNamedGameObjectBinding(PlayableBinding output) =>
            trackBindings
                .Select(binding => (TrackBinding?)binding)
                .FirstOrDefault(binding => binding?.trackName == output.streamName)
                ?.bound;

        Object FindComponentBinding(PlayableBinding output) =>
            FindNamedComponentBinding(output) ??
            FindUnnamedComponentBinding(output);

        Object FindNamedComponentBinding(PlayableBinding output) =>
            FindNamedGameObjectBinding(output)
                ?.GetComponent(output.outputTargetType);

        Object FindUnnamedComponentBinding(PlayableBinding output) =>
            bindings
                .Select(binding => binding.GetComponent(output.outputTargetType))
                .FirstOrDefault(component => component != null);

        static Observable<PlayableDirector> OnPlayableDirectorStopped(PlayableDirector player) =>
            R3.Observable.FromEvent<PlayableDirector>(
                handler => player.stopped += handler,
                handler => player.stopped -= handler
            );
    }
}
