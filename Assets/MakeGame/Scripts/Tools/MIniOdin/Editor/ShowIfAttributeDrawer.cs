using UnityEditor;
using UnityEngine;

namespace NTHiep.MiniOdin
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;

            SerializedProperty conditionProp =
                property.serializedObject.FindProperty(showIf.fieldName);

            if (conditionProp == null)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            if (conditionProp.propertyType == SerializedPropertyType.Enum &&
                conditionProp.enumValueIndex == showIf.enumValue)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            SerializedProperty conditionProp =
                property.serializedObject.FindProperty(showIf.fieldName);

            if (conditionProp == null)
                return EditorGUI.GetPropertyHeight(property, label, true);

            if (conditionProp.propertyType == SerializedPropertyType.Enum &&
                conditionProp.enumValueIndex == showIf.enumValue)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            return 0f; // ẩn hẳn
        }
    }

}