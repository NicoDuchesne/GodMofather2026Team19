using BitDuc.EnhancedTimeline.Markers;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace BitDuc.EnhancedTimeline.Editor
{
    [CustomTimelineEditor(typeof(LoopToMarker))]
    public class LoopToMarkerEditor : MarkerEditor
    {
        public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
        {
            if (marker is not LoopToMarker)
                return;

            DrawLineOverlay(region);
        }

        static void DrawLineOverlay(MarkerOverlayRegion region)
        {
            var overlayLineRect = new Rect(
                region.markerRegion.x + region.markerRegion.width * .5f, region.timelineRegion.y,
                1f, region.timelineRegion.height
            );
            var color = new Color(1f, 1f, 1f, .5f);
            EditorGUI.DrawRect(overlayLineRect, color);
        }
    }
}
