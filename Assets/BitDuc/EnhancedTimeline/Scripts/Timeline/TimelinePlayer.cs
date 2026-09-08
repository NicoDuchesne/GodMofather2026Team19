using System.Collections.Generic;
using BitDuc.EnhancedTimeline.Observable;
using R3;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Timeline
{
    /**
     * Abstraction that can play, pause, and stop playable timelines from
     * @ref BitDuc.EnhancedTimeline.PlayableSerialization.PlayableReference "PlayableReference" objects.
     */
    public interface TimelinePlayer
    {
        /**
         * Gets all timelines currently being played
         */
        public IEnumerable<PlayableAsset> CurrentlyPlaying { get; }

        /**
         * Play a timeline. Events emitted in the returned observable are also emitted by the timeline player.
         *
         * @param timeline A timeline to play.
         * @return An observable emitting the markers and clips in the timeline.
         */
        Observable<TimelineEvent> Play(PlayableAsset timeline);

        /**
         * Pause a timeline.
         *
         * @param timeline The timeline to pause.
         */
        void Pause(PlayableAsset timeline);

        /**
         * resume a timeline.
         *
         * @param timeline The timeline to resume.
         */
        void Resume(PlayableAsset timeline);

        /**
         * Pause all currently playing timelines.
         */
        void PauseAll();

        /**
         * Resume all paused timelines.
         */
        void ResumeAll();

        /**
         * Stop a timeline. Events will no longer be emitted, and the corresponding observables is completed.
         *
         * @param timeline A playing timeline to stop.
         */
        void Stop(PlayableAsset timeline);

        /**
         * Breaks the loop the next time, if any. Next time the timeline would encounter a loop, it will ignore it.
         * Loop breaks can be stacked.
         * 
         * @param timeline The playing timeline to break the loop.
         */
        void BreakLoop(PlayableAsset timeline);

        /**
         * Listen for timeline events of a specific type.
         *
         * @param <T> The specific type of event to listen for.
         * @return An observable of the specified type.
         */
        Observable<T> Listen<T>() where T : TimelineEvent;
    }
}
