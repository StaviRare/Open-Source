using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GLHF.FoldingVariables
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
    public class FoldingVariablesInspectorOverride : Editor
    {
        private bool _showDebugger = false;
        private List<FieldInfo> _fieldsToHide;

        public override void OnInspectorGUI()
        {
            try
            {
                DrawScriptProperty();
                FindDebugAttribute();
                DrawFoldoutHeader();
                DrawFoldoutContent();

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            catch
            {
                DebugProblem();
            }
        }

        private void DrawScriptProperty()
        {
            var scriptProperty = serializedObject.FindProperty("m_Script");

            if (scriptProperty != null)
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
                GUI.enabled = true;
            }
        }
        
        private void FindDebugAttribute()
        {
            _fieldsToHide = new List<FieldInfo>();
            var _bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var _listOfAllFields = serializedObject.targetObject.GetType().GetFields(_bindingFlags);

            for (int i = 0; i < _listOfAllFields.Length; i++)
            {
                var attributeFound = Attribute.GetCustomAttribute(_listOfAllFields[i], typeof(FoldingVariable)) != null;

                if (attributeFound)
                {
                    _fieldsToHide.Add(_listOfAllFields[i]);
                }
                else
                {
                    //if not serielizable (public / serialized field), dont do anything
                    var serializedProperty = serializedObject.FindProperty(_listOfAllFields[i].Name);
                    var propertyFound = serializedProperty != null;

                    if (propertyFound)
                    {
                        EditorGUILayout.PropertyField(serializedProperty, new GUIContent(serializedProperty.displayName));
                    }
                }
            }
        }

        private void DrawFoldoutHeader()
        {
            if (_fieldsToHide.Count == 0)
                return;

            EditorGUILayout.Space();

            var guiStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold
            };

            _showDebugger = EditorGUILayout.Foldout(_showDebugger, "Debugger", true, guiStyle);
        }

        private void DrawFoldoutContent()
        {
            if (!_showDebugger)
                return;

            foreach (var v in _fieldsToHide)
            {
                SerializedProperty serProp = serializedObject.FindProperty(v.Name);
                EditorGUILayout.PropertyField(serProp, new GUIContent(serProp.displayName));
            }
        }

        private void DebugProblem()
        {
            if (serializedObject.targetObject == null)
            {
                Debug.Log("Script refernece is missing!");
                return;
            }

            Debug.Log("New bug! Investigate!");
        }
    }
}