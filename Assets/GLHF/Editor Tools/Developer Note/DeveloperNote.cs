using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class DeveloperNote : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string _note;

    [Header("OnGUI")]
    [SerializeField] private bool _showOnGUI = true;
    [SerializeField] private bool _backgroundOnGUI = false;

    [Header("OnSceneView")]
    [SerializeField] private bool _showOnSceneView = true;
    [SerializeField] private bool _backgroundOnSceneView = false;

    [Header("Positioning")]
    public Vector3 _offset = Vector3.zero;

    private GUIStyle _guiStyle;

    private void OnGUI()
    {
        if (_showOnGUI)
        {
            var noteScale = CalculateNoteScale();
            var objectScreenPosition = Camera.main.WorldToScreenPoint(transform.position + _offset);
            var textRect = CalculateTextRectGUI(objectScreenPosition, noteScale);

            if (_backgroundOnGUI)
            {
                DrawBackground(textRect);
            }

            GUI.Label(textRect, _note, GetGUIStyle());
        }
    }

    private GUIStyle GetGUIStyle()
    {
        if (_guiStyle == null)
        {
            _guiStyle = new GUIStyle
            {
                normal = { textColor = Color.white },
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter
            };
        }
        return _guiStyle;
    }

    private Rect CalculateTextRectGUI(Vector3 objectScreenPosition, Vector2 noteScale)
    {
        var rectPosY = Screen.height - (objectScreenPosition.y + noteScale.y / 2);
        var rectPosX = objectScreenPosition.x - noteScale.x / 2;
        return new Rect(rectPosX, rectPosY, noteScale.x, noteScale.y);
    }

    private Vector2 CalculateNoteScale()
    {
        return GetGUIStyle().CalcSize(new GUIContent(_note));
    }

    private void DrawBackground(Rect textRect)
    {
        var addPadding = 10;
        var backgroundRect = new Rect(textRect.x - (addPadding / 2), textRect.y - (addPadding / 2), textRect.width + addPadding, textRect.height + addPadding);
        GUI.Box(backgroundRect, "");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_showOnSceneView)
        {
            Handles.BeginGUI();

            var noteScale = CalculateNoteScale();
            var objectScreenPosition = HandleUtility.WorldToGUIPoint(transform.position + _offset);
            var textRect = CalculateTextRectSceneView(objectScreenPosition, noteScale);

            if (_backgroundOnSceneView)
            {
                DrawBackground(textRect);
            }

            GUI.Label(textRect, _note, GetGUIStyle());

            Handles.EndGUI();
        }
    }

    private Rect CalculateTextRectSceneView(Vector3 objectScreenPosition, Vector2 noteScale)
    {
        var rectPosY = objectScreenPosition.y - (noteScale.y / 2);
        var rectPosX = objectScreenPosition.x - noteScale.x / 2;
        return new Rect(rectPosX, rectPosY, noteScale.x, noteScale.y);
    }
#endif
}