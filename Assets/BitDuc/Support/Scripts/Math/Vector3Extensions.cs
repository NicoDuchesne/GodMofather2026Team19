using System;
using UnityEngine;

namespace BitDuc.Support.Math
{
    public static class Vector3Extensions
    {
        public static float To(this Vector3 vector, Func<float, float, float, float> map) =>
            map(vector.x, vector.y, vector.z);

        public static Vector2 To(this Vector3 vector, Func<float, float, float, (float x, float y)> map)
        {
            var (x, y) = map(vector.x, vector.y, vector.z);
            return new(x, y);
        }   

        public static Vector3 To(this Vector3 vector, Func<float, float, float, (float x, float y, float z)> map)
        {
            var (x, y, z) = map(vector.x, vector.y, vector.z);
            return new(x, y, z);
        }   

        public static Color ToColor(this Vector3 vector, Func<float, float, float, (float r, float g, float b)> map)
        {
            var (r, g, b) = map(vector.x, vector.y, vector.z);
            return new(r, g, b);
        }   

        public static Color ToColor(this Vector3 vector, Func<float, float, float, (float r, float g, float b, float a)> map)
        {
            var (r, g, b, a) = map(vector.x, vector.y, vector.z);
            return new(r, g, b, a);
        }   
    }
}