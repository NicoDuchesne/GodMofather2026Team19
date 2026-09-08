namespace BitDuc.EnhancedTimeline.Animator.Parameters
{
    /// @cond EXCLUDE
    internal class FloatUpdater : ParameterUpdater
    {
        readonly int hash;
        readonly PostEndValue postEndValue;
        
        float previousValue;

        public FloatUpdater(int hash, PostEndValue postEndValue)
        {
            this.hash = hash;
            this.postEndValue = postEndValue;
        }

        public void Start(UnityEngine.Animator animator)
        {
            if (animator == null)
                return;

            previousValue = animator.GetFloat(hash);
        }

        public void Update(UnityEngine.Animator animator, AnimatorParameterBehaviour behavior)
        {
            if (animator == null)
                return;

            animator.SetFloat(hash, behavior.FloatValue);
        }

        public void End(UnityEngine.Animator animator)
        {
            if (animator == null || postEndValue == PostEndValue.LeaveAsIs)
                return;

            var finalValue = postEndValue == PostEndValue.Revert ? previousValue : default;
            animator.SetFloat(hash, finalValue);
        }
    }
}