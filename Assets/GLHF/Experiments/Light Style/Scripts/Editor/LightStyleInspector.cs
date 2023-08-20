using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GLHF.LightStyles
{
    [CustomEditor(typeof(LightStyle)), CanEditMultipleObjects]
    public class LightStyleInspector : Editor
    {
        private SerializedProperty _valueProperty;
        private SerializedProperty _speedProperty;

        private void OnEnable()
        {
            _valueProperty = serializedObject.FindProperty("value");
            _speedProperty = serializedObject.FindProperty("_speed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawScriptProperty();
            DrawHelpBox();
            EditorGUILayout.PropertyField(_speedProperty);
            DrawLightStyleOptions();
        }

        private void DrawLightStyleOptions()
        {
            // Get the latest light style list
            var ligthStyleData = LightStyleDataFileHandler.DataFile;
            var dataFileExists = ligthStyleData != null;

            if (dataFileExists)
            {
                // Create and sort a dictionary from the data file
                var dictionary = new Dictionary<string, string>();
                var sortedProfileItems = ligthStyleData.Items.OrderBy(x => x.Name);

                // Add a custom option (apart from the options from the data file)
                dictionary.Add("Custom", "");

                // Verify the integrity of the data file item (add them to the option list)
                foreach (var v in sortedProfileItems)
                {
                    var nameIsEmpty = String.IsNullOrEmpty(v.Name);
                    var valueIsEmpty = String.IsNullOrEmpty(v.Value);
                    var canAddItem = !nameIsEmpty && !valueIsEmpty;

                    if (canAddItem)
                    {
                        dictionary.Add(v.Name, v.Value);
                    }
                }

                // Automatic light style detection using an existing value
                var valueDictionaryIndex = dictionary.Values.ToList().IndexOf(_valueProperty.stringValue);
                var index = (int)Mathf.Clamp(valueDictionaryIndex, 0, Mathf.Infinity);
                var currentValueIsPartOfDictionary = index != 0;
                var options = dictionary.Keys.ToArray();

                // Set current index and value
                index = EditorGUILayout.Popup("Type", index, options);
                var isCustomLightStyle = index == 0;

                if (isCustomLightStyle)
                {
                    if (currentValueIsPartOfDictionary)
                    {
                        _valueProperty.stringValue = "";
                    }

                    EditorGUILayout.PropertyField(_valueProperty, new GUIContent("Custom Value"));
                }
                else
                {
                    _valueProperty.stringValue = dictionary.ElementAt(index).Value;
                    EditorGUILayout.PropertyField(_valueProperty, new GUIContent("Default Value"));
                }

                // Remove non letter characters from a string 
                var finalValue = _valueProperty.stringValue;
                var valueAfterNonLetterRemoval = Regex.Replace(finalValue, @"[^A-Za-z]+", "");

                // Set final string value
                _valueProperty.stringValue = valueAfterNonLetterRemoval;
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawScriptProperty()
        {
            var scriptPropertyName = "m_Script";
            var scriptProperty = serializedObject.FindProperty(scriptPropertyName);

            if (scriptProperty != null)
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(scriptProperty);
                GUI.enabled = true;
            }
        }

        private void DrawHelpBox()
        {
            var text = "This component captures the attached light's intensity during Awake, preventing real-time updates to the original intensity.";
            EditorGUILayout.HelpBox(text, MessageType.Info);
        }
    }
}
