namespace BitDuc.EnhancedTimeline.Animator.Parameters
{
    /// @cond EXCLUDE
    internal class TriggerUpdater : ParameterUpdater
    {
        readonly int hash;
        readonly bool resetAtEnd;

        public TriggerUpdater(int hash, bool resetAtEnd)
        {
            this.hash = hash;
            this.resetAtEnd = resetAtEnd;
        }

        public void Start(UnityEngine.Animator animator)
        {
            if (animator == null)
                return;

            animator.SetTrigger(hash);
        }

        public void Update(UnityEngine.Animator animator, AnimatorParameterBehaviour behavior) { }

        public void End(UnityEngine.Animator animator)
        {
            if (animator == null || !resetAtEnd)
                return;

            animator.ResetTrigger(hash);
        }
    }
}
