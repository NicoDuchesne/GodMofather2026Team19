using UnityEngine;

namespace BitDuc.Support.Math
{
    public static class MathExtensions
    {
        public static float Decay(float from, float to, float decay, float deltaTime) =>
            to + (from - to) * Mathf.Exp(-decay * deltaTime);
    }
}
