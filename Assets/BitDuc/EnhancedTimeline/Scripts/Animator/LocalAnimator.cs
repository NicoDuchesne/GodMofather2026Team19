using UnityEngine;

namespace BitDuc.EnhancedTimeline.Animator
{
    public class LocalAnimator : MonoBehaviour, AnimatorService
    {
        public UnityEngine.Animator animator;

        public void SetFloat(int hash, float value) =>
            animator.SetFloat(hash, value);

        public void SetInteger(int hash, int value) =>
            animator.SetInteger(hash, value);

        public void SetBool(int hash, bool value) =>
            animator.SetBool(hash, value);

        public void Trigger(int hash) =>
            animator.SetTrigger(hash);
    }
}