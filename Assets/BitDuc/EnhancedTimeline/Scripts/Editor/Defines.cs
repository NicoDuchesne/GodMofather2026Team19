using UnityEditor;

namespace BitDuc.EnhancedTimeline.Editor
{
    public static class Defines
    {
        [InitializeOnLoadMethod]
        public static void NetworkInputDefines()
        {
            Support.Editor.Defines.AddSymbols("ENHANCED_TIMELINE");
        }
    }
}
