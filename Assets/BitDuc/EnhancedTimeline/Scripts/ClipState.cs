using System;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline
{
    /// @cond EXCLUDE
    internal class ClipState<T, R> where T : PlayableAsset
    {
        readonly T clip;
        readonly Action<Playable, T, R> start;
        readonly Action<Playable, FrameData, T, R> update;
        readonly Action<Playable, T, R> end;

        R target;
        bool hasStarted;
        bool hasEnded;

        public ClipState(
            T clip,
            Action<Playable, T, R> start = null,
            Action<Playable, FrameData, T, R> update = null,
            Action<Playable, T, R> end = null
        ) {
            this.clip = clip;
            this.start = start;
            this.update = update;
            this.end = end;
        }

        public void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var target = (R) playerData;

            if (!hasStarted)
            {
                hasStarted = true;
                this.target = target;
                start?.Invoke(playable, clip, target);
            }

            update?.Invoke(playable, info, clip, target);
        }

        public void BehaviorPause(Playable playable, FrameData info)
        {
            if (ClipHasCompleted(info, playable) || TimelineHasCompleted(playable))
                End(playable);
        }

        public void GraphStop(Playable playable)
        {
            if (target != null)
                End(playable);
        }

        static bool ClipHasCompleted(FrameData info, Playable playable) =>
            IsEffectivelyPaused(info) && TimeIsCompleted(info, playable);

        static bool IsEffectivelyPaused(FrameData info) =>
            info.effectivePlayState == PlayState.Paused;

        static bool TimeIsCompleted(FrameData info, Playable playable) =>
            playable.GetTime() + info.deltaTime >= playable.GetDuration();

        static bool TimelineHasCompleted(Playable playable) =>
            playable.GetGraph().GetRootPlayable(0).IsDone();

        void End(Playable playable)
        {
            if (hasEnded)
                return;

            hasEnded = true;
            end?.Invoke(playable, clip, target);
        }
    }
}