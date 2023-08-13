using UnityEngine;
using UnityEditor;
using System;

public class AutoGrid : EditorWindow
{
    [MenuItem("GLHF/Auto Grid", false, 0)]
    public static void OpenTheThing() => GetWindow<AutoGrid>("Auto Grid");

    private float _snapStep = 1f;
    private float _incrementStep = 1f;
    private bool _enableSnap = false;
    private SnapType _snapType = SnapType.Snap;
    private Vector3[] _objectLocationList = new Vector3[0];
    private GameObject[] _storedSelectedObjects = new GameObject[0];

    // Shortcuts
    private const KeyCode _snapEnableToggleKey = KeyCode.F;
    private const KeyCode _snapTypeToggleKey = KeyCode.G;
    private const EventModifiers _snapToggleModifier = EventModifiers.Control;

    private enum SnapType
    {
        Snap,
        Increment
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void Update()
    {
        SnapSelection();
    }

    private void OnGUI()
    {
        EditorGUIUtility.labelWidth = 100;

        DrawEnableToggle();

        _snapType = (SnapType)EditorGUILayout.EnumPopup("Type:", _snapType);
        _snapStep = EditorGUILayout.FloatField("Snap Step", _snapStep);
        _incrementStep = EditorGUILayout.FloatField("Increment Step", _incrementStep);
        _snapStep = Mathf.Clamp(_snapStep, 0.1f, Mathf.Infinity);

        DrawShortcutHints();
    }

    private void DrawEnableToggle()
    {
        var defaultColor = GUI.color;
        GUI.color = _enableSnap ? Color.green : Color.red;
        
        if (GUILayout.Button("Enabled: " + _enableSnap))
        {
            _enableSnap = !_enableSnap;
        }

        GUI.color = defaultColor;
    }

    private void DrawShortcutHints()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Shortcuts:", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox("These shortcuts are applicable exclusively within the scene view.", MessageType.Info);

        DrawShortcutRow("Toggle Enable", _snapToggleModifier, _snapEnableToggleKey);
        DrawShortcutRow("Toggle Type", _snapToggleModifier, _snapTypeToggleKey);
    }

    private void DrawShortcutRow(string actionLabel, EventModifiers eventModifiers, KeyCode keyCode)
    {
        var viewWidth = EditorGUIUtility.currentViewWidth / 2;
        var shortcutLabel = string.Format("{0} + {1}", eventModifiers, keyCode);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Box(actionLabel, GUILayout.Width(viewWidth));
        GUILayout.Box(shortcutLabel, GUILayout.Width(viewWidth));
        EditorGUILayout.EndHorizontal();
    }

    private void SnapSelection()
    {
        var selectedObjects = Selection.gameObjects;
        var refreshStoredObjects = AreArraysNotEqual(_storedSelectedObjects, selectedObjects);

        if (refreshStoredObjects)
        {
            _storedSelectedObjects = selectedObjects;
            _objectLocationList = new Vector3[_storedSelectedObjects.Length];

            for (int i = 0; i < selectedObjects.Length; i++)
            {
                _objectLocationList[i] = selectedObjects[i].transform.position;
            }
        }

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            if (_enableSnap)
            {
                Undo.RecordObject(selectedObjects[i].transform, "Snap Object");

                if (_snapType == SnapType.Increment)
                {
                    var storedPosition = _objectLocationList[i];
                    var currentPosition = selectedObjects[i].transform.position;

                    if (currentPosition.Equals(storedPosition) == false)
                    {
                        selectedObjects[i].transform.position = RoundIncrement(currentPosition, storedPosition);
                    }
                }
                else
                {
                    selectedObjects[i].transform.position = RoundSnap(selectedObjects[i].transform.position);
                }
            }

            _objectLocationList[i] = selectedObjects[i].transform.position;
        }
    }

    private Vector3 RoundSnap(Vector3 v)
    {
        var rounded = new Vector3(
            Mathf.RoundToInt(v.x / _snapStep),
            Mathf.RoundToInt(v.y / _snapStep),
            Mathf.RoundToInt(v.z / _snapStep));

        var returnValue = rounded * _snapStep;

        return returnValue;
    }

    private Vector3 RoundIncrement(Vector3 currentPosition, Vector3 storedPosition)
    {
        var returnValue = storedPosition;
        var positionDifference = currentPosition - storedPosition;

        // Round position difference to nearest multiple of _step
        positionDifference = new Vector3(
            Mathf.Round(positionDifference.x / _incrementStep) * _incrementStep,
            Mathf.Round(positionDifference.y / _incrementStep) * _incrementStep,
            Mathf.Round(positionDifference.z / _incrementStep) * _incrementStep);

        // Apply rounded position difference to final position
        returnValue += positionDifference;

        return returnValue;
    }

    private bool AreArraysNotEqual(GameObject[] array1, GameObject[] array2)
    {
        if (array1 == null || array2 == null || array1.Length != array2.Length)
        {
            // If either array is null or they have different lengths, they are not equal
            return true;
        }

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
            {
                // If any element of the arrays is different, they are not equal
                return true;
            }
        }

        // If all elements of the arrays are equal, they are equal
        return false;
    }

    private void HandleShortcut(KeyCode keyCode, Action action)
    {
        var currentEvent = Event.current;

        if (currentEvent.type == EventType.KeyDown)
        {
            var isToggleKey = currentEvent.keyCode == keyCode;
            var isModifierPressed = currentEvent.modifiers == _snapToggleModifier;
            var isShortcutPressed = isToggleKey && isModifierPressed;

            if (isShortcutPressed)
            {
                action.Invoke();
                currentEvent.Use();
            }
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        HandleShortcut(_snapEnableToggleKey, OnSnapToggleShortcut);
        HandleShortcut(_snapTypeToggleKey, OnSnapChangeShortcut);
        Repaint();
    }

    private void OnSnapToggleShortcut()
    {
        _enableSnap = !_enableSnap;
    }

    private void OnSnapChangeShortcut()
    {
        _snapType = _snapType == SnapType.Snap 
            ? SnapType.Increment 
            : SnapType.Snap;
    }
}
