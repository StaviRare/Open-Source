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
        private const string _foldoutHeader = "Debugger";

        public override void OnInspectorGUI()
        {
            try
            {
                DrawScriptProperty();
                FindFoldingVariableAttributes();
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
        
        private void FindFoldingVariableAttributes()
        {
            _fieldsToHide = new List<FieldInfo>();
            var _bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var _listOfAllFields = serializedObject.targetObject.GetType().GetFields(_bindingFlags);

            for (int i = 0; i < _listOfAllFields.Length; i++)
            {
                CheckFieldAttributes(_listOfAllFields[i]);
            }
        }

        private void CheckFieldAttributes(FieldInfo fieldInfo)
        {
            var variableIsHidden = Attribute.GetCustomAttribute(fieldInfo, typeof(HideInInspector)) != null;

            if (variableIsHidden)
                return;

            var foldingVariableFound = Attribute.GetCustomAttribute(fieldInfo, typeof(FoldingVariable)) != null;

            if (foldingVariableFound)
            {
                _fieldsToHide.Add(fieldInfo);
                return;
            }

            // If not serielizable (public / serialized field), dont do anything
            var serializedProperty = serializedObject.FindProperty(fieldInfo.Name);
            var propertyFound = serializedProperty != null;

            if (propertyFound)
            {
                EditorGUILayout.PropertyField(serializedProperty, new GUIContent(serializedProperty.displayName));
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

            _showDebugger = EditorGUILayout.Foldout(_showDebugger, _foldoutHeader, true, guiStyle);
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