using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace BitDuc.Support.Editor
{
    public static class Defines
    {
        public static void AddSymbols(params string[] toDefine)
        {
            var (buildTargetGroup, currentDefines) = GetBuildSettings();
            var currentDefineSet = currentDefines.Split(';');
            var newDefines = string.Join(';', currentDefineSet.Concat(toDefine).Distinct());

            if (newDefines == currentDefines)
                return;

            PlayerSettings.SetScriptingDefineSymbols(buildTargetGroup, newDefines);
        }

        static (NamedBuildTarget, string) GetBuildSettings()
        {
            var selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var buildTargetGroup = NamedBuildTarget.FromBuildTargetGroup(selectedBuildTargetGroup);
            var currentDefines = PlayerSettings.GetScriptingDefineSymbols(buildTargetGroup);
            return (buildTargetGroup, currentDefines);
        }

        static void Show(string message, IEnumerable<string> strings) =>
            Debug.Log($"{message}: {string.Join(", ", strings)}");
    }
}
