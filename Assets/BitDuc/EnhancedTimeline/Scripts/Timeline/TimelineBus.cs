using BitDuc.EnhancedTimeline.Observable;
using R3;
using UnityEngine;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Timeline
{
    /**
     * Allows emitting and listening for events. It typically receives
     * @ref BitDuc.EnhancedTimeline.Observable.ObservableMarker "ObservableMarker" notifications
     * from **PlayableDirector** components and emits them as events; it also receives
     * @ref BitDuc.EnhancedTimeline.Observable.ObservableClip "ObservableClip" events from
     * @ref BitDuc.EnhancedTimeline.Observable.ObservablePlayableBehaviour "ObservablePlayableBehaviour" and emits them
     * as events.
     * 
     * This component is only required to listen to events from a timeline if you are using Unity's **PlayableDirector**
     * instead of the @ref BitDuc.EnhancedTimeline.Observable.EnhancedTimelinePlayer "EnhancedTimelinePlayer"; add it to
     * a **GameObject** with a **PlayableDirector**.  
     */
    public class TimelineBus : MonoBehaviour, INotificationReceiver
    {
        readonly Subject<TimelineEvent> events = new();

        /**
         * Listen to a specific type of event.
         *
         * @param <T> The specific type of event to listen.
         * @return An observable that emits events of the type <T>.
         */
        public Observable<T> Listen<T>() where T : TimelineEvent =>
            events.OfType<TimelineEvent, T>();

        /**
         * Emits a clip as an event.
         *
         * @param clip The clip to emit.
         */
        public void Emit(ObservableClip clip) =>
            events.OnNext(clip);

        /**
         * Receives notifications from a **PlayableDirector**, and emits it as an event if its an
         * @ref BitDuc.EnhancedTimeline.Observable.ObservableMarker "ObservableMarker".
         *
         * @param origin The **Playable** instance that triggered the notification.
         * @param notification The **INotification** instance received from the **PlayableDirector**.
         * @param context The context object associated with the notification.
         */
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is SimpleMarker marker)
                events.OnNext(marker);
        }
    }
}
