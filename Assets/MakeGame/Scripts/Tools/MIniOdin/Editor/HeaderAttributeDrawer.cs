using UnityEditor;
using UnityEngine;


namespace NTHiep.MiniOdin
{
    [CustomPropertyDrawer(typeof(ColorHeaderAttribute))]
    public class ColorHeaderAttributeDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight + 6;
        }

        public override void OnGUI(Rect position)
        {
            var header = (ColorHeaderAttribute)attribute;

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel)
            {
                richText = true
            };

            position.y += 2;
            EditorGUI.LabelField(position, header.text, style);
        }
    }


}