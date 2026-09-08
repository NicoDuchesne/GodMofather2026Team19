using UnityEditor;
using UnityEngine;
using static BitDuc.Support.Editor.AssetDatabaseExtra;

namespace BitDuc.Support.Editor
{
    public class AboutWindow : EditorWindow
    {
        Texture2D Logo => logo ??= LoadFromGuid<Texture2D>("a456b9e6d9faa461cb0982ff3e98672e");

        string product;
        string version;
        Texture2D logo = null;

        public static void Show<T>(string product, int major, int minor, int patch) where T : AboutWindow
        {
            var window = GetWindow<T>();
            window.titleContent = new GUIContent($"About {product}");
            window.product = product;
            window.version = $"{major}.{minor}.{patch}";
            var size = new Vector2(300, 220);
            window.minSize = size;
            window.maxSize = size;
        }

        void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label(product, EditorStyles.boldLabel);
            GUILayout.Label($"Version {version}", EditorStyles.label);
            GUILayout.Space(5);
            GUILayout.Label("Created by bit-duc", EditorStyles.label);

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(Logo, GUILayout.Width(100), GUILayout.Height(100));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("bitduc.com"))
                Application.OpenURL("https://bitduc.com");

            if (GUILayout.Button("Join our Discord"))
                Application.OpenURL("https://discord.gg/mSzv5WDzcZ");
            GUILayout.EndHorizontal();

            GUILayout.Label("© 2025 bit-duc", EditorStyles.centeredGreyMiniLabel);
        }
    }
}
