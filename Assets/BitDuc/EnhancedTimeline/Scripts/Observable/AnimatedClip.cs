using R3;
using UnityEngine;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Observable
{
    /**
     * A simplified observable clip on an observable track. Emitted as an event from the
     * @ref BitDuc.EnhancedTimeline.Timeline.EnhancedTimelinePlayer "EnhancedTimelinePlayer" and
     * @ref BitDuc.EnhancedTimeline.Timeline.TimelineBus "TimelineBus" components.
     *
     * Inherit from this class to make your clip available to be added to an **Observable Track** on a **Timeline**.
     *
     * This class allows custom behaviour by using a class inheriting from
     * @ref BitDuc.EnhancedTimeline.Observable.ObservablePlayableBehaviour "ObservablePlayableBehaviour" as **T**.
     * Values declared in the **T** type will be interpolated throughout the clip's reproduction. It shows as a clip with
     * values to animate with curves in the **Timeline**.
     */
    public class AnimatedClip<T> : ObservableClip where T: AnimatedBehaviour, new()
    {
        /**
         * An observable that starts when the clip starts, emits **FrameData** and **T** on every clip update, and
         * completes when the clip finishes.
         */
        public Observable<(FrameData frame, T behaviour)> OnClipUpdate { get; private set; }

        /**
         * A template instance of the type **T**.
         */
        public T template = new();

        T behaviour;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var behavior = AnimatedBehaviour.Create(graph, owner, this, template);
            behaviour = behavior.GetBehaviour();
            return behavior;
        }

        public override void StartClip(Observable<FrameData> frames)
        {
            OnClipUpdate = frames.Select(frame => (frame, behaviour));
        }
    }
}