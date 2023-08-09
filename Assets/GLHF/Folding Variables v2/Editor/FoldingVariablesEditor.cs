using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace GLHF.FoldingVariables2
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class FoldingVariablesEditor : Editor
    {
        private SerializedProperty[] _otherProperties;
        private Dictionary<string, List<SerializedProperty>> _foldoutGroups = new Dictionary<string, List<SerializedProperty>>();
        private Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();

        private void OnEnable()
        {
            var fields = serializedObject.targetObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var otherList = new List<SerializedProperty>();

            foreach (var field in fields)
            {
                var property = serializedObject.FindProperty(field.Name);

                if (property != null)
                {
                    var hideInInspectorAttribute = field.GetCustomAttribute<HideInInspector>();
                    var foldoutAttribute = field.GetCustomAttribute<FoldingVariable>();

                    if (hideInInspectorAttribute == null) // Skip fields marked with [HideInInspector]
                    {
                        if (foldoutAttribute != null)
                        {
                            if (!_foldoutGroups.ContainsKey(foldoutAttribute.foldoutName))
                            {
                                _foldoutGroups[foldoutAttribute.foldoutName] = new List<SerializedProperty>();
                                _foldoutStates[foldoutAttribute.foldoutName] = true;
                            }

                            _foldoutGroups[foldoutAttribute.foldoutName].Add(property);
                        }
                        else
                        {
                            otherList.Add(property);
                        }
                    }
                }
            }

            _otherProperties = otherList.ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawRegularProperties();
            DrawFoldingVariables();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawRegularProperties()
        {
            foreach (var property in _otherProperties)
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        private void DrawFoldingVariables()
        {
            foreach (var foldoutGroup in _foldoutGroups)
            {
                var foldoutName = foldoutGroup.Key;
                var properties = foldoutGroup.Value;

                _foldoutStates[foldoutName] = EditorGUILayout.Foldout(_foldoutStates[foldoutName], foldoutName, true);

                if (_foldoutStates[foldoutName])
                {
                    EditorGUI.indentLevel++;

                    foreach (var property in properties)
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }
    }
}
