using System;
using BitDuc.EnhancedTimeline.Animator.Parameters;
using UnityEngine;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Animator
{
    /// @cond EXCLUDE
    [Serializable]
    public class  AnimatorParameterBehaviour : PlayableBehaviour
    {
        public int IntegerValue => Mathf.FloorToInt(value);
        public bool BooleanValue => value >= 1f;
        public float FloatValue => value;

        [SerializeField] string parameterName;
        [SerializeField] AnimatorParameterType parameterType;
        [SerializeField] PostEndValue postEndValue;
        [SerializeField, HideInInspector] bool resetAtEnd;
        [SerializeField] float value;

        ClipState<AnimatorParameterClip, UnityEngine.Animator> state;
        ParameterUpdater parameterUpdater;

        public void Setup(AnimatorParameterClip parameterClip)
        {
            state = new ClipState<AnimatorParameterClip, UnityEngine.Animator>(parameterClip, StartClip, UpdateClip, EndClip);
            var parameterHash = UnityEngine.Animator.StringToHash(parameterName);
            parameterUpdater = parameterType switch {
                AnimatorParameterType.Trigger => new TriggerUpdater(parameterHash, resetAtEnd),
                AnimatorParameterType.Boolean => new BooleanUpdater(parameterHash, postEndValue),
                AnimatorParameterType.Integer => new IntegerUpdater(parameterHash, postEndValue),
                AnimatorParameterType.Float => new FloatUpdater(parameterHash, postEndValue),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        void StartClip(Playable playable, AnimatorParameterClip parameterClip, UnityEngine.Animator animator) =>
            parameterUpdater.Start(animator);

        void UpdateClip(Playable playable, FrameData frame, AnimatorParameterClip parameterClip, UnityEngine.Animator animator) =>
            parameterUpdater.Update(animator, this);

        void EndClip(Playable playable, AnimatorParameterClip parameterClip, UnityEngine.Animator animator) =>
            parameterUpdater.End(animator);

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) =>
            state.ProcessFrame(playable, info, playerData);

        public override void OnBehaviourPause(Playable playable, FrameData frame) =>
            state.BehaviorPause(playable, frame);

        public override void OnGraphStop(Playable playable) =>
            state.GraphStop(playable);
    }
}
