using BitDuc.EnhancedTimeline.Animator;
using BitDuc.EnhancedTimeline.Timeline;
using UnityEngine;

namespace BitDuc.EnhancedTimeline.Demos.SinglePlayer
{
    public class SinglePlayerFighter : MonoBehaviour
    {
        [SerializeField] bool controlledByPlayer;

        void Start()
        {
            var fighter = GetComponent<Fighter>();
            var player = GetComponent<EnhancedTimelinePlayer>();
            var animator = GetComponent<LocalAnimator>();
            var input = controlledByPlayer ?
                GetComponent<LocalFighterInput>() as FighterInput :
                new NoFighterInput();

            fighter.StartWith(player, animator, input);
        }
    }
}
