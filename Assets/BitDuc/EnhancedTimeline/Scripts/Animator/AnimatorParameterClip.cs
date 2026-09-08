using System;
using UnityEngine;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Animator
{
    /// @cond EXCLUDE
    [Serializable]
    public class AnimatorParameterClip : PlayableAsset
    {
        public AnimatorParameterBehaviour template = new();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var scriptPlayable = ScriptPlayable<AnimatorParameterBehaviour>.Create(graph, template);
            scriptPlayable.GetBehaviour().Setup(this);
            return scriptPlayable;
        }
    }
}
