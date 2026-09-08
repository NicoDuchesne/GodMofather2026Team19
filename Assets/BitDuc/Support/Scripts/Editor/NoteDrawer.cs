namespace BitDuc.Support.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(NoteAttribute))]
    public class NoteDrawer_ : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var note = (NoteAttribute)attribute;
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var fieldRect = new Rect(position.x, position.y, position.width, lineHeight);
            EditorGUI.PropertyField(fieldRect, property, label, true);
            var noteRect = new Rect(position.x, position.y + lineHeight, position.width, lineHeight + 2);
            EditorGUI.HelpBox(noteRect, note.Message, MessageType.Info);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUIUtility.singleLineHeight * 2;
    }
}