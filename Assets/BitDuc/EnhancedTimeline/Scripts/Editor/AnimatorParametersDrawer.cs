using BitDuc.EnhancedTimeline.Animator;
using UnityEditor;
using UnityEngine;

namespace BitDuc.EnhancedTimeline.Editor
{
    /// @cond EXCLUDE
    [CustomPropertyDrawer(typeof(AnimatorParameterBehaviour))]
    internal class AnimatorParametersDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight (SerializedProperty property, GUIContent label) =>
            EditorGUIUtility.singleLineHeight;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            var parameterNameProperty = FindParameterNameProperty(property);
            EditorGUILayout.PropertyField(parameterNameProperty);

            var (parameterType, parameterTypeProperty) = FindParameterTypeProperty(property);
            EditorGUILayout.PropertyField(parameterTypeProperty);

            if (ValuePropertyForType(property, parameterType, out var valueProperty))
                EditorGUILayout.PropertyField( valueProperty);

            if (PostEndPropertyForType(property, parameterType, out var postEndValueProperty))
                EditorGUILayout.PropertyField(postEndValueProperty);
        }

        static SerializedProperty FindParameterNameProperty(SerializedProperty property) =>
            property.FindPropertyRelative("parameterName");

        (AnimatorParameterType, SerializedProperty) FindParameterTypeProperty(SerializedProperty root)
        {
            var property = root.FindPropertyRelative("parameterType");
            var parameterType = (AnimatorParameterType)property.enumValueIndex;
            return (parameterType, property);
        }
        
        static bool ValuePropertyForType(
            SerializedProperty root,
            AnimatorParameterType parameterType,
            out SerializedProperty valueProperty
        ) {
            valueProperty = parameterType switch {
                AnimatorParameterType.Boolean => root.FindPropertyRelative("booleanValue"),
                AnimatorParameterType.Integer => root.FindPropertyRelative("integerValue"),
                _ => null
            };

            return valueProperty != null;
        }

        static bool PostEndPropertyForType(
            SerializedProperty root,
            AnimatorParameterType parameterType,
            out SerializedProperty valueProperty
        ) {
            valueProperty = parameterType switch {
                AnimatorParameterType.Boolean => root.FindPropertyRelative("postEndValue"),
                AnimatorParameterType.Integer => root.FindPropertyRelative("postEndValue"),
                AnimatorParameterType.Float => root.FindPropertyRelative("postEndValue"),
                AnimatorParameterType.Trigger => root.FindPropertyRelative("resetAtEnd"),
                _ => null
            };

            return valueProperty != null;
        }
    }
}
