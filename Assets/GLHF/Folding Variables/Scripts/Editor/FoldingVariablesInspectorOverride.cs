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
        private static bool showFoldingVariables = false;
        private const string foldoutHeader = "Folded Variables";
        private const string scriptPropertyName = "m_Script";

        public override void OnInspectorGUI()
        {
            if (serializedObject.targetObject == null)
            {
                Debug.Log("Script refernece is missing!");
                return;
            }

            OverrideInspector();
        }

        private void OverrideInspector()
        {
            serializedObject.Update();

            var foldingProperties = GetFoldingProperties();

            if (foldingProperties.Length == 0)
            {
                DrawDefaultInspector();
                return;
            }

            var foldingPropartyNameArray = GetFoldingPropertyNames(foldingProperties);

            DrawScriptProperty();
            DrawPropertiesExcluding(serializedObject, foldingPropartyNameArray);
            DrawFoldingToggle();
            DrawFoldingContent(foldingProperties);
            serializedObject.ApplyModifiedProperties();
        }

        private SerializedProperty[] GetFoldingProperties()
        {
            var foldingVariables = new List<SerializedProperty>();
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var scriptFieldList = serializedObject.targetObject.GetType().GetFields(bindingFlags);

            for (int i = 0; i < scriptFieldList.Length; i++)
            {
                var fieldInfo = scriptFieldList[i];
                var foldingVariableFound = Attribute.GetCustomAttribute(fieldInfo, typeof(FoldingVariable)) != null;

                if (foldingVariableFound)
                {
                    var variableIsNotHidden = Attribute.GetCustomAttribute(fieldInfo, typeof(HideInInspector)) == null;

                    if (variableIsNotHidden)
                    {
                        var propertName = serializedObject.FindProperty(fieldInfo.Name);
                        foldingVariables.Add(propertName);
                    }
                }
            }

            return foldingVariables.ToArray();
        }

        private string[] GetFoldingPropertyNames(SerializedProperty[] propertyArray)
        {
            var propertyNames = new List<string>
            {
                scriptPropertyName // Script property not being disabled on DrawPropertiesExcluding fix
            };

            foreach (var property in propertyArray)
            {
                propertyNames.Add(property.name);
            }

            return propertyNames.ToArray();
        }

        private void DrawScriptProperty()
        {
            var scriptProperty = serializedObject.FindProperty(scriptPropertyName);

            if (scriptProperty == null)
            {
                Debug.Log("Could not find script property!");
                return;
            }

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(scriptPropertyName));
            GUI.enabled = true;
        }

        private void DrawFoldingToggle()
        {
            EditorGUILayout.Space();

            var guiStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold
            };

            showFoldingVariables = EditorGUILayout.Foldout(showFoldingVariables, foldoutHeader, true, guiStyle);
        }

        private void DrawFoldingContent(SerializedProperty[] propertyArray)
        {
            if (!showFoldingVariables)
                return;

            foreach (var property in propertyArray)
            {
                EditorGUILayout.PropertyField(property, new GUIContent(property.displayName));
            }
        }
    }
}