using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace NTHiep.MiniOdin
{
    [CustomEditor(typeof(UnityEngine.Object), true)]
    [CanEditMultipleObjects]
    public class ButtonAttributeDrawer : Editor
    {
        private static Dictionary<string, object> _paramCache = new();

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            foreach (var obj in targets)
            {
                DrawButtons(obj);
            }
        }

        private void DrawButtons(object target)
        {
            var type = target.GetType();
            var methods = type.GetMethods(
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            foreach (var method in methods)
            {
                var button = method.GetCustomAttributes(typeof(ButtonAttribute), true)
                                   .FirstOrDefault() as ButtonAttribute;

                if (button == null)
                    continue;

                var parameters = method.GetParameters();
                object[] invokeParams = new object[parameters.Length];

                EditorGUILayout.BeginVertical("box");

                bool supported = true;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = parameters[i];
                    string key = $"{((UnityEngine.Object)target).GetInstanceID()}_{method.Name}_{i}";

                    _paramCache.TryGetValue(key, out object value);

                    //// ===== int =====
                    if (p.ParameterType == typeof(int))
                    {
                        int v = value != null ? (int)value : 0;
                        v = EditorGUILayout.IntField(p.Name, v);
                        value = v;
                    }
                    //// ===== float =====
                    else if (p.ParameterType == typeof(float))
                    {
                        float v = value != null ? (float)value : 0f;
                        v = EditorGUILayout.FloatField(p.Name, v);
                        value = v;
                    }
                    //// ===== string =====
                    else if (p.ParameterType == typeof(string))
                    {
                        string v = value != null ? (string)value : "";
                        v = EditorGUILayout.TextField(p.Name, v);
                        value = v;
                    }
                    //// ===== bool =====
                    else if (p.ParameterType == typeof(bool))
                    {
                        bool v = value != null && (bool)value;
                        v = EditorGUILayout.Toggle(p.Name, v);
                        value = v;
                    }
                    //// ===== UnityEngine.Object =====
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(p.ParameterType))
                    {
                        value = EditorGUILayout.ObjectField(
                            p.Name,
                            value as UnityEngine.Object,
                            p.ParameterType,
                            true
                        );
                    }
                    //// ===== Sprite =====
                    else if (p.ParameterType == typeof(Sprite))
                    {
                        value = EditorGUILayout.ObjectField(
                            p.Name,
                            value as Sprite,
                            typeof(Sprite),
                            false
                        );
                    }
                    //// ===== List<int> =====
                    else if (p.ParameterType == typeof(List<int>))
                    {
                        if (value == null)
                            value = new List<int>();

                        var list = (List<int>)value;

                        EditorGUILayout.LabelField(p.Name);
                        EditorGUI.indentLevel++;

                        int removeIndex = -1;

                        for (int j = 0; j < list.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            list[j] = EditorGUILayout.IntField($"Element {j}", list[j]);

                            if (GUILayout.Button("-", GUILayout.Width(20)))
                                removeIndex = j;

                            EditorGUILayout.EndHorizontal();
                        }

                        if (removeIndex >= 0)
                            list.RemoveAt(removeIndex);

                        if (GUILayout.Button("+ Add int"))
                            list.Add(0);

                        EditorGUI.indentLevel--;
                    }
                    //// ===== List<float> =====
                    else if (p.ParameterType == typeof(List<float>))
                    {
                        if (value == null)
                            value = new List<float>();

                        var list = (List<float>)value;

                        EditorGUILayout.LabelField(p.Name);
                        EditorGUI.indentLevel++;

                        int removeIndex = -1;

                        for (int j = 0; j < list.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();

                            list[j] = EditorGUILayout.FloatField($"Element {j}", list[j]);

                            if (GUILayout.Button("-", GUILayout.Width(20)))
                                removeIndex = j;

                            EditorGUILayout.EndHorizontal();
                        }

                        if (removeIndex >= 0)
                            list.RemoveAt(removeIndex);

                        if (GUILayout.Button("+ Add float"))
                            list.Add(0f);

                        EditorGUI.indentLevel--;
                    }
                    //// ===== List<string> =====
                    else if (p.ParameterType == typeof(List<string>))
                    {
                        if (value == null)
                            value = new List<string>();

                        var list = (List<string>)value;

                        EditorGUILayout.LabelField(p.Name);
                        EditorGUI.indentLevel++;

                        int removeIndex = -1;

                        for (int j = 0; j < list.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();

                            list[j] = EditorGUILayout.TextField($"Element {j}", list[j]);

                            if (GUILayout.Button("-", GUILayout.Width(20)))
                                removeIndex = j;

                            EditorGUILayout.EndHorizontal();
                        }

                        if (removeIndex >= 0)
                            list.RemoveAt(removeIndex);

                        if (GUILayout.Button("+ Add string"))
                            list.Add(string.Empty);

                        EditorGUI.indentLevel--;
                    }
                    //// ===== List<GameObject> =====
                    else if (p.ParameterType == typeof(List<GameObject>))
                    {
                        if (value == null)
                            value = new List<GameObject>();

                        var list = (List<GameObject>)value;

                        EditorGUILayout.LabelField(p.Name);
                        EditorGUI.indentLevel++;

                        int removeIndex = -1;

                        for (int j = 0; j < list.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();

                            list[j] = (GameObject)EditorGUILayout.ObjectField(
                                $"Element {j}",
                                list[j],
                                typeof(GameObject),
                                true
                            );

                            if (GUILayout.Button("-", GUILayout.Width(20)))
                                removeIndex = j;

                            EditorGUILayout.EndHorizontal();
                        }

                        if (removeIndex >= 0)
                            list.RemoveAt(removeIndex);

                        if (GUILayout.Button("+ Add GameObject"))
                            list.Add(null);

                        EditorGUI.indentLevel--;
                    }
                    //// ===== List<Sprite> =====
                    else if (p.ParameterType == typeof(List<Sprite>))
                    {
                        if (value == null)
                            value = new List<Sprite>();

                        var list = (List<Sprite>)value;

                        EditorGUILayout.LabelField(p.Name);
                        EditorGUI.indentLevel++;

                        int removeIndex = -1;

                        for (int j = 0; j < list.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();

                            list[j] = (Sprite)EditorGUILayout.ObjectField(
                                $"Element {j}",
                                list[j],
                                typeof(Sprite),
                                false
                            );

                            if (GUILayout.Button("-", GUILayout.Width(20)))
                                removeIndex = j;

                            EditorGUILayout.EndHorizontal();
                        }

                        if (removeIndex >= 0)
                            list.RemoveAt(removeIndex);

                        if (GUILayout.Button("+ Add Sprite"))
                            list.Add(null);

                        EditorGUI.indentLevel--;
                    }

                    else
                    {
                        supported = false;
                        EditorGUILayout.HelpBox(
                            $"Parameter '{p.Name}' type '{p.ParameterType.Name}' is not supported.",
                            MessageType.Warning
                        );
                    }

                    _paramCache[key] = value;
                    invokeParams[i] = value;
                }


                string label = string.IsNullOrEmpty(button.Label)
                    ? method.Name
                    : button.Label;

                GUI.enabled = supported;

                if (GUILayout.Button(label))
                {
                    try
                    {
                        method.Invoke(
                            method.IsStatic ? null : target,
                            invokeParams
                        );

                        if (target is UnityEngine.Object uObj)
                            EditorUtility.SetDirty(uObj);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }

                GUI.enabled = true;
                EditorGUILayout.EndVertical();
            }
        }
    }
}
