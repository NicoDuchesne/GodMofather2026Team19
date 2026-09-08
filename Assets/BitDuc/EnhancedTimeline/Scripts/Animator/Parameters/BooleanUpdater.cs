namespace BitDuc.EnhancedTimeline.Animator.Parameters
{
    /// @cond EXCLUDE
    internal class BooleanUpdater : ParameterUpdater
    {
        readonly int hash;
        readonly PostEndValue postEndValue;

        bool previousValue;

        public BooleanUpdater(int hash, PostEndValue postEndValue)
        {
            this.hash = hash;
            this.postEndValue = postEndValue;
        }

        public void Start(UnityEngine.Animator animator)
        {
            if (animator == null)
                return;

            previousValue = animator.GetBool(hash);
        }

        public void Update(UnityEngine.Animator animator, AnimatorParameterBehaviour behavior)
        {
            if (animator == null)
                return;

            animator.SetBool(hash, behavior.BooleanValue);
        }

        public void End(UnityEngine.Animator animator)
        {
            if (animator == null || postEndValue == PostEndValue.LeaveAsIs)
                return;

            var finalValue = postEndValue == PostEndValue.Revert ? previousValue : default;
            animator.SetBool(hash, finalValue);
        }
    }
}