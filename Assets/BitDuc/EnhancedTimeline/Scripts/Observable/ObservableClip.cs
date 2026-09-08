using R3;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BitDuc.EnhancedTimeline.Observable
{
    /**
     * Observable clip on an observable track. Emitted as an event from the
     * @ref BitDuc.EnhancedTimeline.Timeline.EnhancedTimelinePlayer "EnhancedTimelinePlayer" and
     * @ref BitDuc.EnhancedTimeline.Timeline.TimelineBus "TimelineBus" components.
     */
    public abstract class ObservableClip : PlayableAsset, TimelineEvent
    {
        public override double duration => 2.5;

        /**
         * Unity's TimelineClip associated to this `ObservableClip` 
         */
        public TimelineClip TimelineClip { get; internal set; }

        /**
         * Called when the clip is started.
         * @param frames An observable that starts, updates, and completes throughout the clip playing. 
         */
        public abstract void StartClip(Observable<FrameData> frames);
    }
}
