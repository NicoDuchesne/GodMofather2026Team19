using BitDuc.EnhancedTimeline.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BitDuc.EnhancedTimeline.Observable
{
    /// @cond EXCLUDE
    [TrackClipType(typeof(ObservableClip))]
    [TrackBindingType(typeof(TimelineBus))]
    [TrackColor(1f, 0.4f, 0f)]
    internal class ObservableTrack : TrackAsset
    {
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            var result = base.CreatePlayable(graph, gameObject, clip);

            foreach(var timelineClip in GetClips())
                ((ObservableClip)timelineClip.asset).TimelineClip = timelineClip;

            return result;
        }
    }
}
