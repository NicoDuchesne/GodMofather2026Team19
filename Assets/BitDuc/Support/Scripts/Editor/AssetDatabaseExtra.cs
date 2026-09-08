using UnityEditor;
using UnityEngine;

namespace BitDuc.Support.Editor
{
    public static class AssetDatabaseExtra
    {
        public static T LoadFromGuid<T>(string guid) where T : Object =>
            AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
    }
}