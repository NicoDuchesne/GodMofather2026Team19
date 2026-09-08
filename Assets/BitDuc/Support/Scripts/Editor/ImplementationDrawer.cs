using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BitDuc.Support.Editor
{
    [CustomPropertyDrawer(typeof(Implementation<,>))]
    public class ImplementationDrawer : PropertyDrawer
    {
        Type defaultType;
        Dictionary<string, Type> namedImplementations;
        string[] indexedImplementationNames;
        string[] indexedImplementationFullNames;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Setup();
            var implementation = property.FindPropertyRelative("implementation");
            var typeName = property.FindPropertyRelative("type");
            var type = GetValidTypeOrSetDefault(typeName);
            ShowObjectField(label, type, implementation);
            ShowImplementationField(typeName);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0;

        Type GetValidTypeOrSetDefault(SerializedProperty typeName)
        {
            if (!namedImplementations.TryGetValue(typeName.stringValue, out var type))
                type = defaultType;

            typeName.stringValue = type.FullName;
            return type;
        }
 
        void Setup()
        {
            if (namedImplementations != null)
                return;

            var typeArguments = fieldInfo.FieldType.GetGenericArguments();
            var abstractType = typeArguments[0];

            defaultType = typeArguments[1];
            namedImplementations = abstractType.Implementations().ToDictionary(type => type.FullName);
            indexedImplementationFullNames = namedImplementations.Keys.ToArray();
            indexedImplementationNames = namedImplementations.Values.Select(impl => impl.Name).ToArray();
        }

        static void ShowObjectField(GUIContent label, Type type, SerializedProperty implementation)
        {
            if (!type.IsInstanceOfType(implementation.objectReferenceValue))
                implementation.objectReferenceValue = null;

            implementation.objectReferenceValue =
                EditorGUILayout.ObjectField(label, implementation.objectReferenceValue, type, true);
        }

        void ShowImplementationField(SerializedProperty typeName)
        {
            var selected = Array.IndexOf(indexedImplementationFullNames, typeName.stringValue);
            selected = EditorGUILayout.Popup("    Use: ", selected, indexedImplementationNames);
            typeName.stringValue = indexedImplementationFullNames[selected];
        }
    }
}
