using BitDuc.EnhancedTimeline.Observable;
using BitDuc.EnhancedTimeline.Timeline;
using R3;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.VFX;

namespace BitDuc.EnhancedTimeline.Demos.SpellCaster
{
    [RequireComponent(typeof(EnhancedTimelinePlayer))]
    public class Caster : MonoBehaviour
    {
        [SerializeField] TimelineAsset cast;
        [SerializeField] Spell spell;
        [SerializeField] VisualEffect spellEffect;

        EnhancedTimelinePlayer player;

        void Awake()
        {
            player = GetComponent<EnhancedTimelinePlayer>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && spell.State == State.None)
            {
                spell.Cast();
                SubscribeSpellEvents();
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                player.BreakLoop(cast);
            }
        }

        void SubscribeSpellEvents()
        {
            var events = player.Play(cast);

            events.OfType<TimelineEvent, PassSpellMarker>()
                .Subscribe(_ => PassSpellToRightHand());

            events.OfType<TimelineEvent, SpellClip>()
                .SelectMany(clip => clip.OnClipUpdate)
                .Subscribe(UpdateGraphVFX);
        }

        void UpdateGraphVFX((FrameData frame, SpellAnimatedBehaviour behavior) data)
        {
            var visualEffect = GetComponentInChildren<VisualEffect>(includeInactive:true);
            visualEffect.SetFloat("flare_size", data.behavior.flareSize);
            visualEffect.SetFloat("orb_size", data.behavior.orbSize);
            visualEffect.SetFloat("smallParticles_dampening", data.behavior.smallParticlesDampening);
            visualEffect.SetFloat("smallParticles_spawnRate", data.behavior.smallParticlesSpawnRate);
        }

        void PassSpellToRightHand()
        {
            spell.Pass();
        }
    }
}
