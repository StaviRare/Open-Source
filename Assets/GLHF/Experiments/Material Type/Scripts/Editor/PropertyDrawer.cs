using UnityEditor;
using UnityEngine;

namespace GLHF.MaterialType
{
    public class MaterialTypeDrawer : MaterialPropertyDrawer
    {
        public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
        {
            EditorGUI.BeginChangeCheck();

            var selectedType = (MaterialType)prop.floatValue;
            selectedType = (MaterialType)EditorGUI.EnumPopup(position, label, selectedType);

            if (EditorGUI.EndChangeCheck())
            {
                prop.floatValue = (float)selectedType;
            }
        }
    }
}