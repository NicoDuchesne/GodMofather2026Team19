using System;
using System.Runtime.CompilerServices;
using BitDuc.EnhancedTimeline.Timeline;
using R3;
using UnityEngine;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Observable
{
    /**
     * Observable implementation of **PlayableBehaviour**, processes **PlayableBehaviour**'s events into clearer start,
     * stop, and update events sent to @ref BitDuc.EnhancedTimeline.Observable.ObservableClip "ObservableClip".
     *
     * Instantiate and  this class when implementing
     * @ref BitDuc.EnhancedTimeline.Observable.ObservableClip "ObservableClip"
     * directly instead of inheriting from @ref BitDuc.EnhancedTimeline.Observable.SimpleClip "SimpleClip".
     */
    [Serializable]
    public class AnimatedBehaviour : PlayableBehaviour
    {
        Subject<FrameData> frames;
        ClipState<ObservableClip, TimelineBus> state;

        /**
         * Create an instance of a **ScriptPlayable** with an inheritor of **ObservablePlayableBehavior**.
         * @param graph The graph received from the overriden **CreatePlayable** method from
         *     @ref BitDuc.EnhancedTimeline.Observable.ObservableClip "ObservableClip".
         * @param owner The owner received from the overriden **CreatePlayable** method from
         *     @ref BitDuc.EnhancedTimeline.Observable.ObservableClip "ObservableClip".
         * @param clip The inherited **ObservableClip**.
         * @param template An instance of the **ObservablePlayableBehaviour** being inherited.
         */
        public static ScriptPlayable<T> Create<T>(
            PlayableGraph graph, GameObject owner, ObservableClip clip, T template
        ) where T : AnimatedBehaviour, new() {
            var playable = ScriptPlayable<T>.Create(graph, template);
            playable.GetBehaviour().Setup(clip);
            return playable;
        }

        /**
         * Create an instance of a **ScriptPlayable** with the default **ObservablePlayableBehavior**.
         * @param graph The graph received from the overriden **CreatePlayable** method from
         *     @ref BitDuc.EnhancedTimeline.Observable.ObservableClip "ObservableClip".
         * @param owner The owner received from the overriden **CreatePlayable** method from
         *     @ref BitDuc.EnhancedTimeline.Observable.ObservableClip "ObservableClip".
         * @param clip The inherited **ObservableClip**.
         */
        public static ScriptPlayable<AnimatedBehaviour> Create(
            PlayableGraph graph, GameObject owner, ObservableClip clip
        ) {
            var playable = ScriptPlayable<AnimatedBehaviour>.Create(graph);
            playable.GetBehaviour().Setup(clip);
            return playable;
        }

        public override void ProcessFrame(Playable playable, FrameData frame, object playerData) =>
            state.ProcessFrame(playable, frame, playerData);

        public override void OnBehaviourPause(Playable playable, FrameData frame) =>
            state.BehaviorPause(playable, frame);

        public override void OnGraphStop(Playable playable) =>
            state.GraphStop(playable);

        void Setup(ObservableClip clip) =>
            state = new ClipState<ObservableClip, TimelineBus>(clip, StartClip, UpdateClip, StopClip);

        void StartClip(Playable playable, ObservableClip clip, TimelineBus bus)
        {
            if (bus == null)
                return;

            frames = new Subject<FrameData>();
            clip.StartClip(frames);
            bus.Emit(clip);
        }

        void UpdateClip(Playable playable, FrameData frame, ObservableClip clip, TimelineBus bus)
        {
            if (bus == null)
                return;

            frames.OnNext(frame);
        }

        void StopClip(Playable playable, ObservableClip clip, TimelineBus bus)
        {
            if (bus == null)
                return;

            frames.OnCompleted();
        }
    }
}
