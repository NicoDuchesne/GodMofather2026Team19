using UnityEditor;

namespace BitDuc.EnhancedTimeline.Editor
{
    /// @cond EXCLUDE
    public class AboutWindow : Support.Editor.AboutWindow
    {
        [MenuItem("Tools/Enhanced Timeline/About")]
        public static void ShowNetworkInputAbout() =>
            Show<AboutWindow>("Enhanced Timeline", 1, 1, 0);
    }
}
