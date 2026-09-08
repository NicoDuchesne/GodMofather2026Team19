namespace BitDuc.EnhancedTimeline.Animator
{
    public interface AnimatorService
    {
        public void SetFloat(int hash, float value);
        public void SetInteger(int hash, int value);
        public void SetBool(int hash, bool value);
        public void Trigger(int hash);
    }
}
