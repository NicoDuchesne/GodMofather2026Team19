using System.Collections.Generic;
using BitDuc.EnhancedTimeline.Markers;
using BitDuc.EnhancedTimeline.Observable;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BitDuc.EnhancedTimeline.Timeline
{
    public class LoopHandler
    {
        public PlayableAsset Timeline => player.playableAsset;

        readonly Stack<SimpleMarker> loopStack;
        readonly PlayableDirector player;

        int breaks = 0;

        public LoopHandler(PlayableDirector player)
        {
            loopStack = new Stack<SimpleMarker>();
            this.player = player;
        }

        public void Handle(TimelineEvent @event)
        {
            switch (@event)
            {
                case LoopFromMarker loopFrom:
                    HandleLoopFrom(loopFrom);
                    break;
                case LoopToMarker:
                    HandleLoopTo();
                    break;
            }
        }

        void HandleLoopFrom(SimpleMarker loopFrom)
        {
            if (MarkerIsAlreadyPushed(loopFrom))
                return;

            loopStack.Push(loopFrom);
        }

        void HandleLoopTo()
        {
            if (!loopStack.TryPop(out var loopFrom))
                return;

            if (breaks > 0)
            {
                breaks--;
                return;
            }

            player.time = loopFrom.time;
            Handle(loopFrom);
        }

        bool MarkerIsAlreadyPushed(SimpleMarker loopFrom) =>
            loopStack.TryPeek(out var top) && top == loopFrom;

        public void Break()
        {
            breaks++;
        }
    }
}