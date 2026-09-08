namespace BitDuc.EnhancedTimeline.Animator.Parameters
{
    /// @cond EXCLUDE
    internal interface ParameterUpdater
    {
        void Start(UnityEngine.Animator animator);
        void Update(UnityEngine.Animator animator, AnimatorParameterBehaviour behavior);
        void End(UnityEngine.Animator animator);
    }
}