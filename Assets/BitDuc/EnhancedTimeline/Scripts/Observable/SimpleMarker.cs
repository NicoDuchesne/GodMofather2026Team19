using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BitDuc.EnhancedTimeline.Observable
{
    /**
     * Simple marker on an observable track. Emitted as an event from the
     * @ref BitDuc.EnhancedTimeline.Timeline.EnhancedTimelinePlayer "EnhancedTimelinePlayer" and
     * @ref BitDuc.EnhancedTimeline.Timeline.TimelineBus "TimelineBus" components.
     */
    [CustomStyle("SignalEmitter")]
    public class SimpleMarker : Marker, INotification, TimelineEvent
    {
        public PropertyName id => new();
    }
}
