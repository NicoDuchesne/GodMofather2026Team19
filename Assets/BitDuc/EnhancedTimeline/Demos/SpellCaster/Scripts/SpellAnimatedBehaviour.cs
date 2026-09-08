using System;
using BitDuc.EnhancedTimeline.Observable;

namespace BitDuc.EnhancedTimeline.Demos.SpellCaster
{
    [Serializable]
    public class SpellAnimatedBehaviour: AnimatedBehaviour
    {
        public float flareSize;
        public float orbSize;
        public float smallParticlesDampening;
        public float smallParticlesSpawnRate;
    }
}