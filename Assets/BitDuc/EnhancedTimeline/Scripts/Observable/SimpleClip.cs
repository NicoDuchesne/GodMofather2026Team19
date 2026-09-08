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
     * Just inherit from this class to make your clip available to be added to an **Observable Track** on a
     * **Timeline**.
     *
     * This class offers only a default behaviour, showing just as a clip with duration in the **Timeline**.
     */
    public class SimpleClip : ObservableClip
    {
        /**
         * An observable that starts when the clip starts, emits **FrameData** on every clip update, and completes when
         * the clip finishes.
         */
        public Observable<FrameData> OnClipUpdate { get; private set; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) =>
            AnimatedBehaviour.Create(graph, owner, this);

        public override void StartClip(Observable<FrameData> frames)
        {
            OnClipUpdate = frames;
        }
    }
}
