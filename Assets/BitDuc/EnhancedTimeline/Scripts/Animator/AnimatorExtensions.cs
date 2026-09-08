namespace BitDuc.EnhancedTimeline.Animator
{
    public static class AnimatorExtensions
    {
        public static void SetFloat(this AnimatorService animator, string name, float value) =>
            animator.SetFloat(UnityEngine.Animator.StringToHash(name), value);

        public static void SetInteger(this AnimatorService animator, string name, int value) =>
            animator.SetInteger(UnityEngine.Animator.StringToHash(name), value);

        public static void SetBool(this AnimatorService animator, string name, bool value) =>
            animator.SetBool(UnityEngine.Animator.StringToHash(name), value);

        public static void Trigger(this AnimatorService animator, string name) =>
            animator.Trigger(UnityEngine.Animator.StringToHash(name));
    }
}