using System;
using UnityEngine;

namespace BitDuc.Support.Math
{
    public static class ValueExtensions
    {
        public static float To(this float value, Func<float, float> map) => map(value);

        public static Vector2 To(this float value, Func<float, (float x, float y)> map)
        {
            var (x, y) = map(value);
            return new(x, y);
        }

        public static Vector3 To(this float value, Func<float, (float x, float y, float z)> map)
        {
            var (x, y, z) = map(value);
            return new(x, y, z);
        }

        public static Color ToColor(this float value, Func<float, (float r, float g, float b)> map)
        {
            var (r, g, b) = map(value);
            return new(r, g, b);
        }

        public static Color ToColor(this float value, Func<float, (float r, float g, float b, float a)> map)
        {
            var (r, g, b, a) = map(value);
            return new(r, g, b, a);
        }
    }
}
